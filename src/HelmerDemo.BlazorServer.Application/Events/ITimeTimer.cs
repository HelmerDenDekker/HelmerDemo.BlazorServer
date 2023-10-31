using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Events;

public interface ITimeTimer
{ 
	/// <summary>
	/// Delegate for raising an event when the time is updated
	/// </summary>
	public event EventHandler<TimeTimerEventArgs> OnTimeChanged;

	/// <summary>
	/// Call this method to raise the event
	/// </summary>
	/// <param name="time"></param>
	public void RaiseTimeUpdate(DigitalClock time);
}
