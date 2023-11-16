using System.Reflection;

namespace HelmerDemo.BlazorServer.Shared.Tools.Models;

public class ApplicationVersion
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationVersion"/> class.
    /// </summary>
    public ApplicationVersion()
    {
        var number = Assembly.GetExecutingAssembly()?.GetName().Version?.ToString();
        Number = number ??= string.Empty;
        var name = Assembly.GetExecutingAssembly()?.GetName().Name?.ToString();
        Name = name ??= string.Empty;
    }

    /// <summary>
    /// Gets the version number in the form x.y.z.buildnumber
    /// </summary>
    public string Number { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the description of the version
    /// </summary>
    public string Name { get; private set; } = string.Empty;


}