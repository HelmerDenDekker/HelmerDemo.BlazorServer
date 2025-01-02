using HelmerDemo.BlazorServer.Application.Domain.Clock;

namespace HelmerDemo.BlazorServer.Application.Observables;

public interface IDigitalTimeObservable
{
    /// <summary>
    /// Declare the event using <see cref="EventHandler"/>
    /// </summary>
    public event EventHandler<DigitalTimeEventArgs> DigitalTimeUpdated;
	
	/// <summary>
	/// Start the clock with the current Time.
	/// </summary>
	/// <returns></returns>
	public Task<DigitalTime> Start();
}
