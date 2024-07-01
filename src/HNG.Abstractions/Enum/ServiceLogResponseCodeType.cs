namespace HNG.Abstractions.Enums
{
    public enum ServiceLogResponseCodeType
    {
        Unknown = 0,
        Successful = 1,
        BadRequest = 2,
        NotFound = 3,
        Unauthorized = 4,
        InternalServerError = 5,
        NetworkError = 6,
        VendorError = 7
    }
}
