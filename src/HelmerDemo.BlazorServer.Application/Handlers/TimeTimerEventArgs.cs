using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Handlers;

/// <summary>
/// Use EventArgs to deliver the information <see cref="DigitalClock"/> to the event handler
/// </summary>
public class TimeHandlerEventArgs : EventArgs
{
	/// <summary>
	/// The time on the Digital Clock
	/// </summary>
	public DigitalTime CurrentTime { get; set; }

	/// <summary>
	/// The <see cref="TimeHandler"/>
	/// </summary>
	/// <param name="time"></param>
	public TimeHandlerEventArgs(DigitalTime time)
	{
		CurrentTime = time;
	}
}
