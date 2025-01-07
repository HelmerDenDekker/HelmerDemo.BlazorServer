using System.Reactive.Linq;
using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Actors;
using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public class CounterActor : ActorChildren<IActor>, IActor
{
	private readonly int _maxCount;
	private readonly IDisposable? _subscription;
	private readonly CounterRxo _counterRxo = new(0);

	private readonly BehaviorSubject<CounterRxo> _counterChangedSubject = new(new CounterRxo(0));

	public IObservable<CounterRxo> WhenCounterChanged => _counterChangedSubject;

	public CounterActor(int max)
	{
		_maxCount = max;
		Address = Constants.CounterActorGuid;
		Name = "counter";
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
		if (_counterRxo.Value < _maxCount)
			return false;

		_counterRxo.ErrorMessage = "Count Dracula stopped counting!";
		_counterChangedSubject.OnNext(_counterRxo);
		_counterChangedSubject.OnCompleted();
		_subscription?.Dispose();
		return true;
	}

	private void OnCounterIncremented()
	{
		if (MaxValueReached())
			return;

		_counterRxo.Increment();
		_counterChangedSubject.OnNext(_counterRxo);
	}

	public void Dispose()
	{
		_counterRxo.ErrorMessage = "Disposed";
		_subscription?.Dispose();
		_counterChangedSubject.Dispose();
	}

	public Guid Address { get; }
	public string Name { get; }
	
	// TODO NB Not implemented!
	public IObservable<ActorAction> WhenNewMessageSent { get; }
	public void Post(ActorAction action)
	{
		if(action.Action == "increment")
			OnCounterIncremented();
	}
}
