using System.Collections.Concurrent;

namespace HelmerDemo.BlazorServer.Application.Actors;

public interface IActorChildren<T>
{
	/// <summary>
	/// An actor has 0:n (internal) children 
	/// </summary>
	internal ConcurrentDictionary<Guid, T> Children { get; }

	/// <summary>
	/// Register an actors child
	/// </summary>
	/// <param name="id"></param>
	/// <param name="actor"></param>
	internal void Add(Guid id, T actor);

	internal void RemoveById(Guid id);

	/// <summary>
	///     Tries to find the actor by Id, returns null if not found
	/// </summary>
	/// <param name="id"></param>
	/// <returns>Model of T or null</returns>
	internal T? FindById(Guid id);
}
