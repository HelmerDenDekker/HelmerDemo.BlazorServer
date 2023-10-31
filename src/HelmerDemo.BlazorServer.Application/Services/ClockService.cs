using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Events;

namespace HelmerDemo.BlazorServer.Application.Services
{
    public class ClockService : IClockService
    {
        private readonly ITimeTimer _timeTimer;

		/// <summary>
		/// Initializes a new <see cref="ClockService"/>
		/// </summary>
		/// <param name="timeTimer">The Timer event</param>
		public ClockService(ITimeTimer timeTimer)
        {
            _timeTimer = timeTimer;
        }

		/// <inheritdoc />
		public async Task<DigitalTime> StartClock()
        {
            // start
            var time = DateTime.Now;
            var hours = time.Hour;
            var minutes = time.Minute;
            var seconds = time.Second;
            var newTime = new DigitalTime(hours, minutes, seconds);

            await AddSecond(newTime);
            return newTime;
        }

        /// <summary>
        /// Recursive function adding seconds (which it won't exactly, but this is for demo purposes, improvements done later
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private async Task AddSecond(DigitalTime time)
        {
            var second = Task.Delay(1000);
            var newTime = new DigitalTime(time.Hours, time.Minutes, time.Seconds + 1);
			var displayTime = new DigitalClock(newTime);
            await second;
            //raise the Timer event to let the UI know it is time to update (loose coupling)
            _timeTimer.RaiseTimeUpdate(displayTime);
            await AddSecond(newTime);
        }

		
    }
}
