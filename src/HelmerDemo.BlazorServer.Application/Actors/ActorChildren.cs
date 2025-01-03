using System.Collections.Concurrent;
using Serilog;

namespace HelmerDemo.BlazorServer.Application.Actors;

public class ActorChildren<T> : IActorChildren<T>  where T : class
{
	public ConcurrentDictionary<Guid, T> Children { get; } = new();
	
	public void Add(Guid id, T actor)
	{
		if (!Children.TryAdd(id, actor))
			Log.Error("{name}Store, Add: key {id} already exists", nameof(T), id);
	}

	public void RemoveById(Guid id)
	{
		if (!Children.TryRemove(id, out _))
			Log.Error("{name}Store, RemoveById: key {id} not found", nameof(T), id);
	}

	public T? FindById(Guid id)
	{
		var isRetrieved = Children.TryGetValue(id, out var model);
		return isRetrieved ? model : null;
	}
}
