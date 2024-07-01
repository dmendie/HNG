using HNG.Abstractions.Contracts;
using HNG.Abstractions.Models;
using HNG.Abstractions.Parameters;

namespace HNG.Abstractions.Services.Infrastructure
{
    public interface IServiceLogService
    {
        ServiceLogEntryDTO Log { get; }
        Task LogAsync(ServiceLogEntryDTO serviceLogEntryDTO);
        Task LogAsync();
        void SetResponseCodes(ServiceLogEntryDTO log, int statusCode);
        Task<PagedList<ServiceLogSearchResult>> SearchServiceLogs(ServiceLogSearchParameters parameters);
        Task<ServiceLogSearchResult> GetServiceLog(long serviceLogId, string? component);
    }
}
