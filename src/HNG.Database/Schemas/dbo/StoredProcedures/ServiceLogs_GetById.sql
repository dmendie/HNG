CREATE PROCEDURE dbo.ServiceLogs_GetById
(
	@ServiceLogId bigint,
    @Component NVARCHAR(100)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int

/* Beginning of procedure */

SELECT

	    s.ServiceLogId,
        s.ApplicationId,
        s.TraceId,
        s.Component,
        s.EntityId,
        s.RequestUri,
        s.RequestIpAddress,
        s.RequestUserName,
        s.ClientId,
        s.SessionId,
        s.[CustomerId],
        s.[CustomerId] as CompanyId, -- for backward compatibility - will be removed in the future
        s.ProfileId,
        s.UserId,
        s.RequestDate,
        s.ResponseDate,
        s.ResponseCode,
        s.ResponseStatusCode,
        s.ExtraInfo,
        s.ErrorMessage,
        s.ServerHostName,
        s.ServerIpAddress,
        s.ServerUserName,
        s.RequestData,
        s.ResponseData,
        s.RequestHeaders,
        s.ResponseHeaders

FROM 
	ServiceLog s    
WHERE
	s.ServiceLogId = @ServiceLogId AND
    (s.Component = @Component OR @Component IS NULL)

/* error-handling */
SELECT @error = @@error 
IF (@error <> 0) GOTO ERROR

RETURN 0

/* error-handling */
ERROR:
	RETURN @error

/* End of procedure*/
