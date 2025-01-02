using HelmerDemo.BlazorServer.Application.Domain.Clock;
using HelmerDemo.BlazorServer.Application.Observables;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

/// <summary>
///     Clock with events
/// </summary>
public partial class ClockEv : ComponentBase, IDisposable
{
	[Inject]
	private IDigitalTimeObservable _clockObservable { get; set; }

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
		_clockObservable.DigitalTimeUpdated += OnTimeUpdated;
	}

	/// <summary>
	///     The event listener, listening to an external event
	/// </summary>
	/// <param name="source"></param>
	/// <param name="args"></param>
	private void OnTimeUpdated(object? source, DigitalTimeEventArgs args)
	{
		_currentTime = args.CurrentTime;
		InvokeAsync(StateHasChanged);
	}

	public void Dispose()
	{
		_clockObservable.DigitalTimeUpdated -= OnTimeUpdated;
		_clockObservable.Dispose();
	}
}
