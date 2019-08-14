using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;

namespace CVPTest.Common
{
    /// <seealso cref="https://github.com/despos/ProgCore/blob/master/Src/Ch12/PartialRendering/Common/RequireReferrerAttribute.cs"/>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireReferrerAttribute : ActionMethodSelectorAttribute
    {
        public RequireReferrerAttribute(params string[] trustedServers)
        {
            TrustedServers = trustedServers;
        }

        /// <summary>
        /// Array of servers acceptable as referrers
        /// </summary>
        public string[] TrustedServers { get; }

        /// <summary>
        /// Determines if the action method is valid for the request
        /// </summary>
        /// <param name="routeContext">Route context</param>
        /// <param name="action">Descriptor of the action being called</param>
        /// <returns></returns>
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var referrer = routeContext.HttpContext.Request.Headers["Referer"].ToString();
            if (referrer == null)
                return false;
            referrer = referrer.Trim('/').ToLower();

            var list = TrustedServers.Select(ts =>
            {
                return routeContext
                                    .HttpContext
                                    .Request
                                    .GetAbsoluteUrl(ts)
                                    .Trim('/')
                                    .ToLower();
            }).ToList();
            var result = list.Any(ts => referrer == ts);
            return result;
        }
    }
}
