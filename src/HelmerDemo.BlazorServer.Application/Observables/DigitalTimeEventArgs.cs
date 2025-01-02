using HelmerDemo.BlazorServer.Application.Domain.Clock;

namespace HelmerDemo.BlazorServer.Application.Observables;

/// <summary>
/// Use EventArgs to deliver the information <see cref="DigitalTime"/> to the event handler
/// </summary>
public class DigitalTimeEventArgs : EventArgs
{
	/// <summary>
	/// The time on the Digital Clock
	/// </summary>
	public DigitalTime CurrentTime { get; set; }

	/// <summary>
	/// The <see cref="time"/>
	/// </summary>
	/// <param name="time"></param>
	public DigitalTimeEventArgs(DigitalTime time)
	{
		CurrentTime = time;
	}
}
