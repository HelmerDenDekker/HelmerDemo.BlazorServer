using HelmerDemo.BlazorServer.Shared.Tools.Helpers;
using Microsoft.AspNetCore.Http;

namespace HelmerDemo.BlazorServer.Shared.Tools.Middleware;

/// <summary>
/// Generic SecurityMiddleware class, please use specific implementations of this class
/// </summary>
public class SecurityMiddleware : IMiddleware
{
    /// <inheritdoc/>
    #pragma warning disable AsyncFixer01 // Unnecessary async/await usage
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.OnStarting(
            (state) =>
            {
                SetSecurityHeaders(context);
                return Task.FromResult(0);
            },
            context);

        await next(context);
    }

    /// <summary>
    /// A method to set the securityHeaders.
    /// </summary>
    /// <param name="context">The http context.</param>
    public virtual void SetSecurityHeaders(HttpContext context)
    {
        var headers = SecurityHeaderHelper.CommonSecurityHeaders(36000);
        foreach (var header in headers)
        {
            if (!context.Response.Headers.ContainsKey(header.Key))
            {
                context.Response.Headers.Add(header.Key, header.Value);
            }
        }
    }
}

// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/extensibility?view=aspnetcore-5.0
