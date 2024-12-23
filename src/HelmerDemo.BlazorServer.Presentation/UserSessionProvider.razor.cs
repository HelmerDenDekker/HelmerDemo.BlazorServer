using System.Net;
using System.Reactive.Linq;
using System.Security.Cryptography;
using HelmerDemo.BlazorServer.Presentation.JsInterop.Contracts;
using HelmerDemo.BlazorServer.Presentation.ViewModel;
using HelmerDemo.BlazorServer.Shared.Tools;
using Microsoft.AspNetCore.Components;
using Serilog;

namespace HelmerDemo.BlazorServer.Presentation;

// Showing the n:1 solution for simple messages
public partial class UserSessionProvider : ComponentBase, IDisposable
{
	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	[Inject]
	public ILocalStorageProvider LocalStorageProvider { get; set; } = default!;

	[Inject]
	public NavigationManager MyNavigationManager { get; set; } = default!;

	private UserSessionViewModel UserSessionContent { get; set; } = new UserSessionViewModel();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			// TODO: If a localstorage is not available in the browser .... => what to do?

			var observable = Observable.FromAsync(() => LocalStorageProvider.GetAsync<UserSessionKey>("helmerdemo-blazor-session"));
			
			observable.Subscribe(content => InitializeAsync(content),
				onError: ex => HandleError(ex),
				() => Log.Information("Get Completed"));
		}

		await base.OnAfterRenderAsync(firstRender);
	}

	private void InitializeAsync(Result<UserSessionKey> userSession)
	{
		// Rules =>  Action

		// TODO: 1: No Id found => show child (AskQuestion)
		// 
		var notFoundRule = userSession.StatusCode.Equals(HttpStatusCode.NotFound);
		if (notFoundRule)
		{
			UpdateState(UserSessionState.Active);
			return;
		}

		//
		// TODO: 3: Id found => Call the UserSessionActor to validate Id
		if (userSession.IsSuccess)
		{
			// TODO: Call the UserSessionActor to validate Id
			UpdateState(UserSessionState.Active);
			return;
		}

		ShowErrorPage("Another error while looking for Id");
	}

	private void HandleError(Exception ex)
	{
		// TODO: 2: In case of a cryptographic-error => remove the stored data, show AskQuestion
		if (ex.GetType() == typeof(CryptographicException))
		{
			var deleteLocalStore = Observable.FromAsync(() => LocalStorageProvider.DeleteAsync("helmerdemo-blazor-session").AsTask());

			deleteLocalStore.Subscribe(
				_ => UpdateState(UserSessionState.Active),
				onError: ex => Log.Error(ex, "Error deleting the local storage"),
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
