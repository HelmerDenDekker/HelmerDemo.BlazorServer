using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Events;

/// <summary>
/// Use EventArgs to deliver the information <see cref="DigitalClockTime"/> to the event handler
/// </summary>
public class TimeTimerEventArgs : EventArgs
{
	/// <summary>
	/// The time on the Digital Clock
	/// </summary>
	public DigitalClock CurrentTime { get; set; }

	/// <summary>
	/// The <see cref="TimeTimer"/>
	/// </summary>
	/// <param name="time"></param>
	public TimeTimerEventArgs(DigitalClock time)
	{
		CurrentTime = time;
	}
}
