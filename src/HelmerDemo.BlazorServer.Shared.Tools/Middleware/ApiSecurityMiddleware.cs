using HelmerDemo.BlazorServer.Shared.Tools.Helpers;
using Microsoft.AspNetCore.Http;

namespace HelmerDemo.BlazorServer.Shared.Tools.Middleware
{
    /// <summary>
    /// Security Middleware for Web API
    /// </summary>
    public class ApiSecurityMiddleware : SecurityMiddleware
    {
        /// <summary>
        /// Override the SetSecurityHeaders for setting API specific security headers.
        /// </summary>
        /// <param name="context">The http Context.</param>
        public override void SetSecurityHeaders(HttpContext context)
        {
            var headers = SecurityHeaderHelper.ApiSecurityHeaders(36000);
            foreach (var header in headers)
            {
                if (!context.Response.Headers.ContainsKey(header.Key))
                {
                    context.Response.Headers.Add(header.Key, header.Value);
                }
            }
        }
    }
}
