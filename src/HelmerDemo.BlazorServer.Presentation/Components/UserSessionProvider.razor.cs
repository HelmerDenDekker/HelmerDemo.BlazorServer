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
			
			Initialize(userSession.Value.UserId);
			
			return;
		}

		ShowErrorPage("Another error while looking for Id");
	}

	private void Initialize(Guid userId)
	{
		Log.Debug("%%%%% Initializing UserActor");
		MyUserActor = RootContainer.MyRootActor.GetOrCreate(userId);
		// I am doing this cascading shizzle like this. I am not sure if this is the right way to do it.
		MyUserActor?.WhenNewMessageSent.Where(act=>act.CreatedCounter() && act.Address == userId).Subscribe(_ =>
		{
			// Rule: if in store => Update UserState to ChildContent
			MyUserActor = RootContainer.MyRootActor.GetOrCreate(userId);
			UpdateState(UserSessionState.Active);
		});

		UpdateState(UserSessionState.Active);
		Log.Debug("%%%%% Done Initializing UserActor");
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
				CheckUserId(userSessionKey.UserId);
			},
			onError: ex => HandleError(ex),
			() => Log.Information("Set User Completed"));
	}

	// If the user opens multiple tabs or windows, and loads them all at once, it will spawn multiple Sets like gremlins in a pool. So wait for it!
	private void CheckUserId(Guid userId)
	{
		// Suppose we have four calls, with id 1,2,3,4. The last one setting the localstorage is the winner.
		// I am going to wait and check if the id matches the last one set. I have no idea of the others existence sadly.
		// 10.000 ticks = 1 ms
		Log.Debug("%%%%% Checking for UserId {userId}", userId);
		var getObservable = Observable.FromAsync(() => LocalStorageProvider.GetAsync<UserSessionKey>("helmerdemo-blazor-session")).Delay(new TimeSpan(100000));
		
		getObservable.Subscribe(content => MatchingId(content, userId),
			onError: ex => HandleError(ex),
			() => Log.Information("Second Get From Localstorage Completed"));
	}

	private void MatchingId(Result<UserSessionKey> content, Guid userId)
	{
		Log.Debug("%%%%% Matching the Ids{userId}", userId);
		// If I am a winner (my Id == last set Id), I will initialize the UserActor
		if(content.IsSuccess && content.Value.UserId == userId)
		{
			Log.Debug("%%%%% UserId {userId} matches", userId, content.Value.UserId);
			Initialize(content.Value.UserId);
			return;
		}
		
		// if I am not the winner: Check if new is the last one set
		if(content.IsSuccess)
		{
			Log.Debug("%%%%% UserId {userId} does not match {contentUserId}", userId, content.Value.UserId);
			CheckUserId(content.Value.UserId);
			return;
		}
		
		Log.Debug("%%%%% Matching the Ids Derailed!");
		var getObservable = Observable.FromAsync(() => LocalStorageProvider.GetAsync<UserSessionKey>("helmerdemo-blazor-session")).Delay(new TimeSpan(500000));
		
		getObservable.Subscribe(content => MatchingId(content, userId),
			onError: ex => HandleError(ex),
			() => Log.Information("Second Get From Localstorage Completed"));
		
		ShowErrorPage("Another error while looking for Id");
		
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
