using HNG.Abstractions.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HNG.Abstractions.Models
{
    public class Organisation
    {
        public string OrgId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [JsonConverter(typeof(StringEnumConverter))]
        public DefaultStatusType Status { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedOn { get; set; }
    }
}
