using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Interfaces;
using HelmerDemo.BlazorServer.Presentation.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Serilog;

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
		// TODO: Only take values when the message is not empty && not the same as the current message
		_subscription = MessageBoxStream.WhenMessageChanged.Subscribe(message =>
		{
			MessageInput.Content = message.Content;
			StateHasChanged();
		});
		
		_enterSubscription = _keyUpStream.Where(key => key == "Enter" || key == "NumpadEnter").Subscribe(_ => Submit(_editContext));

		// TODO: InputText changing => update the MessageBoxStream
		
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
