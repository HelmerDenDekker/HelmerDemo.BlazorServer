namespace HelmerDemo.BlazorServer.Application.Handlers;

public class CounterHandler
{
	private int _maxCount;
	
	public event EventHandler<ProgressEventArgs> Progress;
	public event Action Finished = delegate { };
	public event EventHandler<ErrorEventArgs> Error;

	/// <summary>
	/// Helper to prevent race condition
	/// </summary>
	/// <param name="e"></param>
	protected virtual void OnError(ErrorEventArgs e)
	{
		if (Error != null) Error(this, e);
	}

	protected virtual void OnFinished()
	{
		if(Finished!=null) Finished();
	}

	/// <summary>
	/// Helper to prevent race condition
	/// </summary>
	/// <param name="e"></param>
	protected virtual void OnProgress(ProgressEventArgs e)
	{
		if (Progress != null) Progress(this, e);
	}

	/// <summary>
	/// Starts the action
	/// </summary>
	/// <param name="max">The maximum count</param>
	public void Start(int max)
	{
		_maxCount = max;
		Task.Run((Action)CountDraculaCounts);
	}

	private void CountDraculaCounts()
	{
		try
		{
			foreach (var n in Enumerable.Range(1, _maxCount))
			{
				Thread.Sleep(1000);
				//Raise event
				OnProgress(new ProgressEventArgs(n));
			}
			//Raise finished
			OnFinished();
		}
		catch (Exception exception)
		{
			// Raise Error
			OnError(new ErrorEventArgs(exception));
		}
	}
}
