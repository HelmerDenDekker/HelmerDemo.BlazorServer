using System.Timers;
using HelmerDemo.BlazorServer.Application.Domain.Clock;
using Timer = System.Timers.Timer;

namespace HelmerDemo.BlazorServer.Application.Observables
{
	/// <summary>
	/// The observable
	/// </summary>
    public class DigitalTimeObservable : IDigitalTimeObservable
	{
		private readonly Timer _timer;
		private DigitalTime _time;
		
		/// <summary>
		/// It is a hot observable, added as a singleton to the application, so it starts as the application is started.
		/// </summary>
		public DigitalTimeObservable()
		{
			// start
			_time = new DigitalTime(DateTime.Now);
			_timer = new Timer();
			_timer.Interval = 1000;
			_timer.Elapsed += OnTimeUpdated;
			// start timer
			_timer.Enabled = true;
		}

		private void OnTimeUpdated(object? sender, ElapsedEventArgs e)
		{
			_time = _time.AddSecond();
			RaiseTimeUpdate(_time);
		}

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
		

		/// <summary>
		/// Call this method to raise the event, in this case when time is updated https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-type-members
		/// </summary>
		/// <param name="time"></param>
		private void RaiseTimeUpdate(DigitalTime time)
		{
			DigitalTimeUpdated(this, new DigitalTimeEventArgs(time));
		}

		public void Dispose()
		{
			_timer.Elapsed -= OnTimeUpdated;
			_timer.Dispose();
		}
	}
}
