using HNG.Abstractions.Contracts;

namespace HNG.Tests.Common
{
    public class ApiException : Exception
    {
        public ValidationResponseDTO? Details { get; internal set; }

        public ApiException() : base()
        {

        }

        public ApiException(ValidationResponseDTO? details) : base(details?.Message)
        {
            Details = details;
        }

        public ApiException(string? message) : base(message)
        {

        }

        public ApiException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
