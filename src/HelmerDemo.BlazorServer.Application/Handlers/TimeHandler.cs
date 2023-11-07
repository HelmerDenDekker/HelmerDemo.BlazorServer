using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Handlers
{
    public class TimeHandler : ITimeHandler
	{
		/// <inheritdoc />
		public event EventHandler<TimeHandlerEventArgs> OnTimeChanged = delegate { };

		/// <inheritdoc />
		public void RaiseTimeUpdate(DigitalTime time)
		{
			if (OnTimeChanged != null) OnTimeChanged(this, new TimeHandlerEventArgs(time));
		}
	}
}
