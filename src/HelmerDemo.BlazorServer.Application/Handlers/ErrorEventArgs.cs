namespace HelmerDemo.BlazorServer.Application.Handlers;

/// <summary>
/// Finished or Error
/// </summary>
public class ErrorEventArgs : EventArgs
{
	public ErrorEventArgs(Exception exception)
	{
		Error = exception;
	}

	public Exception Error { get; private set; }
}
