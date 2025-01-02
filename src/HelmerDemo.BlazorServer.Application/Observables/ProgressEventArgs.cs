namespace HelmerDemo.BlazorServer.Application.Observables;

public class ProgressEventArgs : EventArgs
{
	public ProgressEventArgs(int progress)
	{
		Progress = progress;
	}

	public int Progress { get; private set; }
}

