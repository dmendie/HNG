using HNG.Abstractions.Contracts;
using HNG.Abstractions.Exceptions;
using SimpleValidator.Exceptions;

namespace HNG.Business.Helpers
{
    public static class ExceptionHelper
    {
        public static int GetApiExceptionResponseCode(Exception? exception)
        {
            int statusCode = 500;

            if (exception is ValidationException)
            {
                statusCode = 400;
            }
            else if (exception is NotFoundException)
            {
                statusCode = 404;
            }
            else if (exception is PermissionDeniedException)
            {
                statusCode = 401;
            }

            return statusCode;
        }

        public static ValidationResponseDTO GetApiExceptionResponse(Exception? Error)
        {
            if (Error is ValidationException)
            {
                var validationException = Error as ValidationException;
                var response = new ValidationResponseDTO
                {
                    Message = validationException?.Validator?.Errors[0].Message ?? "An error occurred",
                    Details = validationException?.Validator?.Errors ?? Enumerable.Empty<SimpleValidator.Results.ValidationError>().ToList(),
                };
                return response;
            }
            else if (Error is NotFoundException)
            {
                var response = new ValidationResponseDTO
                {
                    Message = Error.Message.Equals("Exception of type 'CIB.Abstractions.Exceptions.NotFoundException' was thrown.") ? "Not Found" : Error.Message
                };
                return response;
            }
            else if (Error is PermissionDeniedException permissionDeniedException)
            {
                var response = new ValidationResponseDTO
                {
                    Message = permissionDeniedException.PermissionName
                };
                return response;
            }
            else
            {
                string message = Error?.Message ?? "An error occurred";
                if (Error?.InnerException != null)
                {
                    message += Environment.NewLine + Error?.InnerException.Message;
                }
                var response = new ValidationResponseDTO
                {
                    Message = message
                };
                return response;
            }
        }

        public static string GetExceptionMessages(Exception e, string msgs = "")
        {
            if (e == null)
            {
                return string.Empty;
            }

            if (msgs == "")
            {
                msgs = e.Message;
            }

            if (e.InnerException != null)
            {
                msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
            }

            return msgs;
        }
    }
}
