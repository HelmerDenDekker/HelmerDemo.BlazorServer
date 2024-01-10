using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Events;

/// <summary>
/// Use EventArgs to deliver the information <see cref="DigitalTime"/> to the event handler
/// </summary>
public class ClockTimerEventArgs : EventArgs
{
	/// <summary>
	/// The time on the Digital Clock
	/// </summary>
	public DigitalClock CurrentTime { get; set; }

	/// <summary>
	/// The <see cref="ClockTimerObservable"/>
	/// </summary>
	/// <param name="time"></param>
	public ClockTimerEventArgs(DigitalClock time)
	{
		CurrentTime = time;
	}
}
