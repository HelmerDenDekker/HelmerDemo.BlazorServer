using HelmerDemo.BlazorServer.Application.Domain.Clock;

namespace HelmerDemo.BlazorServer.Application.Reactive;

public interface IDigitalTimeStream : IDisposable
{
	public IObservable<DigitalTime> WhenDigitalTimeChanged { get; }
}
