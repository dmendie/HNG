using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNG.Abstractions.Models
{
    public class ServiceLogSearchResult
    {
        public long ServiceLogId { get; set; }
        public int ApplicationId { get; set; }
        public string TraceId { get; set; } = null!;
        public string Component { get; set; } = null!;
        public string EntityId { get; set; } = null!;
        public string RequestUri { get; set; } = null!;
        public string RequestIpAddress { get; set; } = null!;
        public string RequestUserName { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string SessionId { get; set; } = null!;
        public string CompanyId { get; set; } = null!;
        public string ProfileId { get; set; } = null!;
        public int? UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public int? ResponseCode { get; set; }
        public int? ResponseStatusCode { get; set; }
        public string ExtraInfo { get; set; } = null!;
        public string ErrorMessage { get; set; } = null!;
        public string ServerHostName { get; set; } = null!;
        public string ServerIpAddress { get; set; } = null!;
        public string ServerUserName { get; set; } = null!;
        public string? RequestData { get; set; }
        public string? ResponseData { get; set; }
        public string? RequestHeaders { get; set; }
        public string? ResponseHeaders { get; set; }
    }
}
