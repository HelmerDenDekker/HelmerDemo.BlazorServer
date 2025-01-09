using System.Net;
using System.Reactive.Concurrency;
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

			getFromLocalStorage.Subscribe(content => ProcessLocalId(content),
				onError: ex => HandleError(ex),
				() => Log.Information("Get From Localstorage Completed"));
		}

		base.OnAfterRender(firstRender);
	}

	private void ProcessLocalId(Result<UserSessionKey> userSession)
	{
		
		// 1: No Id found => show child (Counter) and set UserId
		if (userSession.StatusCode.Equals(HttpStatusCode.NotFound))
		{
			Log.Debug("%%%%% No user session found");
			CheckLocalStorageEnabled();
			return;
		}

		// 2: Id found => Get the UserSessionActor
		if (userSession.IsSuccess)
		{
			Log.Debug("%%%%% User session found");
			// TODO: Investigate if these subscriptions are disposed correctly
			
			_userFoundSubscription = RootContainer.MyRootActor.WhenNewMessageSent
				.Where(act => act
					.IsFoundUser() && act.Address == userSession.Value.UserId)
				.Subscribe(action =>
					{
						// Rule: if in store => Update UserState to ChildContent
					_userNotFoundSubscription.Dispose();
					Initialize(action.Address);
					}, onError: ex => HandleError(ex),
					() => Log.Information("Found User Completed"));
			
			_userNotFoundSubscription = RootContainer.MyRootActor.WhenNewMessageSent
				.Where(act => act
					.UserNotFound() && act.Address == userSession.Value.UserId)
				.Subscribe(action =>
				{
					// Rule: if not in store => Delete from LocalStorage, and set new.
					_userFoundSubscription.Dispose();
					var deleteFromLocalStorage = Observable.FromAsync(() => LocalStorageProvider.DeleteAsync("helmerdemo-blazor-session").AsTask());

					deleteFromLocalStorage.Subscribe(content => CheckLocalStorageEnabled(),
						onError: ex => HandleError(ex),
						() => Log.Information("Delete User Completed"));
				});
			
			RootContainer.MyRootActor.Post(new ActorAction("get", "user", userSession.Value.UserId));
			
			return;
		}

		ShowErrorPage("Another error while looking for Id");
	}

	private void Initialize(Guid userId)
	{
		Log.Debug("%%%%% Initializing UserActor");
		MyUserActor = RootContainer.MyRootActor.FindById(userId) as UserActor;
		// I am doing this cascading shizzle like this. I am not sure if this is the right way to do it.
		MyUserActor?.WhenNewMessageSent.Where(act=>act.CreatedCounter() && act.Address == userId).Subscribe(_ =>
		{
			// Rule: if in store => Update UserState to ChildContent
			MyUserActor = RootContainer.MyRootActor.FindById(userId) as UserActor;
			UpdateState(UserSessionState.Active);
		});
		UpdateState(UserSessionState.Active);
	}

	private void DisposeUserSubscriptions()
	{
		_userFoundSubscription.Dispose();
		_userNotFoundSubscription.Dispose();
	}

	private void CheckLocalStorageEnabled()
	{
		Log.Debug("%%%%% Checking if local storage is enabled");
		var enabledObservable = Observable.FromAsync(() => LocalStorageProvider.IsEnabled());
		enabledObservable.Subscribe(enabled =>
		{
			if (!enabled)
			{
				ShowErrorPage("Local storage is not enabled");
				return;
			}

			SetUserSessionKey();
		});
	}

	private void SetUserSessionKey()
		{
		var userSessionKey = new UserSessionKey
		{
			UserId = Guid.NewGuid()
		};
		
		Log.Debug("%%%%% Setting user session with key in LocalStorage {key}", userSessionKey.UserId);
		
		var setLocalStorage = Observable.FromAsync(() => LocalStorageProvider.SetAsync("helmerdemo-blazor-session", userSessionKey).AsTask());

		setLocalStorage.Subscribe(content =>
			{
				WaitForOthers(userSessionKey.UserId);
			},
			onError: ex => HandleError(ex),
			() => Log.Information("Set User Completed"));
	}

	// If the user opens multiple tabs or windows, and loads them all at once, it will spawn multiple Sets like gremlins in a pool. So wait for it!
	private void WaitForOthers(Guid userId)
	{
		// Suppose we have four calls, with id 1,2,3,4. The last one setting the localstorage is the winner.
		// I am going to wait and check if the id matches the last one set. I have no idea of the others existence sadly.
		// 500.000 ticks = 50 ms
		Log.Debug("%%%%% Waiting for others to finish");
		var getObservable = Observable.FromAsync(() => LocalStorageProvider.GetAsync<UserSessionKey>("helmerdemo-blazor-session")).Delay(new TimeSpan(500000));
		
		getObservable.Subscribe(content => MatchingId(content, userId),
			onError: ex => HandleError(ex),
			() => Log.Information("Second Get From Localstorage Completed"));
	}

	private void MatchingId(Result<UserSessionKey> content, Guid userId)
	{
		Log.Debug("%%%%% Matching the Ids");
		// If I am a winner (my Id == last set Id), I will initialize the UserActor
		if(content.IsSuccess && content.Value.UserId == userId)
		{
			WaitingForOnUserCreated(content.Value.UserId);
			return;
		}
		
		// if I am not the winner:
		if(content.IsSuccess && content.StatusCode.Equals(HttpStatusCode.OK))
		{
			// I am not the winner, I have to wait for the winner to initialize the UserActor (call Initialize).
			var delayedInitialize = Observable.Timer(TimeSpan.FromMilliseconds(50));
			delayedInitialize.Subscribe(_ => Initialize(userId));
			return;
		}
		var getObservable = Observable.FromAsync(() => LocalStorageProvider.GetAsync<UserSessionKey>("helmerdemo-blazor-session")).Delay(new TimeSpan(500000));
		
		getObservable.Subscribe(content => MatchingId(content, userId),
			onError: ex => HandleError(ex),
			() => Log.Information("Second Get From Localstorage Completed"));
		
		ShowErrorPage("Another error while looking for Id");
		
	}

	private void WaitingForOnUserCreated(Guid userId)
	{
		Log.Debug("%%%%% Waiting for on UserCreated");
		_userCreatedSubscription = RootContainer.MyRootActor.WhenNewMessageSent
			.Where(act => act
				.UserCreated() && act.Address == userId)
			.Subscribe(action =>
				{
					Initialize(userId);
				}, onError: ex => HandleError(ex),
				() => Log.Information("Created Completed"));
		RootContainer.MyRootActor.Post(new ActorAction("set", "user", userId));
	}

	

	private void HandleError(Exception ex)
	{
		Log.Error(ex, "%%%%% Error in UserSessionProvider");
		// 2: In case of a cryptographic-error => remove the stored data, show AskQuestion
		if (ex.GetType() == typeof(CryptographicException))
		{
			var deleteLocalStore = Observable.FromAsync(() => LocalStorageProvider.DeleteAsync("helmerdemo-blazor-session").AsTask());

			deleteLocalStore.Subscribe(
				_ => UpdateState(UserSessionState.Active),
				onError: exc => Log.Error(exc, "Error deleting the local storage"),
				() => Log.Information("Delete User Completed"));
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
		Log.Debug("%%%%% Disposing UserSessionProvider");
		_userFoundSubscription?.Dispose();
		_userNotFoundSubscription?.Dispose();
		_userCreatedSubscription?.Dispose();
	}
}
