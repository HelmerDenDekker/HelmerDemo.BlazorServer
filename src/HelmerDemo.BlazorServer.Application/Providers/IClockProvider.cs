using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Providers;

public interface IClockProvider
{
    /// <summary>
    /// Start the clock with the current Time.
    /// </summary>
    /// <returns></returns>
    public Task<DigitalTime> StartClock();
}
