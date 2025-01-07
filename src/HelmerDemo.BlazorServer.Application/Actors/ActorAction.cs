namespace HelmerDemo.BlazorServer.Application.Actors;

public class ActorAction
{
	public ActorAction(string action, string name, Guid address, string value = null)
	{
		Address = address;
		ActorName = name;
		Action = action;
		Value = value;
	}
	
	
	
	public Guid Address { get; set; }
	
	// TODO - this should be an enum preferably
	public string ActorName { get; set; }
	
	// TODO - this should be an enum preferably
	public string Action { get; set; }
	
	public string Value { get; set; }
}
