using System.Collections.Concurrent;
using System.Reactive.Subjects;
using HelmerDemo.BlazorServer.Application.Actors;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public class UserActor : ActorChildren<IActor>, IActor
{
	private readonly Subject<ActorAction> _newActionSubject = new();
	
	
	
	public UserActor(Guid address)
	{
		Address = address;
		Name = "user";
	}
	
	public Guid Address { get; }
	public string Name { get; }
	
	public IObservable<ActorAction> WhenNewMessageSent => _newActionSubject;
	public void Post(ActorAction action)
	{
		if (action.IsSetCounter())
		{
			int maxValue = Convert.ToInt32(action.Value);
			Add(Constants.CounterActorGuid, new CounterActor(maxValue));
			_newActionSubject.OnNext(new ActorAction("created", "counter", action.Address));
		}

		if (action.DeleteCounter())
		{
			RemoveById(Constants.CounterActorGuid);
		}
	}
}
