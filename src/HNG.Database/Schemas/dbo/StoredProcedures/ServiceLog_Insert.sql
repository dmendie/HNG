
CREATE PROCEDURE dbo.ServiceLog_Insert
(
	@ServiceLogId bigint OUTPUT,
	@ApplicationId int,
	@TraceId nvarchar(50),
	@Component nvarchar(100),
	@EntityId nvarchar(100),
	@RequestUri nvarchar(500),
	@RequestIpAddress nvarchar(50),
	@RequestUserName nvarchar(100),
	@ClientId nvarchar(100),
	@SessionId nvarchar(100),
	@CompanyId nvarchar(100),
	@ProfileId nvarchar(100),
	@UserId int,
	@RequestDate datetime2,
	@ResponseDate datetime2,
	@RequestHeaders nvarchar(max),
	@ResponseHeaders nvarchar(max),
	@RequestData nvarchar(max),
	@ResponseData nvarchar(max),
	@ResponseCode int,
    @ResponseStatusCode int, 
	@ExtraInfo nvarchar(max),
	@ErrorMessage nvarchar(max),
	@ServerHostName nvarchar(50),
	@ServerIpAddress nvarchar(50),
	@ServerUserName nvarchar(100)
)
WITH ENCRYPTION
AS
SET NOCOUNT ON
/* Declare local variables */
DECLARE
	@error int

/* Beginning of procedure */
BEGIN TRANSACTION

INSERT INTO ServiceLog 
(
	ApplicationId,
	TraceId,
	Component,
	EntityId,
	RequestUri,
	RequestIpAddress,
	RequestUserName,
	ClientId,
	SessionId,
	[CustomerId],
	ProfileId,
	UserId,
	RequestDate,
	ResponseDate,
	RequestHeaders,
	ResponseHeaders,
	RequestData,
	ResponseData,
	ResponseCode,
    ResponseStatusCode,
	ExtraInfo,
	ErrorMessage,
	ServerHostName,
	ServerIpAddress,
	ServerUserName
)
VALUES
(
	@ApplicationId,
	@TraceId,
	@Component,
	@EntityId,
	@RequestUri,
	@RequestIpAddress,
	@RequestUserName,
	@ClientId,
	@SessionId,
	@CompanyId,
	@ProfileId,
	@UserId,
	@RequestDate,
	@ResponseDate,
	@RequestHeaders,
	@ResponseHeaders,
	@RequestData,
	@ResponseData,
	@ResponseCode,
    @ResponseStatusCode,
	@ExtraInfo,
	@ErrorMessage,
	@ServerHostName,
	@ServerIpAddress,
	@ServerUserName
)

/* error-handling */
SELECT @error = @@error, @ServiceLogId = SCOPE_IDENTITY()
IF (@error <> 0) GOTO ERROR

COMMIT TRANSACTION
RETURN 0

/* error-handling */
ERROR:
	IF (@@trancount > 0) ROLLBACK TRANSACTION
	RETURN @error

/* End of procedure*/
