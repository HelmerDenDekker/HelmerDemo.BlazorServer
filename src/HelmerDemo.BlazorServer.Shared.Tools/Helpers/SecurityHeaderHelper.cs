namespace HelmerDemo.BlazorServer.Shared.Tools.Helpers;

/// <summary>
/// This class contains the Security Headers (for use in attributes and middleware)
/// </summary>
public static class SecurityHeaderHelper
{
    private const string ContentSecurityPolicy = "Content-Security-Policy";
    private const string XContentSecurityPolicy = "X-Content-Security-Policy";
    private const string XFrameOptions = "X-Frame-Options";

    /// <summary>
    /// A list of Security Headers for MVC applications
    /// </summary>
    /// <param name="httpsExpiryTime">The time, in seconds, that the browser should remember that a site is only to be accessed using https</param>
    /// <returns>A Dictionary list with API headers</returns>
    public static Dictionary<string, string> ApiSecurityHeaders(int httpsExpiryTime)
    {
        var mvcHeaders = CommonSecurityHeaders(httpsExpiryTime);

        // Prevent XSS-attack https://infosec.mozilla.org/guidelines/web_security#content-security-policy
        var defaultContentSecurityPolicy = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self'; upgrade-insecure-requests;";
        mvcHeaders.Add(ContentSecurityPolicy, defaultContentSecurityPolicy);
        mvcHeaders.Add(XContentSecurityPolicy, defaultContentSecurityPolicy);

        // Prevent click-jacking https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
        mvcHeaders.Add(XFrameOptions, "SAMEORIGIN");

        return mvcHeaders;
    }

    /// <summary>
    /// A list of Security Headers for MVC applications
    /// </summary>
    /// <param name="httpsExpiryTime">The time, in seconds, that the browser should remember that a site is only to be accessed using https</param>
    /// <returns>A Dictionary list with MVC security headers</returns>
    public static Dictionary<string, string> MvcSecurityHeaders(int httpsExpiryTime)
    {
        var mvcHeaders = CommonSecurityHeaders(httpsExpiryTime);

        // Prevent MIME type sniffing https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
        mvcHeaders.Add("X-Content-Type-Options", "nosniff");

        // Prevent click-jacking https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
        mvcHeaders.Add(XFrameOptions, "SAMEORIGIN");

        // Prevent XSS-attack https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
        var defaultContentSecurityPolicy = "default-src 'self'; object-src 'none'; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts; base-uri 'self'; upgrade-insecure-requests;";
        mvcHeaders.Add(ContentSecurityPolicy, defaultContentSecurityPolicy);
        mvcHeaders.Add(XContentSecurityPolicy, defaultContentSecurityPolicy);

        // Minimum information principle  https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
        mvcHeaders.Add("Referrer-Policy", "no-referrer");

        // Prevent XSS attacks in IE and chrome (extends csp) https://infosec.mozilla.org/guidelines/web_security#x-xss-protection
        mvcHeaders.Add("X-XSS-Protection", "1; mode=block");

        // ToDo add Permissions-policy: https://www.permissionspolicy.com/
        return mvcHeaders;
    }

    /// <summary>
    /// Overloading <see cref="MvcSecurityHeaders"/> with custom contentSecurityPolicy. See https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
    /// </summary>
    /// <param name="httpsExpiryTime">The time, in seconds, that the browser should remember that a site is only to be accessed using https</param>
    /// <param name="contentSecurityPolicy">The custom content security policy. See https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy </param>
    /// <returns>A Dictionary list with MVC security headers</returns>
    public static Dictionary<string, string> MvcSecurityHeaders(int httpsExpiryTime, string contentSecurityPolicy)
    {
        var mvcHeaders = MvcSecurityHeaders(httpsExpiryTime);
        mvcHeaders.Remove(ContentSecurityPolicy);
        mvcHeaders.Remove(XContentSecurityPolicy);
        mvcHeaders.Add(ContentSecurityPolicy, contentSecurityPolicy);
        mvcHeaders.Add(XContentSecurityPolicy, contentSecurityPolicy);
        return mvcHeaders;
    }

    /// <summary>
    /// A list of generic Security Headers, these are included in the API and Web security methods, preferably use these methods and not this one.
    /// </summary>
    /// <param name="httpsExpiryTime">The time, in seconds, that the browser should remember that a site is only to be accessed using https</param>
    /// <returns>A Dictionary list with common security headers</returns>
    public static Dictionary<string, string> CommonSecurityHeaders(int httpsExpiryTime)
    {
        var mvcHeaders = new Dictionary<string, string>();

        // Always use Https: https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Strict-Transport-Security
        mvcHeaders.Add("Strict-Transport-Security", $"max-age={httpsExpiryTime}");

        return mvcHeaders;
    }
}