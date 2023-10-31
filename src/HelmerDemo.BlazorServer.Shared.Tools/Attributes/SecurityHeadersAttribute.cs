using HelmerDemo.BlazorServer.Shared.Tools.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HelmerDemo.BlazorServer.Shared.Tools.Attributes
{
    /// <summary>
    /// Attribute for Security Headers
    /// </summary>
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Overrides the OnResultExecuting and adds the Security headers
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;

            if (result is ViewResult)
            {
                AddHeaders(context, SecurityHeaderHelper.MvcSecurityHeaders(36000));
            }

            if (result is ActionResult)
            {
                AddHeaders(context, SecurityHeaderHelper.ApiSecurityHeaders(36000));
            }
        }

        /// <summary>
        /// This generic method adds the Security headers
        /// </summary>
        /// <param name="context">Th result context.</param>
        /// <param name="headers">Dictionary List of the security headers</param>
        private void AddHeaders(ResultExecutingContext context, Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                if (!context.HttpContext.Response.Headers.ContainsKey(header.Key))
                {
                    context.HttpContext.Response.Headers.Add(header.Key, header.Value);
                }
            }
        }
    }
}
