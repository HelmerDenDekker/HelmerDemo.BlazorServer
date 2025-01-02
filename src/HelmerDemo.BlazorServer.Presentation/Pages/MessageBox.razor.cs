using System.Reactive.Linq;
using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Interfaces;
using HelmerDemo.BlazorServer.Presentation.ViewModel;
using HelmerDemo.BlazorServer.Shared.Tools.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

// Showing the n:1 solution for simple messages
public partial class MessageBox : ComponentBase, IDisposable
{
	[Inject]
	private IMessageBoxStream MessageBoxStream { get; set; } = default!;
	
	protected EditContext _editContext;
	
	protected TextAreaViewModel MessageInput { get; set; } = new() { Content = string.Empty };
	
	private IObservable<string> _whenInputChanged;
	private readonly Subject<string> _inputStream = new();
	private InputTextArea _inputTextReference;
	private IObservable<string> _whenKeyUp;
	private readonly Subject<string> _keyUpStream = new();
	private IDisposable? _subscription;
	private IDisposable? _enterSubscription;

	protected override void OnInitialized()
	{
		_editContext = new EditContext(MessageInput);
		
		// Subscribe to the MessageBoxStream and load the latest message
		_subscription = MessageBoxStream.WhenMessageChanged.Where(m=>!m.Content.IsNullOrWhiteSpace() && !m.Equals(MessageInput.Content)).Subscribe(message =>
		{
			MessageInput.Content = message.Content;
			InvokeAsync(StateHasChanged);
		});
		
		_enterSubscription = _keyUpStream.Where(key => key == "Enter" || key == "NumpadEnter").Subscribe(_ => Submit(_editContext));

		// TODO: This is very very chatty, consider throttling (or debouncing?) the stream. also, it should update the Message content, not creating a new
		_inputStream.Where(m=>!m.IsNullOrWhiteSpace()).Subscribe(_ => MessageBoxStream.OnMessageChanged(Message.Create(MessageInput.Content)));
		
		base.OnInitialized();
	}

	private void LogEvents(string e)
	{
		Console.WriteLine("Field changed: " + e);
	}
	private void Submit(EditContext obj)
	{
		Console.WriteLine($"Form submitted: {MessageInput.Content}" );
		MessageInput.Content = string.Empty;
		MessageBoxStream.OnMessageChanged(Message.Create(""));
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
		((IDisposable)_inputTextReference).Dispose();
		_subscription?.Dispose();
		_enterSubscription?.Dispose();
	}
}
