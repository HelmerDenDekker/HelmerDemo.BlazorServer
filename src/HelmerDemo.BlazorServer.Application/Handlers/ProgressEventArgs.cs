namespace HelmerDemo.BlazorServer.Application.Handlers;

public class ProgressEventArgs : EventArgs
{
	public ProgressEventArgs(int progress)
	{
		Progress = progress;
	}

	public int Progress { get; private set; }
}

