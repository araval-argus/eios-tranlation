

namespace eios_tranlation.core.ResponseMiddleware
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Extensions for Response Middleware.
    /// </summary>
    public static class ResponseMiddlewareExtensions
    {
        /// <summary>
        /// Extension method that ignores the formatiing.
        /// </summary>
        /// <param name="context">HttpContext.</param>
        /// <returns>A boolean indicating if the response should by pass the middleware.</returns> .
        public static bool Ignore(this HttpContext context)
        {
            return context.IsSwagger() || context.IsDownload() || context.IsNoContent();
        }

        /// <summary>
        /// Extension method that registers the middleware in the request pipeline.
        /// </summary>
        /// <param name="builder">IApplication Builder.</param>
        /// <param name="traceResponse">Trace Response.</param>
        /// <returns><see cref="IApplicationBuilder"/>object.</returns>
        public static IApplicationBuilder UseApiResponseWrapperMiddleware(this IApplicationBuilder builder, bool traceResponse = false)
        {
            return builder.UseMiddleware<ResponseMiddleware>(traceResponse);
        }

        /// <summary>
        /// Checks if the response is downloadeable type or not.
        /// </summary>
        /// <param name="context">HttpContext.</param>
        /// <returns>A boolean indicating if the response is downloadable type or not.</returns>
        internal static bool IsDownload(this HttpContext context)
        {
            bool isdowload = false;
            try
            {
                if (context.Response.ContentType != null)
                {
                    foreach (var downloaableType in DownloadableTypes())
                    {
                        bool isWildCard = downloaableType.Contains("*");
                        if (!isWildCard)
                        {
                            isdowload = downloaableType.Contains(context.Response.ContentType);
                        }
                        else
                        {
                            string[] mimeParts = downloaableType.Split("/", StringSplitOptions.RemoveEmptyEntries);
                            isdowload = mimeParts != null && mimeParts.Length > 0 && context.Response.ContentType.Contains(mimeParts[0]);
                        }

                        if (isdowload)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return isdowload;
        }

        /// <summary>
        /// Extension method that checks if the status code 204.
        /// </summary>
        /// <param name="context">HttpContext.</param>
        /// <returns>A boolean indicating whether the status code is 204 or not.</returns>
        internal static bool IsNoContent(this HttpContext context)
        {
            return context.Response.StatusCode == (int)HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Check if the request is of the swagger.
        /// </summary>
        /// <param name="context">HttpContext.</param>
        /// <returns>A boolean indicating whether the it is a Swagge request or not..</returns>
        internal static bool IsSwagger(this HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }

        /// <summary>
        /// List of downloadable mime types.
        /// </summary>
        /// <returns>A List of downloadable mime types.</returns>
        internal static List<string> DownloadableTypes()
        {
            return new List<string>
            {
                 MediaTypeNames.Application.Octet,
                 "image/*",
                 "video/*",
                 "audio/*",
                 "text/csv",
                 "application/pdf",
                 "application/json",
                 "application/xml",
                 "application/msword",
                 "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                 "application/vnd.ms-excel",
                 "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                 "application/vnd.ms-powerpoint",
                 "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                 "application/vnd.ms-outlook",
                 "application/x-zip-compressed",
                 "text/xml"
            };
        }
    }
}
