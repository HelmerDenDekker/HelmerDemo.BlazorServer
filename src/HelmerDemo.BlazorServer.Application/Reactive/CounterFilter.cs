using HelmerDemo.BlazorServer.Application.Actors;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public static class CounterFilter
{
	public static bool IsSetCounter(this ActorAction action)
	{
		return action.ActorName.Equals("counter", StringComparison.OrdinalIgnoreCase) && action.Action.Equals("set", StringComparison.OrdinalIgnoreCase);
	}
	
	public static bool CreatedCounter(this ActorAction action)
	{
		return action.ActorName.Equals("counter", StringComparison.OrdinalIgnoreCase) && action.Action.Equals("created", StringComparison.OrdinalIgnoreCase);
	}
	
	public static bool DeleteCounter(this ActorAction action)
	{
		return action.ActorName.Equals("counter", StringComparison.OrdinalIgnoreCase) && action.Action.Equals("delete", StringComparison.OrdinalIgnoreCase);
	}
}
