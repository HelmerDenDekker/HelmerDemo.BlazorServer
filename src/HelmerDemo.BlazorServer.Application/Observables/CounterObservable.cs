namespace HelmerDemo.BlazorServer.Application.Observables;

public class CounterObservable
{
	private int _maxCount;

	public event EventHandler<ProgressEventArgs> Progressing;
	public event Action Finished = delegate { };
	public event EventHandler<ErrorEventArgs> Errored;

	/// <summary>
	///     Helper to prevent race condition
	/// </summary>
	/// <param name="e"></param>
	protected virtual void OnError(ErrorEventArgs e)
	{
		Errored(this, e);
	}

	protected virtual void OnFinished()
	{
		Finished();
	}

	/// <summary>
	///     Helper to prevent race condition
	/// </summary>
	/// <param name="e"></param>
	protected virtual void OnProgress(ProgressEventArgs e)
	{
		Progressing(this, e);
	}

	/// <summary>
	///     Starts the action
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
