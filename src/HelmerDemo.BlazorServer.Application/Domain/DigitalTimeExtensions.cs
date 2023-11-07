namespace HelmerDemo.BlazorServer.Application.Domain;

public static class DigitalTimeExtensions
{
	/// <summary>
	/// Adds a second to the input time
	/// </summary>
	/// <param name="digitalTime"></param>
	/// <returns></returns>
	public static DigitalTime AddSecond(this DigitalTime digitalTime)
	{
		return new DigitalTime(digitalTime.Hours, digitalTime.Minutes, digitalTime.Seconds + 1);
	}
}
