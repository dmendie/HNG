using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HNG.Api.Client.ActionFilters;

/// <summary>
///Action filter that validates model class sent on post requests
/// </summary>
public class ValidationFilterAttribute : IActionFilter
{
    /// <summary>
    ///ValidationFilterAttribute Constructor
    /// </summary>
    public ValidationFilterAttribute()
    { }

    /// <summary>
    ///OnEexecuting filter
    /// </summary>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var action = context.RouteData.Values["action"];
        var controller = context.RouteData.Values["controller"];

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var param = context.ActionArguments.SingleOrDefault(x => x.Value.ToString().EndsWith("DTO")).Value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        if (param is null)
        {
            var errors = new List<object>
            {
                new { field = "Request body", message = "Object is null in request body" }
            };

            var errorResponse = new
            {
                errors = errors
            };

            context.Result = new UnprocessableEntityObjectResult(errorResponse);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(ms => ms.Value?.Errors.Count > 0)
                .Select(ms => new
                {
                    field = ms.Key,
                    message = ms.Value?.Errors.First().ErrorMessage
                })
                .ToList();

            var errorResponse = new
            {
                errors = errors
            };

            context.Result = new UnprocessableEntityObjectResult(errorResponse);
        }
    }

    /// <summary>
    ///OnActionexecuted filter
    /// </summary>
    public void OnActionExecuted(ActionExecutedContext context)
    { }
}
