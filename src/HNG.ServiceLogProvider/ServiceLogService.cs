using HNG.Abstractions.Contracts;
using HNG.Abstractions.Enums;
using HNG.Abstractions.Exceptions;
using HNG.Abstractions.Helpers;
using HNG.Abstractions.Models;
using HNG.Abstractions.Parameters;
using HNG.Abstractions.Services.Data;
using HNG.Abstractions.Services.Infrastructure;

namespace HNG.ServiceLogProvider
{
    public class ServiceLogService : IServiceLogService
    {
        readonly IServiceLogDataService ServiceLogDataService;
        public ServiceLogEntryDTO Log { get; private set; }
        const string intentionallyNotLogged = "Intentionally Excluded From Logs";

        public ServiceLogService(IServiceLogDataService serviceLogDataService)
        {
            ServiceLogDataService = serviceLogDataService;
            Log = new ServiceLogEntryDTO();
        }

        /// <summary>
        /// Logs using provided service log entry - typically used in libraries to custom log outgoing requests/responses
        /// </summary>
        public async Task LogAsync(ServiceLogEntryDTO entry)
        {
            if (entry.Enabled)
            {
                if (!entry.LogRequestHeaders && !String.IsNullOrWhiteSpace(entry.RequestHeaders))
                {
                    entry.RequestHeaders = intentionallyNotLogged;
                }

                if (!entry.LogRequestData && !String.IsNullOrWhiteSpace(entry.RequestData))
                {
                    entry.RequestData = intentionallyNotLogged;
                }

                if (!entry.LogResponseHeaders && !String.IsNullOrWhiteSpace(entry.ResponseHeaders))
                {
                    entry.ResponseHeaders = intentionallyNotLogged;
                }

                if (!entry.LogResponseData && !String.IsNullOrWhiteSpace(entry.ResponseData))
                {
                    entry.ResponseData = intentionallyNotLogged;
                }

                if (String.IsNullOrWhiteSpace(entry.RequestUserName))
                {
                    entry.RequestUserName = "Unknown";
                }

                entry.ServiceLogId = await ServiceLogDataService.Insert(null, entry.ApplicationId, entry.TraceId, entry.Component, entry.EntityId, entry.RequestUri, entry.RequestIpAddress, entry.RequestUserName, entry.ClientId, entry.SessionId, entry.CompanyId, entry.ProfileId, entry.UserId, entry.RequestDate, entry.ResponseDate, entry.RequestHeaders, entry.ResponseHeaders, entry.RequestData, entry.ResponseData, entry.ResponseCode, entry.ResponseHttpCode, entry.ExtraInfo, entry.ErrorMessage, entry.ServerHostName, entry.ServerIpAddress, entry.ServerUserName);
            }
        }

        /// <summary>
        /// Logs using in memory service log entry - typically used for automated logging in host for incoming requests- eg controller or console
        /// </summary>
        public Task LogAsync()
        {
            return LogAsync(this.Log);
        }

        /// <summary>
        /// Sets response codes based on http status codes
        /// </summary>
        public void SetResponseCodes(ServiceLogEntryDTO log, int statusCode)
        {
            log.ResponseHttpCode = statusCode;
            log.ResponseCode = (int)ServiceLogResponseCodeType.Unknown;

            if (log.ResponseHttpCode >= 200 && log.ResponseHttpCode < 300)
            {
                log.ResponseCode = (int)ServiceLogResponseCodeType.Successful;
            }

            if (log.ResponseHttpCode >= 400 && log.ResponseHttpCode < 500)
            {
                log.ResponseCode = (int)ServiceLogResponseCodeType.BadRequest;
            }

            if (log.ResponseHttpCode == 404 || log.ResponseHttpCode == 410)
            {
                log.ResponseCode = (int)ServiceLogResponseCodeType.NotFound;
            }

            if (log.ResponseHttpCode >= 500 && log.ResponseHttpCode < 600)
            {
                log.ResponseCode = (int)ServiceLogResponseCodeType.InternalServerError;
            }

            if (log.ResponseHttpCode == 401 || log.ResponseHttpCode == 403 || log.ResponseHttpCode == 407 || log.ResponseHttpCode == 511)
            {
                log.ResponseCode = (int)ServiceLogResponseCodeType.Unauthorized;
            }

            if (log.ResponseHttpCode == 408 || log.ResponseHttpCode == 502 || log.ResponseHttpCode == 504)
            {
                log.ResponseCode = (int)ServiceLogResponseCodeType.NetworkError;
            }
        }

        public async Task<PagedList<ServiceLogSearchResult>> SearchServiceLogs(ServiceLogSearchParameters parameters)
        {
            //if no parameters are sent then return empty list
            if (
                (parameters.ServiceLogId == null) &&
                (parameters.ApplicationId == null) &&
                (parameters.TraceId == null) &&
                (parameters.Component == null) &&
                (parameters.EntityId == null) &&
                (parameters.RequestUri == null) &&
                (parameters.RequestIpAddress == null) &&
                (parameters.RequestUserName == null) &&
                (parameters.ClientId == null) &&
                (parameters.SessionId == null) &&
                (parameters.CompanyId == null) &&
                (parameters.UserId == null) &&
                (parameters.RequestDateFrom == null) &&
                (parameters.RequestDateTo == null) &&
                (parameters.ResponseCode == null)
               )
            {
                return new PagedList<ServiceLogSearchResult>();
            }

            //if service log id is not sent then ensure we have a request date filter
            if (parameters.ServiceLogId == null)
            {
                //check we have request dates sent- if not default to request in last day
                if (parameters.RequestDateFrom == null && parameters.RequestDateTo == null)
                {
                    parameters.RequestDateFrom = DateTime.UtcNow.AddDays(-1);
                    parameters.RequestDateTo = DateTime.UtcNow;
                }

                //if both dates are the same then return items for that day
                if (parameters.RequestDateFrom != null && parameters.RequestDateTo != null && parameters.RequestDateFrom == parameters.RequestDateTo)
                {
                    parameters.RequestDateFrom = DateTime.UtcNow.AddDays(-1);
                    parameters.RequestDateTo = DateTime.UtcNow;
                }

                //if only one date is sent then set the other 1 day apart
                if (parameters.RequestDateFrom == null)
                {
                    parameters.RequestDateFrom = parameters.RequestDateTo!.Value.AddDays(-1);
                }

                //if only one date is sent then set the other 1 day apart
                if (parameters.RequestDateTo == null)
                {
                    parameters.RequestDateTo = parameters.RequestDateFrom!.Value.AddDays(1);
                }

                if (parameters.RequestDateFrom != null && parameters.RequestDateTo != null)
                {
                    //if dates are the wrong way around then return zero records
                    if (parameters.RequestDateFrom > parameters.RequestDateTo)
                    {
                        //return 0 records
                        return new PagedList<ServiceLogSearchResult>();
                    }

                    var requestDiff = parameters.RequestDateTo.Value.Subtract(parameters.RequestDateFrom.Value);
                    if (requestDiff.TotalDays > 31)
                    {
                        //if date range exceeds 30 days return 0 records
                        return new PagedList<ServiceLogSearchResult>();
                    }
                }
            }

            var data = await ServiceLogDataService.Search(null, parameters.ServiceLogId, parameters.ApplicationId, parameters.TraceId, parameters.Component, parameters.EntityId, parameters.RequestUri, parameters.RequestIpAddress, parameters.RequestUserName, parameters.ClientId, parameters.SessionId, parameters.CompanyId, parameters.ProfileId, parameters.UserId, parameters.RequestDateFrom, parameters.RequestDateTo, parameters.ResponseCode);

            //page data
            var pagedList = PagingHelper.ToPagedList(data, parameters.PageNumber, parameters.PageSize);

            return pagedList;
        }

        public async Task<ServiceLogSearchResult> GetServiceLog(long serviceLogId, string? component)
        {
            var data = await ServiceLogDataService.GetById(null, serviceLogId, component);

            return data == null ? throw new NotFoundException() : data;
        }
    }
}
