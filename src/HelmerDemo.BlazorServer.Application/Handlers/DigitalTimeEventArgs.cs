using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Handlers;

/// <summary>
/// Event Argument class for <see cref="DigitalTime"/>; Use EventArgs to deliver the information  to the event handler
/// </summary>
public class DigitalTimeEventArgs : EventArgs
{
	/// <summary>
	/// The time on the Digital Clock
	/// </summary>
	public DigitalTime CurrentTime { get; set; }

	/// <summary>
	/// The <see cref="DigitalTimeHandler"/>
	/// </summary>
	/// <param name="time"></param>
	public DigitalTimeEventArgs(DigitalTime time)
	{
		CurrentTime = time;
	}
}
