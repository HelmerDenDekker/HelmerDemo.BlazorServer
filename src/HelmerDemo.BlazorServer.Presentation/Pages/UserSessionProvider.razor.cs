using System.Security.Cryptography;
using HelmerDemo.BlazorServer.Presentation.JsInterop.Contracts;
using HelmerDemo.BlazorServer.Presentation.Logic;
using HelmerDemo.BlazorServer.Presentation.ViewModel;
using HelmerDemo.BlazorServer.Shared.Tools;
using Microsoft.AspNetCore.Components;
using Serilog;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

// Showing the n:1 solution for simple messages
public partial class UserSessionProvider : ComponentBase, IDisposable
{
	[Parameter]
	public RenderFragment? Content { get; set; }
	
	[Inject]
	private ILocalStorageProvider LocalStorageProvider { get; set; } = default!;
	
	[Inject]
	public NavigationManager MyNavigationManager { get; set; } = default!;
	
	private UserSessionViewModel UserSessionContent { get; set; } = new UserSessionViewModel();

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			// TODO: If a localstorage is not available in the browser .... => what to do?
			
			try
			{
				// TODO: Use a stream instead!!! There is no use blocking shit for some event to take place.
				var userSessionResult = await LocalStorageProvider.GetAsync<UserSessionKey>(CookieLogic.CookieKey("test"));
				
				//TODO: Instead of InitializeAsync, create rules and subscribe to these.
				InitializeAsync(userSessionResult);
			}
			catch (TaskCanceledException cancelledException)
			{
				ShowErrorPage("The task was cancelled");
			}
			catch (Exception e)
			{
				ShowErrorPage("A generic error occured");
			}
			
		}

		await base.OnAfterRenderAsync(firstRender);
	}

	private async Task InitializeAsync(Result<UserSessionKey> userSession)
	{
		// Rules =>  Action
		// TODO: 1: No Id found => show AskQuestion
		// 
		// TODO: 2: In case of a cryptographic-error => remove the stored data, show AskQuestion
		//
		// TODO: 3: Id found => Call the UserSessionActor to validate Id
		
		
	}
	
	private void ShowErrorPage(string errorMessage)
	{
		UserSessionContent.State = UserSessionState.Error;
		UserSessionContent.SystemMessage = errorMessage;
		InvokeAsync(StateHasChanged);
	}
	
	

	public async ValueTask DisposeAsync()
	{
		// ???
	}
}
