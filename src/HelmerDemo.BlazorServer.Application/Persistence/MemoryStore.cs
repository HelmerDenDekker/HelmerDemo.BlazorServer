using System.Collections.Concurrent;
using Serilog;

namespace HelmerDemo.BlazorServer.Application.Persistence;

public class MemoryStore<T> : IMemoryStore<T>  where T : class
{
	public ConcurrentDictionary<Guid, T> Entities { get; } = new();
	
	public void Add(Guid id, T model)
	{
		if (!Entities.TryAdd(id, model))
			Log.Error("{name}Store, Add: key {id} already exists", nameof(T), id);
	}

	public void RemoveById(Guid id)
	{
		if (!Entities.TryRemove(id, out _))
			Log.Error("{name}Store, RemoveById: key {id} not found", nameof(T), id);
	}

	public T? FindById(Guid id)
	{
		var isRetrieved = Entities.TryGetValue(id, out var model);
		return isRetrieved ? model : null;
	}
}
