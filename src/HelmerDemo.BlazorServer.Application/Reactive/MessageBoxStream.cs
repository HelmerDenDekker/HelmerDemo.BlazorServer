using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Interfaces;

namespace HelmerDemo.BlazorServer.Application.Reactive;

/// <summary>
/// This is a singleton store as a stream.
/// </summary>
public class MessageBoxStream : IMessageBoxStream, IDisposable
{
	/// <summary>
	///  The BehaviorSubject begins by emitting the item most recently emitted: https://reactivex.io/documentation/subject.html
	/// </summary>
	private readonly BehaviorSubject<Message> _messageChangedSubject = new(Message.Create("Loading..."));

	// TODO: MessageUserBox, later
	public IObservable<Message> WhenMessageChanged => _messageChangedSubject;
	
	public void OnMessageChanged(Message message)
	{
		message.ChangeContent(message.Content);
		_messageChangedSubject.OnNext(message);
	}

	public void Dispose()
	{
		_messageChangedSubject.Dispose();
	}
}
