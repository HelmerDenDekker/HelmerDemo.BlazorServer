using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Events;
using HelmerDemo.BlazorServer.Application.Services;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages
{
	public class ClockComponent : ComponentBase
	{
		[Inject]
		private ITimeTimer _timeTimer { get; set; }
		[Inject]
		private IClockService _clockService { get; set; }

		/// <summary>
		/// The digital time in the frontend
		/// </summary>
		protected DigitalClock CurrentTime = new DigitalClock(new DigitalTime(0,0,0));

		/// <summary>
		/// Overrides the OnInitialized to subscribe the listener
		/// </summary>
		/// <returns></returns>
		protected override async Task OnInitializedAsync()
		{
			_timeTimer.OnTimeChanged += TimeListener;
			var currentTime = await _clockService.StartClock();
			this.CurrentTime = new DigitalClock(currentTime);
		}

		/// <summary>
		/// The event listener, listening to an external event
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		private void TimeListener(object source, TimeTimerEventArgs args)
		{
			this.CurrentTime = args.CurrentTime;
			InvokeAsync(() => StateHasChanged());
		}
	}
}
