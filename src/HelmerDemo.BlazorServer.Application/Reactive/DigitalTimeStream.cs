using System.Reactive.Linq;
using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public class DigitalTimeStream : IDisposable
{
	public DigitalTimeStream()
	{
		// Source
		IObservable<long> ticks = Observable.Timer(
			dueTime: TimeSpan.Zero,
			period: TimeSpan.FromSeconds(1));
		// Subscribe
		_subscription = ticks.Subscribe(_ =>
			{
				_currentTime = _currentTime.AddSecond();
				_digitalTimeSubject.OnNext(_currentTime);
			}
		);
	}
	
	private readonly Subject<DigitalTime> _digitalTimeSubject = new();
	private readonly IDisposable _subscription;
	public IObservable<DigitalTime> WhenDigitalTimeChanged => _digitalTimeSubject;

	private DigitalTime _currentTime = new(DateTime.Now);
	
	public void Dispose()
	{
		_subscription.Dispose();
		_digitalTimeSubject.Dispose();
	}
}
