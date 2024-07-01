namespace HNG.Abstractions.Parameters
{
    public class ServiceLogSearchParameters : QueryStringParameters
    {
        public long? ServiceLogId { get; set; }
        public int? ApplicationId { get; set; }
        public string? TraceId { get; set; }
        public string? Component { get; set; }
        public string? EntityId { get; set; }
        public string? RequestUri { get; set; }
        public string? RequestIpAddress { get; set; }
        public string? RequestUserName { get; set; }
        public string? ClientId { get; set; }
        public string? SessionId { get; set; }
        public string? CompanyId { get; set; }
        public string? ProfileId { get; set; }
        public int? UserId { get; set; }
        public DateTime? RequestDateFrom { get; set; }
        public DateTime? RequestDateTo { get; set; }
        public int? ResponseCode { get; set; }
    }

}
