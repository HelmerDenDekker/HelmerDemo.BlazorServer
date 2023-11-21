using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Handlers;

public class DigitalTimeHandler : IDigitalTimeHandler
{
	/// <inheritdoc />
	public event EventHandler<DigitalTimeEventArgs> TimeUpdated = delegate { };

	/// <inheritdoc />
	public void OnTimeUpdated(DigitalTime time)
	{
		if (TimeUpdated != null) TimeUpdated(this, new DigitalTimeEventArgs(time));
	}
}