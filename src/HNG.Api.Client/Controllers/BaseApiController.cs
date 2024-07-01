using HNG.Web.Common.Extensions.ServiceLogs;
using Microsoft.AspNetCore.Mvc;

namespace HNG.Api.Client.Controllers
{
    /// <summary>
    /// Base API Controller that all controllers inherit from - hosts common functions/behavior
    /// </summary>
    [Produces("application/json")]
    [ApiController]
    [ServiceFilter(typeof(ClientServiceLogAttribute))]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Returns raw request body - only works if model binder not used - ie do not populate model in action parameters
        /// </summary>
        protected async Task<string> GetRequestBody()
        {
            Request.EnableBuffering();
            Request.Body.Position = 0;
            var reader = new StreamReader(Request.Body);
            var rawRequestBody = await reader.ReadToEndAsync();
            Request.Body.Position = 0;
            return rawRequestBody;
        }
    }
}
