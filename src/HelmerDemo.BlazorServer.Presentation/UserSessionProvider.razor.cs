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

namespace HelmerDemo.BlazorServer.Presentation;

// A user session provider for shared variables between pages, and between page navigations.
public partial class UserSessionProvider : ComponentBase, IDisposable
{
	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	[Inject]
	public ILocalStorageProvider LocalStorageProvider { get; set; } = default!;

	[Inject]
	public NavigationManager MyNavigationManager { get; set; } = default!;
	
	// TODO: Inject the RootActor
	[Inject]
	public RootActor MyRootActor { get; set; } = default!;
	
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

		// 1: No Id found => show child (AskQuestion) and set UserId
		var notFoundRule = userSession.StatusCode.Equals(HttpStatusCode.NotFound);
		if (notFoundRule)
		{
			UpdateState(UserSessionState.Active);
			return;
		}

		// 3: Id found => Call the UserSessionActor to validate Id
		if (userSession.IsSuccess)
		{
			
			var userActor = MyRootActor.FindById(userSession.Value.UserId);
			
			// I can have the logic in here, because this class is only triggered once per user, and only after first render.
            // Rule: if not in store => Delete from LocalStorage, and set new.
			
			
			// Rule: if in store => Update UserState to ChildContent
			UpdateState(UserSessionState.Active);
			return;
		}
		
		
		
		

		ShowErrorPage("Another error while looking for Id");
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
		// ???
	}
}
