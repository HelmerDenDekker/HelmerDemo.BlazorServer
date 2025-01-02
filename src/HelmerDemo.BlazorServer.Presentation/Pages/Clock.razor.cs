using System.Timers;
using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Events;
using Microsoft.AspNetCore.Components;
using Timer = System.Timers.Timer;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

/// <summary>
/// Real simple example of the clock with the Timer
/// </summary>
public partial class Clock : ComponentBase, IDisposable
{
	public class ClockComponent : ComponentBase
	{
		[Inject]
		private IClockTimer _observableClock { get; set; }


		/// <summary>
		/// The digital time in the frontend
		/// </summary>
		protected DigitalClock CurrentTime = new DigitalClock(new DigitalTime(0,0,0));

	/// <summary>
	/// overrides <see cref="OnAfterRender"/> event to subscribe to the listener and start the timer  
	/// </summary>
	/// <param name="firstRender"></param>
	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			_observableClock.ClockTimeUpdated += OnTimeUpdated;
			var currentTime = await _observableClock.Start();
			this.CurrentTime = new DigitalClock(currentTime);
		}

		/// <summary>
		/// The event listener, listening to an external event
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		private void OnTimeUpdated(object source, ClockTimerEventArgs args)
		{
			_timer.Elapsed -= TimeListener;
		}
		_timer?.Dispose();
	}
	
	/// <summary>
	/// The event listener, listening to an external event
	/// </summary>
	/// <param name="source"></param>
	/// <param name="args"></param>
	private void TimeListener(object source, ElapsedEventArgs e)
	{
		this.CurrentTime = CurrentTime.AddSecond();
		InvokeAsync(StateHasChanged);
	}
}