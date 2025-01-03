using System.Collections.Concurrent;

namespace HelmerDemo.BlazorServer.Application.Actors;

public interface IActorChildren<T>
{
	/// <summary>
	/// An actor has 0:n (internal) children 
	/// </summary>
	public ConcurrentDictionary<Guid, T> Children { get; }

	/// <summary>
	/// Register an actors child
	/// </summary>
	/// <param name="id"></param>
	/// <param name="actor"></param>
	public void Add(Guid id, T actor);

	public void RemoveById(Guid id);

	/// <summary>
	///     Tries to find the actor by Id, returns null if not found
	/// </summary>
	/// <param name="id"></param>
	/// <returns>Model of T or null</returns>
	public T? FindById(Guid id);
}
