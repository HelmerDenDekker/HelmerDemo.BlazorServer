namespace HelmerDemo.BlazorServer.Presentation.Logic;

public class CookieLogic
{
	public static string CookieKey(string id) => $"helmerdemo-blazor-session.{id}";
}
