using HNG.Abstractions.Models;
using HNG.Abstractions.Parameters;
using HNG.Abstractions.Services.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HNG.Api.Client.Controllers
{
    /// <summary>
    /// ServiceLogs controller - use for troubleshooting api request calls. Provides  insight to what data request, response data, headers etc
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Tags("Service Logs")]
    [Route("v1/servicelogs")]
    public class ServiceLogsController : BaseApiController
    {

        readonly IServiceLogService ServiceLogService;

        /// <summary>
        /// ServiceLogsController Constructor 
        /// </summary> 
        public ServiceLogsController(IServiceLogService serviceLogService)
        {
            ServiceLogService = serviceLogService;
        }

        /// <summary>
        /// Search Service Logs
        /// </summary>
        [HttpGet]
        public async Task<PagedList<ServiceLogSearchResult>> SearchRequests([FromQuery] ServiceLogSearchParameters parameters)
        {
            var response = await ServiceLogService.SearchServiceLogs(parameters);
            return response;
        }

    }
}

