using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Reactive;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

/// <summary>
/// Clock with ReactiveX
/// </summary>
public partial class ClockRxEv : ComponentBase, IDisposable
{
	/// <summary>
	/// The digital time in the frontend
	/// </summary>
	protected DigitalTime CurrentTime = new DigitalTime(DateTime.Now);
	

	private IDisposable? _subscription;
	private DigitalTimeStream _subject = new();

	/// <summary>
	/// Overrides the OnInitialized to subscribe the listener
	/// </summary>
	/// <returns></returns>
	protected override void OnInitialized()
	{
		// Subscribe
		_subscription = _subject.WhenDigitalTimeChanged.Subscribe(
			time =>
			{
				this.CurrentTime = time;
				InvokeAsync(StateHasChanged);
			}, e=> OnError(e.Message));
	}

	/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
	void IDisposable.Dispose()
	{
		_subject?.Dispose();
		_subscription?.Dispose();
	}
	
	private void OnError(string errorMessage)
	{
		ErrorMessage = errorMessage;
		InvokeAsync(StateHasChanged);
	}

	public string ErrorMessage { get; set; } = string.Empty;
}