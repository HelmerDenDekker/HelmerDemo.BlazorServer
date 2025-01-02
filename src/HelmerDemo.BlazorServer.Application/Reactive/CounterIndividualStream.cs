using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public class CounterIndividualStream : IDisposable
{
	private readonly int _maxCount;
	private readonly IDisposable? _subscription;

	private readonly Subject<Unit> _secondPassedSubject = new();

	public IObservable<Unit> WhenSecondPassed => _secondPassedSubject;

	public CounterIndividualStream(int max)
	{
		_maxCount = max;
		// Source
		IObservable<long> ticks = Observable.Timer(
			dueTime: TimeSpan.Zero,
			period: TimeSpan.FromSeconds(1));
		// Subscribe
		_subscription = ticks.Subscribe(_ => { _secondPassedSubject.OnNext(Unit.Default); },
			exception => _secondPassedSubject.OnError(exception),
			() => _secondPassedSubject.OnCompleted()
		);
	}

	public void Dispose()
	{
		_subscription?.Dispose();
		_secondPassedSubject.Dispose();
	}
}
