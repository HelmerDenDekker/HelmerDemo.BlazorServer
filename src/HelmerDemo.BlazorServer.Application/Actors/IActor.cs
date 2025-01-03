namespace HelmerDemo.BlazorServer.Application.Actors;

/// <summary>
/// https://en.wikipedia.org/wiki/Actor_model
/// </summary>
public interface IActor : IActorChildren<IActor>
{
	/// <summary>
	/// An actor has a unique identifier or Address (like a mailbox)
	/// </summary>
	public Guid Address { get; }
	
	public string Name { get; }
	
	// Send a message from the actor with the IObservable<T> (ability to subscribe to messages) => I want to be able to send messages to an other actor. This is a decoupled way of doing logic.
	public IObservable<ActorAction> WhenNewMessageSent { get; }
	
	// Receive a message from an actor by subscribing to the IObservable<T>
	// Process a message
	
	// TODO: How to send a message to an actor? 
	
	
	// Restart the actor
	
	// Stop the actor
	
	// Dispose the actor
	
	// Actor State
	
	// Monitor the (child) actor
	
	// This is a lot!! What about single responsibility? => An actor is split up in multiple parts in Akka.NET.
}
