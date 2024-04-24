using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Events;

public interface IClockTimer
{
    /// <summary>
    /// Declare the event using <see cref="EventHandler"/>
    /// </summary>
    public event EventHandler<ClockTimerEventArgs> ClockTimeUpdated;
	
	/// <summary>
	/// Start the clock with the current Time.
	/// </summary>
	/// <returns></returns>
	public Task<DigitalTime> Start();
}
