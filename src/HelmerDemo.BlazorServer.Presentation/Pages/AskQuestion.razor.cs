using System.Reactive.Linq;
using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Presentation.ViewModel;
using HelmerDemo.BlazorServer.Shared.Tools.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

// Showing the n:1 solution for simple messages
public partial class AskQuestion : ComponentBase, IDisposable
{
	[CascadingParameter]
	public UserSessionProvider SessionContainer { get; set; }
	
	protected EditContext _editContext;
	
	protected TextAreaViewModel MessageInput { get; set; } = new() { Content = string.Empty };
	
	private IObservable<string> _whenInputChanged;
	private readonly Subject<string> _inputStream = new();
	private InputTextArea _inputTextReference;
	private IObservable<string> _whenKeyUp;
	private readonly Subject<string> _keyUpStream = new();
	private IDisposable? _subscription;
	private IDisposable? _enterSubscription;
	
	private string CookieKey => $"askquestion-with-someid";

	protected override async Task OnInitializedAsync()
	{
		_editContext = new EditContext(MessageInput);
		
		// Subscribe to the MessageBoxStream and load the latest message
		await SetFromStorageAsync();
		
		_enterSubscription = _keyUpStream.Where(key => key == "Enter" || key == "NumpadEnter").Subscribe(_ => Submit(_editContext));
		
		// TODO: Extra (bonus) Keep this in sync with other AskQuestion tabs.
		_inputStream.Where(m => !m.IsNullOrWhiteSpace()).Throttle(TimeSpan.FromMilliseconds(100)).Subscribe(_ => InvokeAsync(()=>StoreMessageAsync()) );
	}

	private async Task SetFromStorageAsync()
	{
		var result = await SessionContainer.LocalStorageProvider.GetAsync<string>(CookieKey);
		if (result.IsSuccess)
		{
			MessageInput.Content = result.Value;
			StateHasChanged();
		}
	}

	private void LogEvents(string e)
	{
		Console.WriteLine("Field changed: " + e);
	}
	private void Submit(EditContext obj)
	{
		Console.WriteLine($"Form submitted: {MessageInput.Content}" );
		MessageInput.Content = string.Empty;
		// TODO: Send the message to the server and Delete the cookiekey entry from LocalStorage
		StateHasChanged();
	}
	
	private void MessageTextInputEventHandler(ChangeEventArgs obj)
	{
		_inputStream.OnNext(MessageInput.Content);
	}
	
	private void MessageTextKeyUpEventHandler(KeyboardEventArgs obj)
	{
		_keyUpStream.OnNext(obj.Code);
	}

	public void Dispose()
	{
		_inputStream.Where(m => !m.IsNullOrWhiteSpace()).Subscribe(_ => InvokeAsync(()=> StoreMessageAsync()));
		((IDisposable)_inputTextReference).Dispose();
		_subscription?.Dispose();
		_enterSubscription?.Dispose();
	}

	public async Task StoreMessageAsync()
	{
		await SessionContainer.LocalStorageProvider.SetAsync<string>(CookieKey, MessageInput.Content);
	}
}
