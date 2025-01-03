namespace HelmerDemo.BlazorServer.Application.Actors;

public class ActorAction
{
	public ActorAction(Guid address, string name, string action)
	{
		Address = address;
		Name = name;
		Action = action;
	}
	
	public Guid Address { get; set; }
	
	// TODO - this should be an enum preferably
	public string Name { get; set; }
	
	// TODO - this should be an enum preferably
	public string Action { get; set; }
}
