using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Handlers;

public class CounterSubject
{
	private int _maxCount;
	
	public IObservable<CountProgress> Start(int max)
	{
		_maxCount = max;
		var subject = new Subject<CountProgress>();
		Task.Run(() => CountDraculaCounts(subject));
		return subject;
	}

	private void CountDraculaCounts(Subject<CountProgress> subject)
	{
		try
		{
			foreach (var n in Enumerable.Range(1, _maxCount))
			{
				Thread.Sleep(1000);
				subject.OnNext(new CountProgress(n));
			}
			subject.OnCompleted();
		}
		catch (Exception exception)
		{
			subject.OnError(exception);
		}
		finally
		{
			subject.Dispose();
		}
	}
	
}
