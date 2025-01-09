using HelmerDemo.BlazorServer.Presentation.JsInterop.Contracts;
using HelmerDemo.BlazorServer.Shared.Tools;
using HelmerDemo.BlazorServer.Shared.Tools.Extensions;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Serilog;

namespace HelmerDemo.BlazorServer.Presentation.JsInterop;

public class LocalStorageProvider : ILocalStorageProvider
{
	private readonly ProtectedLocalStorage _localStorage;

	public LocalStorageProvider(ProtectedLocalStorage localStorage)
	{
		_localStorage = localStorage;
	}

	public async Task<bool> IsEnabled()
	{
		var key = "local-storage-enabled";
		var id = "unique-id";
		await _localStorage.SetAsync(key, id);
		var result = await _localStorage.GetAsync<string>(key);
		
		if(result.Success && result.Value == id)
		{
			await _localStorage.DeleteAsync(key);
			return true;
		}

		return false;
	}

	public async Task<Result<T>> GetAsync<T>(string key)
	{
		try
		{
			var protectedBrowserStorageResult = await _localStorage.GetAsync<T>(key);

			if (!protectedBrowserStorageResult.Success || protectedBrowserStorageResult.Value is null)
				return Result.NotFound.DownCast<T>();

			return Result.Ok.DownCast(protectedBrowserStorageResult.Value);
		}
		catch (Exception e)
		{
			Log.Error(e, "Error while getting value from local storage for {key}", key);
			return Result.InternalServerError.DownCast<T>();
		}
	}

	public async ValueTask SetAsync<T>(string key, T value) => await _localStorage.SetAsync(key, value);

	public async ValueTask DeleteAsync(string key) => await _localStorage.DeleteAsync(key);
}
