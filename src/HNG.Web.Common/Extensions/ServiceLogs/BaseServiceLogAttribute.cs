using HNG.Abstractions.Enums;
using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Infrastructure;
using HNG.Business.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HNG.Web.Common.Extensions.ServiceLogs
{
    /// <summary>
    /// Middleware base - responsible for logging API requests to the database
    /// </summary>
    public abstract class BaseServiceLogAttribute : Attribute, IAsyncResourceFilter
    {
        readonly IServiceLogService Logger;
        readonly AppSettings appSettings;

        /// <summary>
        /// ServiceLogActionEnhancerAttribute - constructor
        /// </summary>
        public BaseServiceLogAttribute(AppSettings appSettings, IServiceLogService logger)
        {
            Logger = logger;
            this.appSettings = appSettings;
        }

        /// <summary>
        /// ServiceLogActionEnhancerAttribute - OnResourceExecutionAsync
        /// </summary>
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            try
            {
                Logger.Log.ApplicationId = appSettings.AppId;

                Logger.Log.TraceId = GetHeaderValue(request.Headers, "TraceId") ?? Guid.NewGuid().ToString();
                Logger.Log.RequestUri = $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";
                Logger.Log.RequestHeaders = GetHeaders(request.Headers);
                Logger.Log.ServerUserName = Environment.UserName;
                Logger.Log.ServerHostName = Environment.MachineName;
                Logger.Log.ServerIpAddress = request.HttpContext.Connection.LocalIpAddress?.ToString() ?? "0.0.0.0";
                Logger.Log.RequestIpAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";

                Logger.Log.RequestData = await GetRequestBody(context.HttpContext?.Request);
                Logger.Log.Component = "Unknown";
                if (request.RouteValues.Any())
                {
                    var last = request.RouteValues.Last();
                    if (
                        !(
                            last.Key.Equals("action", StringComparison.OrdinalIgnoreCase) ||
                            last.Key.Equals("controller", StringComparison.OrdinalIgnoreCase)
                         )
                    )
                    {
                        Logger.Log.EntityId = $"{request.RouteValues.Last().Value}";
                    }
                }

                Enhance();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            } //intentionally - lose logs if logging causes an error


            //call action
            Logger.Log.RequestDate = DateTime.UtcNow;
            var executedContext = await next();
            Logger.Log.ResponseDate = DateTime.UtcNow;

            if (executedContext?.HttpContext?.Response != null)
            {
                var response = executedContext.HttpContext.Response;
                try
                {
                    Logger.Log.Component = $"{request.Method} {executedContext.ActionDescriptor?.AttributeRouteInfo?.Template}";

                    Logger.SetResponseCodes(Logger.Log, response.StatusCode);

                    if (executedContext.Result != null)
                    {
                        Logger.Log.ResponseData = GetResponseBody(executedContext.Result);
                    }

                    Logger.Log.ResponseHeaders = GetHeaders(response.Headers);

                    if (executedContext.Exception != null)
                    {
                        Logger.Log.ErrorMessage = executedContext.Exception.Message;

                        if (executedContext.Exception.StackTrace != null)
                        {
                            Logger.Log.ExtraInfo += executedContext.Exception.StackTrace;
                        }
                        
                        Logger.SetResponseCodes(Logger.Log, ExceptionHelper.GetApiExceptionResponseCode(executedContext.Exception));
                        var exceptionDto = ExceptionHelper.GetApiExceptionResponse(executedContext.Exception);
                        var errorJson = JsonConvert.SerializeObject(exceptionDto);
                        Logger.Log.ResponseData = errorJson;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            try
            {
                await Logger.LogAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        protected virtual void Enhance()
        {

        }

        private async Task<string?> GetRequestBody(HttpRequest? Request)
        {
            string? body = null;
            if (Request != null)
            {
                Request.EnableBuffering();
                Request.Body.Position = 0;
                var reader = new StreamReader(Request.Body);
                body = await reader.ReadToEndAsync();
                Request.Body.Position = 0;
            }
            body = string.IsNullOrWhiteSpace(body) ? null : body;

            return body;
        }

        private string? GetHeaderValue(IHeaderDictionary Headers, string headerKey)
        {
            var headerExists = Headers.TryGetValue(headerKey, out var headerValue);
            return headerExists ? $"{headerValue}" : null;
        }

        private string? GetHeaders(IHeaderDictionary Headers)
        {
            var exclusions = new List<string>() {"Authorization" };

            Dictionary<string, string?>? headers = null;
            if (Headers.Any())
            {
                headers = new Dictionary<string, string?>();
                foreach (var header in Headers)
                {
                    if (!exclusions.Contains(header.Key, StringComparer.OrdinalIgnoreCase))
                    {
                        headers.Add(header.Key, header.Value);
                    }
                }
            }

            if (headers == null)
            {
                return null;
            }
            else
            {
                return JsonConvert.SerializeObject(headers, Formatting.Indented);
            }
        }

        private string? GetResponseBody(IActionResult result)
        {

            string? response = null;
            if (result is JsonResult json)
            {
                var x = json.Value;
                var status = json.StatusCode;
                response = JsonConvert.SerializeObject(x);
            }
            if (result is ObjectResult o)
            {
                var x = o.Value;
                var status = o.StatusCode;
                response = JsonConvert.SerializeObject(x);
            }
            return response;
        }
    }
}
