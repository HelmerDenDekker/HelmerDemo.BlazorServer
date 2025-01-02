using HelmerDemo.BlazorServer.Application.Domain.Clock;

namespace HelmerDemo.BlazorServer.Application.Observables
{
	/// <summary>
	/// The observable
	/// </summary>
    public class DigitalTimeObservable : IDigitalTimeObservable
	{
		/// <summary>
		/// This class gets the EventHandler name, but it is not needed if the generic version of <see cref="EventHandler"/> is used. https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/events/how-to-publish-events-that-conform-to-net-framework-guidelines
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public delegate void DigitalTimeEventHandler(object sender, DigitalTimeEventArgs args);

		/// <summary>
		/// The non-generic way to define an event
		/// </summary>
		public event DigitalTimeEventHandler NonGenericClockTimeUpdated;

        /// <inheritdoc />
        public event EventHandler<DigitalTimeEventArgs> DigitalTimeUpdated = delegate { };

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
		private void RaiseTimeUpdate(DigitalTime time)
		{
			DigitalTimeUpdated(this, new DigitalTimeEventArgs(time));
		}

        /// <summary>
        /// Recursive function adding seconds (which it won't exactly, but this is for demo purposes) TODO: Use timer instead!!
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private async Task AddSecond(DigitalTime time)
		{
			var second = Task.Delay(1000);
			var newTime = new DigitalTime(time.Hours, time.Minutes, time.Seconds + 1);
			await second;
			//raise the Timer event to let the UI know it is time to update (loose coupling)
			RaiseTimeUpdate(newTime);
			await AddSecond(newTime);
		}
	}
}
