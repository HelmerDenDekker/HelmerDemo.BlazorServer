using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Services;

public interface IClockService
{
    /// <summary>
    /// Start the clock with the current Time.
    /// </summary>
    /// <returns></returns>
    public Task<DigitalTime> StartClock();
}
