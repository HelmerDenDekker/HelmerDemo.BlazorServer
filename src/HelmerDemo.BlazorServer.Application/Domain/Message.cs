namespace HelmerDemo.BlazorServer.Application.Domain;

public class Message
{
	private Message(string content)
	{
		Content = content;
		CreatedAt = DateTime.UtcNow;
	}

	public Message()
	{
		Content = "";
		CreatedAt = DateTime.UtcNow;
	}
	
	public string Content { get; private set; }
	
	/// <summary>
	/// Static, set only once
	/// </summary>
	public DateTime CreatedAt { get; }
	
	public static Message Create(string content)
	{
		return new Message(content);
	}
	
	public static Message Empty => new Message();
	
	public void ChangeContent(string newContent)
	{
		// This is a static method, so we can't access the instance members
		Content = newContent;
	}
}
