using HelmerDemo.BlazorServer.Shared.Tools;

namespace HelmerDemo.BlazorServer.Presentation.JsInterop.Contracts;

public interface ILocalStorageProvider
{
	public Task<bool> IsEnabled();
	
	public Task<Result<T>> GetAsync<T>(string key);

	public ValueTask SetAsync<T>(string key, T value);

	public ValueTask DeleteAsync(string key);
}
