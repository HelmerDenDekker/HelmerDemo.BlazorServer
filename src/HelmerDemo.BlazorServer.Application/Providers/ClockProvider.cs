using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Handlers;

namespace HelmerDemo.BlazorServer.Application.Providers
{
    public class ClockProvider : IClockProvider
    {
        private readonly ITimeHandler _timeTimer;

		/// <summary>
		/// Initializes a new <see cref="ClockProvider"/>
		/// </summary>
		/// <param name="timeTimer">The Timer event</param>
		public ClockProvider(ITimeHandler timeTimer)
        {
            _timeTimer = timeTimer;
        }

		/// <inheritdoc />
		public async Task<DigitalTime> StartClock()
        {
            // start
            var currentTime = new DigitalTime(DateTime.Now);

            await AddSecond(currentTime);
            return currentTime;
        }

        /// <summary>
        /// Recursive function adding seconds (which it won't exactly, but this is for demo purposes, improvements done later
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private async Task AddSecond(DigitalTime time)
        {
            var second = Task.Delay(1000);
            var updatedTime = time.AddSecond();
            await second;
            //raise the Timer event to let the UI know it is time to update (loose coupling)
            _timeTimer.RaiseTimeUpdate(updatedTime);
            await AddSecond(updatedTime);
        }

		
    }
}
