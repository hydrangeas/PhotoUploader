﻿using System;
using Microsoft.AspNetCore.Http;

namespace CVPTest.Common
{
    /// <seealso cref="https://github.com/despos/ProgCore/blob/master/Src/Ch12/PartialRendering/Common/RequestExtensions.cs"/>
    public static class RequestExtensions
    {
        /// <summary>
        /// Checks whether the current request is coming via AJAX
        /// </summary>
        /// <param name="request">ASP.NET Request object</param>
        /// <returns>True or False</returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return true;
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        public static string GetAbsoluteUrl(this HttpRequest request, string relativeUrl)
        {
            return String.Format("{0}://{1}{2}",
                request.Scheme,
                request.Host,
                relativeUrl);
        }

    }
}
