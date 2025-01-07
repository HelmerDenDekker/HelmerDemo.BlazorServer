using HelmerDemo.BlazorServer.Application.Actors;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public static class UserActionFilter
{
	public static bool IsSetUser(this ActorAction action)
	{
		return action.ActorName.Equals("user", StringComparison.OrdinalIgnoreCase) && action.Action.Equals("set", StringComparison.OrdinalIgnoreCase);
	}

	public static bool IsGetUser(this ActorAction action)
	{
		return action.ActorName.Equals("user", StringComparison.OrdinalIgnoreCase) && action.Action.Equals("get", StringComparison.OrdinalIgnoreCase);
	}
	
	public static bool IsFoundUser(this ActorAction action)
	{
		return action.ActorName.Equals("user", StringComparison.OrdinalIgnoreCase) && action.Action.Equals("found", StringComparison.OrdinalIgnoreCase);
	}
	
	public static bool UserNotFound(this ActorAction action)
	{
		return action.ActorName.Equals("user", StringComparison.OrdinalIgnoreCase) && action.Action.Equals("not-found", StringComparison.OrdinalIgnoreCase);
	}
	
	public static bool UserCreated(this ActorAction action)
	{
		return action.ActorName.Equals("user", StringComparison.OrdinalIgnoreCase) && action.Action.Equals("created", StringComparison.OrdinalIgnoreCase);
	}
}
