using HNG.Abstractions.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HNG.Abstractions.Models
{
    public class User
    {
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        [JsonConverter(typeof(StringEnumConverter))]
        public DefaultStatusType Status { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string ModifiedBy { get; set; } = null!;

        public DateTime ModifiedOn { get; set; }
    }
}
