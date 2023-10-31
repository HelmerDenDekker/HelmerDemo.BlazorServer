using HelmerDemo.BlazorServer.Shared.Tools.Extensions;

namespace HelmerDemo.BlazorServer.Application.Domain
{
	public class DigitalClock
	{
		public DigitalClock(DigitalTime time)
		{
			Hours = time.Hours.ToString();
			Minutes = time.Minutes.ToString();
			Seconds = time.Seconds.ToString();

			if (time.Hours < 10)
			{
				Hours = Hours.PrePend("0");
			}
			if (time.Minutes < 10)
			{
				Minutes = Minutes.PrePend("0");
			}
			if (time.Seconds < 10)
			{
				Seconds = Seconds.PrePend("0");
			}
		}

		public string Hours { get; private set; }

		public string Minutes { get; private set; }

		public string Seconds { get; private set; }
	}
}
