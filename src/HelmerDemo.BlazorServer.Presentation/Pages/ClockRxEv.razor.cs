using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Handlers;
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
	protected DigitalTime CurrentTime = new(DateTime.Now);
	
	/// <summary>
	/// Error message to inform the user
	/// </summary>
	protected string ErrorMessage = "";


	private IDisposable _subscription;
	private ClockSubject _subject;

	/// <summary>
	/// Overrides the OnInitialized to subscribe the listener
	/// </summary>
	/// <returns></returns>
	protected override async Task OnInitializedAsync()
	{
		CurrentTime = new DigitalTime(DateTime.Now);

		// Source
		_subject = new ClockSubject();
		var timer = _subject.Start();
		// Subscribe
		_subscription = timer.Subscribe(
			tick => TimeObserver(tick), e=> OnError(e.Message));
		
		this.CurrentTime = new(DateTime.Now);
	}

	/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
	void IDisposable.Dispose()
	{
		_subject?.Stop();
		_subscription?.Dispose();
	}
	
	/// <summary>
	/// Observer
	/// </summary>
	/// <param name="time"></param>
	private void TimeObserver(DigitalTime time) 
	{
		this.CurrentTime = time;
		InvokeAsync(StateHasChanged);
	}
	
	private void OnError(string errorMessage)
	{
		ErrorMessage = errorMessage;
		InvokeAsync(StateHasChanged);
	}
}