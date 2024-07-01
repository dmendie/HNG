using HNG.Abstractions.Models;
using System.Data;

namespace HNG.Abstractions.Services.Data
{
    public interface IServiceLogDataService : IDataService
    {
        Task<long> Insert(IDbTransaction? transaction, int ApplicationId, string? TraceId, string Component, string? EntityId, string RequestUri, string RequestIpAddress, string RequestUserName, string? ClientId, string? SessionId, string? CompanyId, string? ProfileId, int? UserId, DateTime RequestDate, DateTime? ResponseDate, string? RequestHeaders, string? ResponseHeaders, string? RequestData, string? ResponseData, int? ResponseCode, int? ResponseStatusCode, string? ExtraInfo, string? ErrorMessage, string ServerHostName, string ServerIpAddress, string ServerUserName);
        Task<IEnumerable<ServiceLogSearchResult>> Search(IDbTransaction? transaction, long? serviceLogId, int? applicationId, string? traceId, string? component, string? entityId, string? requestUri, string? requestIpAddress, string? requestUserName, string? clientId, string? sessionId, string? companyId, string? profileId, int? userId, DateTime? requestDateFrom, DateTime? requestDateTo, int? responseCode);

        Task<ServiceLogSearchResult?> GetById(IDbTransaction? transaction, long serviceLogId, string? component);
    }
}
