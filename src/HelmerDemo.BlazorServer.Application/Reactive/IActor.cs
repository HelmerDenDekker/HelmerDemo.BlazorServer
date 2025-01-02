namespace HelmerDemo.BlazorServer.Application.Reactive;

/// <summary>
/// https://en.wikipedia.org/wiki/Actor_model
/// </summary>
public interface IActor
{
	/// <summary>
	/// An actor has a unique identifier or Address (like a mailbox)
	/// </summary>
	public Guid Address { get; }
	
	/// <summary>
	/// An actor has 0:n (internal) children => I need direct control over them
	/// </summary>
	List<IActor> Children { get; }
	
	// Create a child
	public void Register(IActor child);
	
	// Send a message with the IObservable<T>, or ability to subscribe to messages
	public IObservable<object> WhenActorStateChanged { get; }
	
	// Receive a message by subscribing to the IObservable<T>
	// Process a message
	
	
	
	// Restart the actor
	
	// Stop the actor
	
	// Dispose the actor
	
	// Actor State
}
