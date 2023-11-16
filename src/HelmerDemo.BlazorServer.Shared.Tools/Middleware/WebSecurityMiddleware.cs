using HelmerDemo.BlazorServer.Shared.Tools.Helpers;
using HelmerDemo.BlazorServer.Shared.Tools.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HelmerDemo.BlazorServer.Shared.Tools.Middleware;

/// <summary>
/// Web security Middleware
/// </summary>
public class WebSecurityMiddleware : SecurityMiddleware
{
    private readonly HeaderSettings _headersettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebSecurityMiddleware"/> class.
    /// </summary>
    /// <param name="options"></param>
    public WebSecurityMiddleware(IOptions<HeaderSettings> options)
    {
        _headersettings = options.Value;
    }

    /// <summary>
    /// Overrides the security headers, setting secure web headers.
    /// </summary>
    /// <param name="context">The http context.</param>
    public override void SetSecurityHeaders(HttpContext context)
    {
        var headers = GetSecurityHeaders();
        foreach (var header in headers)
        {
            if (!context.Response.Headers.ContainsKey(header.Key))
            {
                context.Response.Headers.Add(header.Key, header.Value);
            }
        }
    }

    private Dictionary<string, string> GetSecurityHeaders()
    {
        if (!string.IsNullOrWhiteSpace(_headersettings.ContentSecurityPolicy))
        {
            return SecurityHeaderHelper.MvcSecurityHeaders(36000, _headersettings.ContentSecurityPolicy);
        }

        return SecurityHeaderHelper.MvcSecurityHeaders(36000);
    }
}