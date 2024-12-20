namespace HelmerDemo.BlazorServer.Presentation.ViewModel;

public class UserSessionViewModel
{
	public UserSessionState State { get; set; } = UserSessionState.Loading;
	
	public string SystemMessage { get; set; } = "Loading...";
}
