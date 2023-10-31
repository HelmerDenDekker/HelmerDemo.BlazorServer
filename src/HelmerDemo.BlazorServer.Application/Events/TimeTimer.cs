using HelmerDemo.BlazorServer.Application.Domain;

namespace HelmerDemo.BlazorServer.Application.Events
{
    public class TimeTimer : ITimeTimer
	{
		/// <inheritdoc />
		public event EventHandler<TimeTimerEventArgs> OnTimeChanged = delegate { };

		/// <inheritdoc />
		public void RaiseTimeUpdate(DigitalClock time)
		{
			if (OnTimeChanged != null) OnTimeChanged(this, new TimeTimerEventArgs(time));
		}
	}
}
