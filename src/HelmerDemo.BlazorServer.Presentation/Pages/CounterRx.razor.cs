using System.Timers;
using HelmerDemo.BlazorServer.Application.Handlers;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

public partial class CounterRx: ComponentBase, IDisposable
{
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

	private IDisposable _subscription;

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
			//Finish the events
			Dispose();
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
			var eventDemo = new CounterSubject();
			var observable = eventDemo.Start(MaxValue);
			_subscription = observable.Subscribe(
				p => TimerIntervalListener(),
				e => OnError(e.Message),
				() => OnFinished());
		}
		base.OnAfterRender(firstRender);
	}

		
	/// <summary>
	/// During prerender, this component is rendered without calling OnAfterRender and then immediately disposed this means timer will be null so we have to check for null or use the Null-conditional operator ? 
	/// </summary>
	public void Dispose()
	{
		_subscription?.Dispose();
	}

	/// <summary>
	/// The event listener, listening to ElapsedEventArgs
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void TimerIntervalListener()
	{
		IncrementCount();
		// To make sure that the state is in sync on both client and server, add InvokeAsync(() => StateHasChanged()); to the timer interval callback
		InvokeAsync(StateHasChanged);
	}
	
	private void OnFinished()
	{
		ErrorStyle = "text-danger";
		ErrorMessage = "Count Dracula has finished counting";
		InvokeAsync(StateHasChanged);
	}

	private void OnError(string errorMessage)
	{
		ErrorStyle = "text-danger";
		ErrorMessage = errorMessage;
		InvokeAsync(StateHasChanged);
	}
}