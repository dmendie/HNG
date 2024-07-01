using HNG.Abstractions.Contracts;
using HNG.Abstractions.Exceptions;
using HNG.Abstractions.Models;
using HNG.Business.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using SimpleValidator.Exceptions;
using static System.Net.Mime.MediaTypeNames;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension class that adds support for handling exceptions
    /// </summary>
    public static class CustomExceptionHandler
    {
        /// <summary>
        /// AppSettings
        /// </summary>
        public static AppSettings AppSettings { get; set; } = null!;

        /// <summary>
        /// Extension method that adds global support for handling exceptions
        /// </summary>
        public static void HandleException(IApplicationBuilder exceptionHandlerApp)
        {
            exceptionHandlerApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                context.Response.ContentType = Application.Json;
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (exceptionHandlerPathFeature?.Error is ValidationException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
                else if (exceptionHandlerPathFeature?.Error is NotFoundException)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                else if (exceptionHandlerPathFeature?.Error is PermissionDeniedException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }

                var response = ExceptionHelper.GetApiExceptionResponse(exceptionHandlerPathFeature?.Error);
                string json = JsonConvert.SerializeObject(response);

                if ((!AppSettings.Settings.EnableDetailedErrorMessages) && (context.Response.StatusCode == StatusCodes.Status500InternalServerError))
                {
                    var sanitizedResponse = new ValidationResponseDTO
                    {
                        Message = "An error occurred while processing your request. Please contact support for assistance."
                    };
                    json = JsonConvert.SerializeObject(sanitizedResponse);
                }

                await context.Response.WriteAsync(json);
            });
        }
    }
}
