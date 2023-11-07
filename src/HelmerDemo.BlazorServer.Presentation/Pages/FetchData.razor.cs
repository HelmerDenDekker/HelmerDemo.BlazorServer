using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Providers;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages
{
    public class FetchDataComponent: ComponentBase
    {
		[Inject]
		private WeatherForecastService _forecastService { get; set; }

		/// <summary>
		/// Weather forecasts
		/// </summary>
		protected WeatherForecast[]? Forecasts;

		/// <summary>
		/// Overrides the OnInitialized to call the service and fetch the data
		/// </summary>
		/// <returns></returns>
		protected override async Task OnInitializedAsync()
		{
			this.Forecasts = await this._forecastService.GetForecastAsync(DateTime.Now);
		}
    }
}
