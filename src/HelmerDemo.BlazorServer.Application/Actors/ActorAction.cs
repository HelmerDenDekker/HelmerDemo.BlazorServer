namespace HelmerDemo.BlazorServer.Application.Actors;

public class ActorAction
{
	public ActorAction(Guid address, string name, string action)
	{
		Address = address;
		ActorName = name;
		Action = action;
	}
	
	public Guid Address { get; set; }
	
	// TODO - this should be an enum preferably
	public string ActorName { get; set; }
	
	// TODO - this should be an enum preferably
	public string Action { get; set; }
}
