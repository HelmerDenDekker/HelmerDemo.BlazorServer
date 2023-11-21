namespace HelmerDemo.BlazorServer.Application.Domain;

public class DigitalTime
{
    /// <summary>
    /// Gets the hours
    /// </summary>
    public int Hours { get; private set; }

    /// <summary>
    /// Gets the minutes
    /// </summary>
    public int Minutes { get; private set; }

    /// <summary>
    /// Gets the seconds
    /// </summary>
    public int Seconds { get; private set; }

    /// <summary>
    /// Creates a new digital time
    /// </summary>
    /// <param name="dateTime">The DateTime</param>
    public DigitalTime(DateTime dateTime)
    {
        Hours = dateTime.Hour; Minutes = dateTime.Minute; Seconds = dateTime.Second;
    }
    
    /// <summary>
    /// Creates a new digital time   
    /// </summary>
    /// <param name="hours"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
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

    
    
    /// <summary>
    /// Validates the input to hours, minutes and second
    /// </summary>
    /// <param name="hours"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    /// <exception cref="ArgumentException"></exception>
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
