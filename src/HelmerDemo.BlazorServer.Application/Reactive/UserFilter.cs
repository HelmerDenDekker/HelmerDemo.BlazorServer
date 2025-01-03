using HelmerDemo.BlazorServer.Application.Actors;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public static class UserActionFilter
{
	public static bool IsSetUser(this ActorAction action)
	{
		return action.Name.Equals("user", StringComparison.OrdinalIgnoreCase) && action.Action.Equals("set", StringComparison.OrdinalIgnoreCase);

	}
}
