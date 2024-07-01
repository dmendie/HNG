namespace HNG.Abstractions.Contracts
{
    public class ServiceLogEntryDTO
    {
        public bool Enabled { get; set; } = true;
        public bool LogRequestHeaders { get; set; } = true;
        public bool LogRequestData { get; set; } = true;
        public bool LogResponseHeaders { get; set; } = true;
        public bool LogResponseData { get; set; } = true;

        public long ServiceLogId { get; set; }
        public int ApplicationId { get; set; }
        public string? TraceId { get; set; }
        public string Component { get; set; } = null!;
        public string? EntityId { get; set; }
        public string RequestUri { get; set; } = null!;
        public string RequestIpAddress { get; set; } = null!;
        public string RequestUserName { get; set; } = null!;
        public string? ClientId { get; set; }
        public string? SessionId { get; set; }
        public string? CompanyId { get; set; }
        public string? ProfileId { get; set; }
        public int? UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string? RequestHeaders { get; set; }
        public string? ResponseHeaders { get; set; }
        public string? RequestData { get; set; }
        public string? ResponseData { get; set; }
        public int? ResponseCode { get; set; }
        public int? ResponseHttpCode { get; set; }
        public string? ExtraInfo { get; set; }
        public string? ErrorMessage { get; set; }
        public string ServerHostName { get; set; } = null!;
        public string ServerIpAddress { get; set; } = null!;
        public string ServerUserName { get; set; } = null!;
    }
}
