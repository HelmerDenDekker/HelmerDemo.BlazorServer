using System.Reactive.Linq;
using HelmerDemo.BlazorServer.Application.Domain;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages
{
	public class ClockRxComponent : ComponentBase
	{

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
			CurrentTime = new DigitalTime(DateTime.Now);

			// Source
			IObservable<long> ticks = Observable.Timer(
				dueTime: TimeSpan.Zero,
				period: TimeSpan.FromSeconds(1));
			// Subscribe
			ticks.Subscribe(
				 tick => TimeListener());

			this.CurrentTime = new(DateTime.Now);
		}

		/// <summary>
		/// The event listener, listening to an external event
		/// </summary>
		private void TimeListener()
		{
			this.CurrentTime = CurrentTime.AddSecond();
			InvokeAsync(StateHasChanged);
		}
	}
}
