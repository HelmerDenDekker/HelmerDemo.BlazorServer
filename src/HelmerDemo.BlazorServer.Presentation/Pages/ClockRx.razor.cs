using System.Reactive.Linq;
using HelmerDemo.BlazorServer.Application.Domain;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

public partial class ClockRx : ComponentBase, IDisposable
{
	private IDisposable? _subscription;

	/// <summary>
	///     The digital time in the frontend
	/// </summary>
	protected DigitalTime CurrentTime = new(DateTime.Now);

	/// <summary>
	///     Overrides the OnInitialized to subscribe the listener
	/// </summary>
	/// <returns></returns>
	protected override void OnInitialized()
	{
		CurrentTime = new DigitalTime(DateTime.Now);

		// Source
		IObservable<long> ticks = Observable.Timer(
			dueTime: TimeSpan.Zero,
			period: TimeSpan.FromSeconds(1));
		// Subscribe
		_subscription = ticks.Subscribe(_ =>
			{
				CurrentTime = CurrentTime.AddSecond();
				InvokeAsync(StateHasChanged);
			}
		);
	}

	public void Dispose()
	{
		_subscription?.Dispose();
	}
}
