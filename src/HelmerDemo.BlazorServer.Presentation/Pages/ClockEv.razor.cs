using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Handlers;
using HelmerDemo.BlazorServer.Application.Providers;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

/// <summary>
/// Clock with events
/// </summary>
public partial class ClockEv : ComponentBase, IDisposable
{
	[Inject]
	private IDigitalTimeHandler _timeTimer { get; set; }
	
	[Inject]
	private IClockProvider _clockService { get; set; }

	/// <summary>
	/// The digital time in the frontend
	/// </summary>
	protected DigitalTime CurrentTime = new(DateTime.Now);

	/// <summary>
	/// Overrides the OnInitialized to subscribe the listener
	/// </summary>
	/// <returns></returns>
	protected override async Task OnInitializedAsync()
	{
		_timeTimer.TimeUpdated += TimeListener;
		var currentTime = await _clockService.StartClock();
		this.CurrentTime = currentTime;
	}

	/// <summary>
	/// The event listener, listening to an external event
	/// </summary>
	/// <param name="source"></param>
	/// <param name="args"></param>
	private void TimeListener(object source, DigitalTimeEventArgs args)
	{
		this.CurrentTime = args.CurrentTime;
		InvokeAsync(StateHasChanged);
	}

	public void Dispose()
	{
		_timeTimer.TimeUpdated -= TimeListener;
		// TODO release managed resources here
	}
}