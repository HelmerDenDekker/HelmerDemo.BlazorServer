using System.Collections.Concurrent;
using Serilog;

namespace HelmerDemo.BlazorServer.Application.Actors;

public class ActorChildren<T>  where T : class
{
	internal ConcurrentDictionary<Guid, T> Children { get; } = new();
	
	internal void Add(Guid id, T actor)
	{
		if (!Children.TryAdd(id, actor))
			Log.Error("Adding key {id} to dictionary {name} failed: key already exists", id, nameof(T));
	}

	internal void RemoveById(Guid id)
	{
		if (!Children.TryRemove(id, out _))
			Log.Error("Removal of key {id} in dictionary {name} failed: key not found", id, nameof(T));
	}

	internal T? FindById(Guid id)
	{
		var isRetrieved = Children.TryGetValue(id, out var model);
		return isRetrieved ? model : null;
	}
}
