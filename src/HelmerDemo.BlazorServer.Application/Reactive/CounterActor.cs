using System.Reactive.Linq;
using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public class CounterActor : IDisposable
{
	private readonly int _maxCount;
	private readonly IDisposable? _subscription;
	private readonly CountProgress _countProgress = new(0);

	private readonly BehaviorSubject<CountProgress> _counterChangedSubject = new(new CountProgress(0));

	public IObservable<CountProgress> WhenCounterChanged => _counterChangedSubject;

	public CounterActor(int max)
	{
		_maxCount = max;
		// Source
		IObservable<long> ticks = Observable.Timer(
			dueTime: TimeSpan.Zero,
			period: TimeSpan.FromSeconds(1));
		// Subscribe
		_subscription = ticks.Subscribe(_ =>
			{
				// Rules
				if (MaxValueReached())
					return;

				// increment the counter
				OnCounterIncremented();
			},
			exception => _counterChangedSubject.OnError(exception),
			() => _counterChangedSubject.OnCompleted()
		);
	}

	private bool MaxValueReached()
	{
		if (_countProgress.Value < _maxCount)
			return false;

		_counterChangedSubject.OnCompleted();
		_subscription?.Dispose();
		return true;
	}

	public void OnCounterIncremented()
	{
		if (MaxValueReached())
			return;

		_countProgress.Increment();
		_counterChangedSubject.OnNext(_countProgress);
	}

	public void Dispose()
	{
		_subscription?.Dispose();
		_counterChangedSubject.Dispose();
	}
}
