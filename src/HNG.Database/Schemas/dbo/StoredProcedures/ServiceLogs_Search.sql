-- SR0007: Microsoft.Rules.Data : Nullable columns can cause final results to be evaluated as NULL for the predicate.
-- (suppress this specific warning for this query)

CREATE PROCEDURE dbo.ServiceLogs_Search
(
    @ServiceLogId BIGINT = NULL,
    @ApplicationId INT = NULL,
    @TraceId NVARCHAR(50) = NULL,
    @Component NVARCHAR(100) = NULL,
    @EntityId NVARCHAR(100) = NULL,
    @RequestUri NVARCHAR(500) = NULL,
    @RequestIpAddress NVARCHAR(50) = NULL,
    @RequestUserName NVARCHAR(100) = NULL,
    @ClientId NVARCHAR(100) = NULL,
    @SessionId NVARCHAR(100) = NULL,
    @CompanyId NVARCHAR(100) = NULL,
    @ProfileId NVARCHAR(100) = NULL,
    @UserId INT = NULL,
    @RequestDateFrom DATETIME2 = NULL,
    @RequestDateTo DATETIME2 = NULL,
    @ResponseCode INT = NULL
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
BEGIN
    /* Declare local variables */
    DECLARE @error INT

    /* Beginning of procedure */

    SELECT TOP 1000
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
        s.ServerUserName
    FROM 
        ServiceLog s
    WHERE
        (s.ServiceLogId = @ServiceLogId OR @ServiceLogId IS NULL) AND
        (s.ApplicationId = @ApplicationId OR @ApplicationId IS NULL) AND
        (s.TraceId = @TraceId OR @TraceId IS NULL) AND
        (s.Component = @Component OR @Component IS NULL) AND
        (s.EntityId = @EntityId OR @EntityId IS NULL) AND
        ((s.RequestUri LIKE ('%' + @RequestUri + '%')) OR (@RequestUri IS NULL)) AND
        (s.RequestIpAddress = @RequestIpAddress OR @RequestIpAddress IS NULL) AND
        (s.RequestUserName = @RequestUserName OR @RequestUserName IS NULL) AND
        (s.ClientId = @ClientId OR @ClientId IS NULL) AND
        (s.SessionId = @SessionId OR @SessionId IS NULL) AND
        (s.[CustomerId] = @CompanyId OR @CompanyId IS NULL) AND
        (s.ProfileId = @ProfileId OR @ProfileId IS NULL) AND
        (s.UserId = @UserId OR @UserId IS NULL) AND
        ((@RequestDateFrom IS NULL AND s.RequestDate IS NOT NULL) OR s.RequestDate >= @RequestDateFrom) AND
        ((@RequestDateTo IS NULL AND s.RequestDate IS NOT NULL) OR s.RequestDate <= @RequestDateTo) AND
        (s.ResponseCode = @ResponseCode OR @ResponseCode IS NULL)

    ORDER BY
        s.ServiceLogId Desc

    /* error-handling */
    SELECT @error = @@error 
    IF (@error <> 0) GOTO ERROR

    RETURN 0

    /* error-handling */
    ERROR:
        RETURN @error
END
