namespace HelmerDemo.BlazorServer.Application.Handlers;

/// <summary>
/// Finished or Error
/// </summary>
public class ErrorEventArgs : EventArgs
{
	public ErrorEventArgs(Exception exception = null)
	{
		Exception = exception;
	}

	public Exception Exception { get; private set; }
}
