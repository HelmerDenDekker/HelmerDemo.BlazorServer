namespace HelmerDemo.BlazorServer.Application.Observables;

public interface IDigitalTimeObservable : IDisposable
{
    /// <summary>
    /// Declare the event using <see cref="EventHandler"/>
    /// </summary>
    public event EventHandler<DigitalTimeEventArgs> DigitalTimeUpdated;
	
}
