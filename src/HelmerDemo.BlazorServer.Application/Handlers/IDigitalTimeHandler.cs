using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Handlers;

public interface IDigitalTimeHandler
{ 
	/// <summary>
	/// delegate for raising an event when the time is updated
	/// </summary>
	public event EventHandler<DigitalTimeEventArgs> TimeUpdated;

	/// <summary>
	/// Call this method to raise the event
	/// </summary>
	/// <param name="time"></param>
	public void OnTimeUpdated(DigitalTime time);
}
