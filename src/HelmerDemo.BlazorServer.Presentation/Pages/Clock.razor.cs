using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Domain.Clock;
using HelmerDemo.BlazorServer.Application.Observables;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages
{
	public class ClockComponent : ComponentBase
	{
		[Inject]
		private IDigitalTimeObservable _observableClock { get; set; }


		/// <summary>
		/// The digital time in the frontend
		/// </summary>
		protected DigitalTime CurrentTime = new DigitalTime(0,0,0);

		/// <summary>
		/// Overrides the OnInitialized to subscribe the listener
		/// </summary>
		/// <returns></returns>
		protected override async Task OnInitializedAsync()
		{
			_observableClock.DigitalTimeUpdated += OnTimeUpdated;
			var currentTime = await _observableClock.Start();
			CurrentTime = currentTime;
		}

		/// <summary>
		/// The event listener, listening to an external event
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		private void OnTimeUpdated(object source, DigitalTimeEventArgs args)
		{
			CurrentTime = args.CurrentTime;
			InvokeAsync(() => StateHasChanged());
		}
	}
}
