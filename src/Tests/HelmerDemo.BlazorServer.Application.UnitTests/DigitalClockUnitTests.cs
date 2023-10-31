using HelmerDemo.BlazorServer.Application.Domain;
using HelmerDemo.BlazorServer.Shared.Tools.Extensions;

namespace HelmerDemo.BlazorServer.Application.UnitTests
{
	public class DigitalClockUnitTests
	{
		[Fact]
		public void DigitalClockInit_WithZero_ShouldPrependZero()
		{
			var digitalTime = new DigitalTime(0,0,0);
			var displayTime = new DigitalClock(digitalTime);
			Assert.Equal("00", displayTime.Hours);
			Assert.Equal("00", displayTime.Minutes);
			Assert.Equal("00", displayTime.Seconds);
		}

		[Fact]
		public void Prepend_WithZero_ShouldPrependZero()
		{
			int zero = 0;
			var input = zero.ToString();
			var result = input.PrePend("0");
			Assert.Equal("00", result);
		}
	}
}