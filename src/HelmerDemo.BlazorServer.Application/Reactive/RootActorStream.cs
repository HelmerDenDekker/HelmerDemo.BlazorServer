using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Actors;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public class RootActorStream: IRootActorStream
{
	private readonly Subject<ActorAction> _actionSubject = new();
	
	public IObservable<ActorAction> WhenActionUpdated => _actionSubject;
	
	public void OnMessageReceived(ActorAction message)
	{
		// put it on the stream
		_actionSubject.OnNext(message);
	}

	public void Dispose()
	{
		_actionSubject.Dispose();
	}
}

public interface IRootActorStream : IDisposable
{
	IObservable<ActorAction> WhenActionUpdated { get; }
	void OnMessageReceived(ActorAction message);
}