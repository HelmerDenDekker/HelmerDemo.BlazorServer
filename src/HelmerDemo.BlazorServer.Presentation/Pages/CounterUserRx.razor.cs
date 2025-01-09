using System.Reactive.Linq;
using HelmerDemo.BlazorServer.Application.Actors;
using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Application.Reactive;
using HelmerDemo.BlazorServer.Presentation.Components;
using HelmerDemo.BlazorServer.Presentation.ViewModel;
using Microsoft.AspNetCore.Components;

namespace HelmerDemo.BlazorServer.Presentation.Pages;

public partial class CounterUserRx: ComponentBase, IDisposable
{

	/// <summary>
	/// The maximum value allowed as count size of the buffer
	/// </summary>
	protected int MaxValue = 0;

	private CounterViewModel CounterContent = new();

	private IDisposable? _subscription;
	
	[CascadingParameter]
	private UserSessionProvider UserContainer { get; set; } = default!;
	
	private CounterActor? _counterActor;
	private IDisposable _createdSubscription;

	/// <summary>
	/// Add 1 to the count, until max value
	/// </summary>
	protected void IncrementCount()
	{
		_counterActor?.Post(new ActorAction("increment", "counter", Constants.CounterActorGuid));
	}

	/// <summary>
	/// overrides <see cref="OnAfterRender"/> event to subscribe to the listener and start the timer  
	/// </summary>
	/// <param name="firstRender"></param>
	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender && _counterActor == null)
		{
			// get counter actor from the user actor children
			if (UserContainer.MyUserActor == null)
			{
				OnError("User actor not found");
				return;
			}

			var actor = UserContainer.MyUserActor?.FindById(Constants.CounterActorGuid);
				
			var counterActor = (CounterActor)actor;

			if (counterActor != null)
			{
				SetCounterActor(counterActor);
			}
			else
			{
				_createdSubscription = UserContainer.MyUserActor.WhenNewMessageSent.Where(act=>act.CreatedCounter() && act.Address == UserContainer.MyUserActor.Address).Subscribe(
             					_ => SetCounterActor((CounterActor)UserContainer.MyUserActor?.FindById(Constants.CounterActorGuid)),
             					e => OnError(e.Message),
             					() => OnFinished());
				UserContainer.MyUserActor?.Post(new ActorAction("set", "counter", UserContainer.MyUserActor.Address, "20"));
				
			}
		}
		base.OnAfterRender(firstRender);
	}

	private void SetCounterActor(CounterActor counterActor)
	{
		_counterActor = counterActor;
		_subscription = _counterActor.WhenCounterChanged.Subscribe(
			rxo => OnCounterUpdated(rxo),
			e => OnError(e.Message),
			() => OnFinished());
	}

		
	/// <summary>
	/// During prerender, this component is rendered without calling OnAfterRender and then immediately disposed this means timer will be null so we have to check for null or use the Null-conditional operator ? 
	/// </summary>
	public void Dispose()
	{
		UserContainer.MyUserActor?.Post(new ActorAction("delete", "counter", UserContainer.MyUserActor.Address));
		_subscription?.Dispose();
		_counterActor?.Dispose();
		_createdSubscription?.Dispose();
	}

	/// <summary>
	/// The event listener
	/// </summary>
	private void OnCounterUpdated(CounterRxo counterRxo)
	{
		CounterContent.FromRxo(counterRxo);
		InvokeAsync(StateHasChanged);
	}
	
	private void OnFinished()
	{
		CounterContent.ErrorStyle = "text-warning";
		_subscription?.Dispose();
		_counterActor?.Dispose();
		InvokeAsync(StateHasChanged);
	}

	private void OnError(string errorMessage)
	{
		CounterContent.ErrorStyle = "text-danger";
		CounterContent.ErrorMessage = errorMessage;
		_subscription?.Dispose();
		_counterActor?.Dispose();
		InvokeAsync(StateHasChanged);
	}
}