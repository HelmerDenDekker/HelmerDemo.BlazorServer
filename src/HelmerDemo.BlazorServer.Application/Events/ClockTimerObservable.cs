using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Events
{
	/// <summary>
	/// The observable
	/// </summary>
    public class ClockTimerObservable : IClockTimer
	{
		/// <summary>
		/// This class gets the EventHandler name, but it is not needed if the generic version of <see cref="EventHandler"/> is used. https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/events/how-to-publish-events-that-conform-to-net-framework-guidelines
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public delegate void ClockTimerEventHandler(object sender, ClockTimerEventArgs args);

		/// <summary>
		/// The non-generic way to define an event
		/// </summary>
		public event ClockTimerEventHandler NonGenericClockTimeUpdated;

        /// <inheritdoc />
        public event EventHandler<ClockTimerEventArgs> ClockTimeUpdated = delegate { };

		/// <inheritdoc />
		public async Task<DigitalTime> Start()
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
		/// Call this method to raise the event, in this case when time is updated https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-type-members
		/// </summary>
		/// <param name="time"></param>
		private void RaiseTimeUpdate(DigitalClock time)
		{
			if (ClockTimeUpdated != null) ClockTimeUpdated(this, new ClockTimerEventArgs(time));
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
			RaiseTimeUpdate(displayTime);
			await AddSecond(newTime);
		}
    }
}
