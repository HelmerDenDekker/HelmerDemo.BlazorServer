namespace HelmerDemo.BlazorServer.Application.Domain;

public class DigitalTime
{
    public int Hours { get; private set; }

    public int Minutes { get; private set; }

    public int Seconds { get; private set; }


    public DigitalTime(int hours, int minutes, int seconds)
    {
        Validate(hours, minutes, seconds);

        //Logic

        if (seconds == 60)
        {
            seconds = 0;
            minutes++;
        }

        if (minutes == 60)
        {
            minutes = 0;
            hours++;
        }

        if (hours == 24)
        {
            hours = 0;
        }

        // Set

        Hours = hours; Minutes = minutes; Seconds = seconds;
    }


    private static void Validate(int hours, int minutes, int seconds)
    {
        //Validation
        if (hours >= 24)
        {
            throw new ArgumentException("Invalid time");
        }

        if (minutes >= 60)
        {
            throw new ArgumentException("Invalid time");
        }

        if (seconds > 60)
        {
            throw new ArgumentException("Invalid time");
        }
    }
}
