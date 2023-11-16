using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Handlers;

public class ClockSubject
{
	private bool _stopTheClock = false;
	
	public IObservable<DigitalTime> Start()
	{
		var subject = new Subject<DigitalTime>();
		Task.Run(() => RunClock(subject));
		return subject;
	}

	public void Stop()
	{
		_stopTheClock = true;
	}

	private void RunClock(Subject<DigitalTime> subject)
	{
		try
		{
			while(!_stopTheClock)
			{
				Thread.Sleep(1000);
				subject.OnNext(new DigitalTime(DateTime.Now));
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
