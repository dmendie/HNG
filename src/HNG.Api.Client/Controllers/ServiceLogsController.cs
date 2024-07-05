using HNG.Abstractions.Models;
using HNG.Abstractions.Parameters;
using HNG.Abstractions.Services.Infrastructure;
using HNG.Api.Client.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HNG.Api.Admin.Controllers
{
    /// <summary>
    /// ServiceLogs controller  
    /// </summary>
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

