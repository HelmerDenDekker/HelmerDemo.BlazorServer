namespace HelmerDemo.BlazorServer.Shared.Tools.Extensions;

/// <summary>
/// Extends the StringExtensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="input">this string</param>
    /// <returns>true if the value parameter is null or System.String.Empty, or if value consists exclusively of white-space characters.</returns>
    public static bool IsNullOrWhiteSpace(this string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }

    /// <summary>
    /// Indicates whether the specified string is null or an empty string ("").
    /// </summary>
    /// <param name="input">this string</param>
    /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
    public static bool IsNullOrEmpty(this string input)
    {
        return string.IsNullOrEmpty(input);
    }

    /// <summary>
    /// Combine two separate urls
    /// </summary>
    /// <param name="firstUrl">The first part of the url to be combined</param>
    /// <param name="secondUrl">The second part of the url to be combined</param>
    /// <returns>Combined url as a string</returns>
    public static string CombineUrls(this string firstUrl, string secondUrl)
    {
        firstUrl = firstUrl.TrimEnd('/');
        secondUrl = secondUrl.TrimStart('/');
        return string.Format("{0}/{1}", firstUrl, secondUrl);
    }

    /// <summary>
    /// Indicates whether the string is a valid email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool IsValidEmail(this string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false;
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Prepend a value
    /// </summary>
    /// <param name="input"></param>
    /// <param name="prepend"></param>
    /// <returns></returns>
    public static string PrePend(this string input, string prepend)
    {
        return prepend + input;
    }
}