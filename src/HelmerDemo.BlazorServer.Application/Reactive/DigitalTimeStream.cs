using System.Reactive.Linq;
using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Domain.Clock;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public class DigitalTimeStream : IDigitalTimeStream
{
	private readonly Subject<DigitalTime> _digitalTimeSubject = new();
	private readonly IDisposable _subscription;
	private DigitalTime _currentTime = new(DateTime.Now);
	
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
	
	public IObservable<DigitalTime> WhenDigitalTimeChanged => _digitalTimeSubject;
	
	public void Dispose()
	{
		_subscription.Dispose();
		_digitalTimeSubject.Dispose();
	}
}