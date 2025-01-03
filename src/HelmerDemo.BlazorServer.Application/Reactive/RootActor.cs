using System.Collections.Concurrent;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Actors;

namespace HelmerDemo.BlazorServer.Application.Reactive;


// The Root actor is ever lasting (lives as long as the application lives)
public class RootActor : ActorChildren<IActor>, IActor
{
	private readonly IRootActorStream _rootActorStream;

	public RootActor(IRootActorStream rootActorStream)
	{
		_rootActorStream = rootActorStream;
		Process(); // self start processing. It is a stream. 
	}
	
	private readonly Subject<ActorAction> _newActionSubject = new();
	
	public Guid Address { get; } = Guid.NewGuid();
	
	public string Name { get; } = "root";

	
	public IObservable<ActorAction> WhenNewMessageSent => _newActionSubject;
	
	

	public void Post(ActorAction action)
	{
		_rootActorStream.OnMessageReceived(action);
	}
	
	/// <summary>
	/// The Actor should process the incoming messages.
	/// </summary>
	public void Process()
	{
		_rootActorStream.WhenActionUpdated.Where(act=>act.IsSetUser()).Subscribe(action =>
		CreateNewUserActor(action));
	}
	

	// LOGIC
	
	// private rules here... TODO: somewhere I can test them

	private void CreateNewUserActor(ActorAction action)
	{
		// TODO Add new user actor
	}

	// TODO: Oncompleted and OnError. Self-healing business. these are rules as well.
}
