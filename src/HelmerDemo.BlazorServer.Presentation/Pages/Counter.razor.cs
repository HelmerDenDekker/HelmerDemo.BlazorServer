using System.Timers;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

public partial class Counter: ComponentBase, IDisposable
{
	private System.Timers.Timer _timer;

	/// <summary>
	/// Buffer to keep track of count
	/// </summary>
	protected int CurrentCount = 0;

	/// <summary>
	/// The maximum value allowed as count size of the buffer
	/// </summary>
	protected int MaxValue = 0;

	/// <summary>
	/// Visual style for errors (Demonstrates influencing styling from code-behind!)
	/// </summary>
	protected string ErrorStyle = "";

	/// <summary>
	/// Error message to inform the user the <see cref="MaxValue"/> was hit
	/// </summary>
	protected string ErrorMessage = "You hit the maximum value";

	/// <summary>
	/// Add 1 to the count, until max value
	/// </summary>
	protected void IncrementCount()
	{
		if(CurrentCount < MaxValue)
			CurrentCount++;
		else
		{
			ErrorStyle = "text-danger";
			//Stop timer and finish the events 
			_timer.Enabled = false;
			_timer.Elapsed -= OnTimeUpdated;
			_timer.Dispose();
		}
	}

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
			_timer.Elapsed += OnTimeUpdated;
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
			_timer.Elapsed -= OnTimeUpdated;
		}
		_timer.Dispose();
	}

	/// <summary>
	/// The event listener, listening to ElapsedEventArgs
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void OnTimeUpdated(object? sender, ElapsedEventArgs e)
	{
		IncrementCount();
		// To make sure that the state is in sync on both client and server, add InvokeAsync(() => StateHasChanged()); to the timer interval callback
		InvokeAsync(StateHasChanged);
	}
}