using System.Reactive.Linq;
using HelmerDemo.BlazorServer.Application.Domain.Clock;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

public partial class ClockRx : ComponentBase, IDisposable
{
	private IDisposable? _subscription;

	/// <summary>
	///     The digital time in the frontend
	/// </summary>
	protected DigitalTime _currentTime = new(DateTime.Now);

	/// <summary>
	///     Overrides the OnInitialized to subscribe the listener
	/// </summary>
	/// <returns></returns>
	protected override void OnInitialized()
	{
		_currentTime = new DigitalTime(DateTime.Now);

		// Source
		IObservable<long> ticks = Observable.Timer(
			dueTime: TimeSpan.Zero,
			period: TimeSpan.FromSeconds(1));
		// Subscribe
		_subscription = ticks.Subscribe(_ =>
			{
				_currentTime = _currentTime.AddSecond();
				InvokeAsync(StateHasChanged);
			}
		);
	}

	public void Dispose()
	{
		_subscription?.Dispose();
	}
}
