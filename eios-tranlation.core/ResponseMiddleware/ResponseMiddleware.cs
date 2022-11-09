

namespace eios_tranlation.core.ResponseMiddleware
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using eios_translation.core.Wrappers;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// A class representing response middleware.
    /// </summary>
    public class ResponseMiddleware
    {
        private readonly RequestDelegate next;

        private readonly bool traceResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next Request Delegate in pipeline.</param>
        /// <param name="traceResponse">Flag indicating trace response.</param>
        public ResponseMiddleware(RequestDelegate next, bool traceResponse = false)
        {
            this.next = next;
            this.traceResponse = traceResponse;
        }

        /// <summary>
        /// Invoke Next Request Delegate.
        /// </summary>
        /// <param name="context">HttpContext.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Ignore())
            {
                await this.next(context);
            }
            else
            {
                var originalBodyStream = context.Response.Body;

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    try
                    {
                        await this.next.Invoke(context);

                        if (!context.IsDownload())
                        {
                            if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                            {
                                var body = await FormatResponse(context.Response);
                                await this.HandleSuccessRequestAsync(context, body, (HttpStatusCode)context.Response.StatusCode);
                            }
                            else
                            {
                                await this.HandleNotSuccessRequestAsync(context, (HttpStatusCode)context.Response.StatusCode);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await this.HandleExceptionAsync(context, ex);
                    }
                    finally
                    {
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                    }
                }
            }
        }

        /// <summary>
        /// A method to format the response.
        /// </summary>
        /// <param name="response">HttpResponse.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private static async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return plainBodyText;
        }

        /// <summary>
        /// Handle API Exception.
        /// </summary>
        /// <param name="context">HttpContext.</param>
        /// <param name="exception">Exception.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ApiError apiError;
            int code;

            switch (exception)
            {
                case ApiException ex:

                    apiError = new ApiError(ex.Message)
                    {
                        ValidationErrors = ex.Errors
                    };

                    code = ex.StatusCode;
                    context.Response.StatusCode = code;

                    break;
                case UnauthorizedAccessException _:
                    apiError = new ApiError("Unauthorized Access");
                    code = (int)HttpStatusCode.Unauthorized;
                    context.Response.StatusCode = code;

                    break;
                default:
                    var msg = exception.GetBaseException().Message;
                    var stack = exception.StackTrace;

                    apiError = new ApiError(msg) { Details = stack };
                    code = (int)HttpStatusCode.InternalServerError;
                    context.Response.StatusCode = code;

                    break;
            }

            context.Response.ContentType = "application/json";

            var response = new ApiResponse<string>(code, ResponseMessageEnum.Exception.GetDescription(), null, apiError);

            var json = JsonConvert.SerializeObject(response);
            return context.Response.WriteAsync(json);
        }

        /// <summary>
        /// Handle API Error.
        /// </summary>
        /// <param name="context">HttpContext.</param>
        /// <param name="code">HttpStatusCode.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private Task HandleNotSuccessRequestAsync(HttpContext context, HttpStatusCode code)
        {
            if (context.IsNoContent())
            {
                return Task.CompletedTask;
            }

            context.Response.ContentType = "application/json";

            var apiError = new ApiError("Your request cannot be processed. Something wrong. Please contact support person.");

            var response = new ApiResponse<string>((int)code, ResponseMessageEnum.Failure.GetDescription(), null, apiError);
            context.Response.StatusCode = (int)code;

            var json = JsonConvert.SerializeObject(response);

            return context.Response.WriteAsync(json);
        }

        /// <summary>
        /// Handle API Success.
        /// </summary>
        /// <param name="context">HttpContext.</param>
        /// <param name="body">Response Body.</param>
        /// <param name="code">HttpStatusCode.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private Task HandleSuccessRequestAsync(HttpContext context, object body, HttpStatusCode code)
        {
            if (body is null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            ApiResponse<object> response;

            context.Response.ContentType = "application/json";

            ApiResponse<object> convertedBody = null;

            try
            {
                convertedBody = bool.TryParse(body.ToString(), out _) ? null : JsonConvert.DeserializeObject<ApiResponse<object>>(body.ToString());
            }
            catch
            {
                // ignored
            }

            if (convertedBody?.Result == null && (convertedBody?.StatusCode == null || convertedBody.StatusCode == 0))
            {
                var bodyText = !body.ToString().IsValidJson() ? JsonConvert.SerializeObject(body) : body.ToString();

                var bodyContent = JsonConvert.DeserializeObject<object>(bodyText);

                var type = bodyContent?.GetType();

                if (type == typeof(JObject))
                {
                    response = JsonConvert.DeserializeObject<ApiResponse<object>>(bodyText);

                    response = response.StatusCode != (int)code
                        ? new ApiResponse<object>((int)code, ((ResponseMessageEnum)response.StatusCode).GetDescription(), bodyContent)
                        : new ApiResponse<object>((int)code, ResponseMessageEnum.Success.GetDescription(), bodyContent);
                }
                else
                {
                    response = new ApiResponse<object>((int)code, ResponseMessageEnum.Success.GetDescription(), bodyContent);
                }
            }
            else
            {
                response = convertedBody;
            }

            var jsonString = JsonConvert.SerializeObject(response);

            if (!this.traceResponse)
            {
                return context.Response.WriteAsync(jsonString);
            }

            return context.Response.WriteAsync(jsonString);
        }
    }
}
