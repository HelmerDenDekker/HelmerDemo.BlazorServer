using HelmerDemo.BlazorServer.Application.Domain.Clock;
using HelmerDemo.BlazorServer.Application.Reactive;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

/// <summary>
///     Clock with ReactiveX and a stream
/// </summary>
public partial class ClockRxEv : ComponentBase, IDisposable
{
	/// <summary>
	///     The digital time in the frontend
	/// </summary>
	private DigitalTime _currentTime = new(DateTime.Now);

	private IDisposable? _subscription;

	[Inject]
	private IDigitalTimeStream _clockObservable { get; set; }

	/// <summary>
	///     Overrides the OnInitialized to subscribe the listener
	/// </summary>
	/// <returns></returns>
	protected override void OnInitialized()
	{
		// Subscribe
		_subscription = _clockObservable.WhenDigitalTimeChanged.Subscribe(
			time =>
			{
				_currentTime = time;
				InvokeAsync(StateHasChanged);
			},
			e => OnError(e.Message));
	}

	/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
	public void Dispose()
	{
		_clockObservable.Dispose();
		_subscription?.Dispose();
	}

	private void OnError(string errorMessage)
	{
		ErrorMessage = errorMessage;
		InvokeAsync(StateHasChanged);
	}

	public string ErrorMessage { get; set; } = string.Empty;
}
