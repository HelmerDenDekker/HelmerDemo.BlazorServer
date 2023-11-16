using System.Timers;
using HelmerDemo.BlazorServer.Application.Domain;
using Microsoft.AspNetCore.Components;
using Timer = System.Timers.Timer;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

/// <summary>
/// Real simple example of the clock with the Timer
/// </summary>
public partial class Clock : ComponentBase, IDisposable
{
	/// <summary>
	/// The digital time in the frontend
	/// </summary>
	protected DigitalTime CurrentTime = new(DateTime.Now);

	private Timer _timer;

	/// <summary>
	/// overrides <see cref="OnAfterRender"/> event to subscribe to the listener and start the timer  
	/// </summary>
	/// <param name="firstRender"></param>
	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			_timer = new System.Timers.Timer();
			_timer.Interval = 1000;
			// Subscribe to the listener
			_timer.Elapsed += TimeListener;
			_timer.AutoReset = true;
			// Start the timer
			_timer.Enabled = true;
		}
		base.OnAfterRender(firstRender);
	}
		
	/// <summary>
	/// During prerender, this component is rendered without calling OnAfterRender and then immediately disposed this means timer will be null so we have to check for null or use the Null-conditional operator ? 
	/// </summary>
	public void Dispose()
	{
		if (_timer != null)
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