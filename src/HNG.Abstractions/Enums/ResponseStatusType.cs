using System.ComponentModel;

namespace HNG.Abstractions.Enums
{
    public enum ResponseStatusType
    {
        [Description("success")]
        Success = 0,
        [Description("Bad request")]
        BadRequest = -1
    }
}
