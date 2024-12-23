using System.Collections.Concurrent;

namespace HelmerDemo.BlazorServer.Application.Persistence;

public interface IMemoryStore<T>
{
	public ConcurrentDictionary<Guid, T> Entities { get; }

	public void Add(Guid id, T model);

	public void RemoveById(Guid id);

	/// <summary>
	///     Tries to find the KeyAndValue by Id, returns null if not found
	/// </summary>
	/// <param name="id"></param>
	/// <returns>Model of T or null</returns>
	public T? FindById(Guid id);
}
