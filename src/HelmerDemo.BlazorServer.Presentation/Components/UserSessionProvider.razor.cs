using System.Net;
using System.Reactive.Linq;
using System.Security.Cryptography;
using HelmerDemo.BlazorServer.Application.Actors;
using HelmerDemo.BlazorServer.Application.Reactive;
using HelmerDemo.BlazorServer.Presentation.JsInterop.Contracts;
using HelmerDemo.BlazorServer.Presentation.ViewModel;
using HelmerDemo.BlazorServer.Shared.Tools;
using Microsoft.AspNetCore.Components;
using Serilog;

namespace HelmerDemo.BlazorServer.Presentation.Components;

// A user session provider for shared variables between pages, and between page navigations.
public partial class UserSessionProvider : ComponentBase, IDisposable
{
	private IDisposable _userFoundSubscription;
	private IDisposable _userNotFoundSubscription;
	private IDisposable _userCreatedSubscription;

	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	[Inject]
	public ILocalStorageProvider LocalStorageProvider { get; set; } = default!;

	[CascadingParameter]
	public required Root RootContainer { get; set; }

	public UserActor? MyUserActor { get; set; } = default!;

	private UserSessionViewModel UserSessionContent { get; set; } = new UserSessionViewModel();

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			// TODO: If a localstorage is not available in the browser .... => Solve this in your LocalStorageProvider

			var getFromLocalStorage = Observable.FromAsync(() => LocalStorageProvider.GetAsync<UserSessionKey>("helmerdemo-blazor-session"));

			getFromLocalStorage.Subscribe(content => Initialize(content),
				onError: ex => HandleError(ex),
				() => Log.Information("Get Completed"));
		}

		base.OnAfterRender(firstRender);
	}

	private void Initialize(Result<UserSessionKey> userSession)
	{
		// Rules =>  Action

		// 1: No Id found => show child (Counter) and set UserId
		var notFoundRule = userSession.StatusCode.Equals(HttpStatusCode.NotFound);
		if (notFoundRule)
		{
			SetUserSession();
			return;
		}

		// 3: Id found => Call the UserSessionActor to validate Id
		if (userSession.IsSuccess)
		{
			// TODO: Investigate if these subscriptions are disposed correctly
			
			_userFoundSubscription = RootContainer.MyRootActor.WhenNewMessageSent
				.Where(act => act
					.IsFoundUser() && act.Address == userSession.Value.UserId)
				.Subscribe(action =>
					{
						// Rule: if in store => Update UserState to ChildContent
					_userNotFoundSubscription.Dispose();
					SetMyUserActor(action);
					}, onError: ex => HandleError(ex),
					() => Log.Information("Found Completed"));
			
			_userNotFoundSubscription = RootContainer.MyRootActor.WhenNewMessageSent
				.Where(act => act
					.UserNotFound() && act.Address == userSession.Value.UserId)
				.Subscribe(action =>
				{
					// Rule: if not in store => Delete from LocalStorage, and set new.
					_userFoundSubscription.Dispose();
					var deleteFromLocalStorage = Observable.FromAsync(() => LocalStorageProvider.DeleteAsync("helmerdemo-blazor-session").AsTask());

					deleteFromLocalStorage.Subscribe(content => SetUserSession(),
						onError: ex => HandleError(ex),
						() => Log.Information("Delete Completed"));
				});
			
			RootContainer.MyRootActor.Post(new ActorAction("get", "user", userSession.Value.UserId));
			
			return;
		}

		ShowErrorPage("Another error while looking for Id");
	}

	private void SetMyUserActor(ActorAction action)
	{
		MyUserActor = RootContainer.MyRootActor.FindById(action.Address) as UserActor;
		MyUserActor?.WhenNewMessageSent.Where(act=>act.CreatedCounter() && act.Address == action.Address).Subscribe(_ =>
		{
			// Rule: if in store => Update UserState to ChildContent
			MyUserActor = RootContainer.MyRootActor.FindById(action.Address) as UserActor;
			UpdateState(UserSessionState.Active);
		});
		UpdateState(UserSessionState.Active);
	}

	private void DisposeUserSubscriptions()
	{
		_userFoundSubscription.Dispose();
		_userNotFoundSubscription.Dispose();
	}

	private void SetUserSession()
	{
		var userSessionKey = new UserSessionKey
		{
			UserId = Guid.NewGuid()
		};
		
		var setLocalStorage = Observable.FromAsync(() => LocalStorageProvider.SetAsync("helmerdemo-blazor-session", userSessionKey).AsTask());

		setLocalStorage.Subscribe(content =>
			{
				_userCreatedSubscription = RootContainer.MyRootActor.WhenNewMessageSent
					.Where(act => act
						.UserCreated() && act.Address == userSessionKey.UserId)
					.Subscribe(action =>
					{
						SetMyUserActor(action);
					}, onError: ex => HandleError(ex),
					() => Log.Information("Created Completed"));
				RootContainer.MyRootActor.Post(new ActorAction("set", "user", userSessionKey.UserId));
			},
			onError: ex => HandleError(ex),
			() => Log.Information("Set Completed"));
	}

	private void HandleError(Exception ex)
	{
		// 2: In case of a cryptographic-error => remove the stored data, show AskQuestion
		if (ex.GetType() == typeof(CryptographicException))
		{
			var deleteLocalStore = Observable.FromAsync(() => LocalStorageProvider.DeleteAsync("helmerdemo-blazor-session").AsTask());

			deleteLocalStore.Subscribe(
				_ => UpdateState(UserSessionState.Active),
				onError: exc => Log.Error(exc, "Error deleting the local storage"),
				() => Log.Information("Delete Completed"));
			return;
		}

		ShowErrorPage("generic error occured");
	}

	private void ShowErrorPage(string errorMessage)
	{
		UserSessionContent.State = UserSessionState.Error;
		UserSessionContent.SystemMessage = errorMessage;
		InvokeAsync(StateHasChanged);
	}

	private void UpdateState(UserSessionState state)
	{
		UserSessionContent.State = state;
		InvokeAsync(StateHasChanged);
	}

	public void Dispose()
	{
		_userFoundSubscription?.Dispose();
		_userNotFoundSubscription?.Dispose();
		_userCreatedSubscription?.Dispose();
	}
}
