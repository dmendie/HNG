CREATE TABLE [dbo].[ServiceLog]
(
	[ServiceLogId]      BIGINT NOT NULL IDENTITY,
    [ApplicationId]     INT NOT NULL,            -- unique application id
    [TraceId]           NVARCHAR(50)  NULL,      --correlation id used to group multiple entries together
    [Component]         NVARCHAR(100) NOT NULL,  --identifies part of program logging this entry - eg action method, method name, etc 
    [EntityId]          NVARCHAR(100) NULL,      -- generic string used for looking up - eg account id, etc

    [RequestUri]        NVARCHAR(500)  NOT NULL,      -- uri of the service request being made
    [RequestIpAddress]  NVARCHAR(50)   NOT NULL,  -- ip address or hostname of the machine making the request
    [RequestUserName]   NVARCHAR(100)  NOT NULL, -- username

    [ClientId]          NVARCHAR(100)  NULL, -- OAuth clientid (if applicable)
    [SessionId]         NVARCHAR(100)  NULL, -- OAuth session id (if applicable)
    [CustomerId]        NVARCHAR(100)  NULL, -- Customer Id -- customer app only
    [ProfileId]         NVARCHAR(100)  NULL, -- Profile Id -- customer app only
    [UserId]            INT  NULL,           -- User id - admin app only

    [RequestDate]       DATETIME2     NOT NULL, --timestamp of when the request was made (UTC)
    [ResponseDate]      DATETIME2     NULL,     --timestamp of when the response was returned (UTC)

    [RequestHeaders]    NVARCHAR(MAX) NULL, --raw request headers
    [ResponseHeaders]   NVARCHAR(MAX) NULL, --raw response headers

    [RequestData]       NVARCHAR(MAX) NULL, -- raw request content
    [ResponseData]      NVARCHAR(MAX) NULL, -- raw response content

    [ResponseCode]       INT NULL,   -- response code - success=1 - see LOOKUP RESPONSE_CODE for other values
    [ResponseStatusCode] INT NULL,   -- response code - success=1 - see LOOKUP RESPONSE_CODE for other values
    [ExtraInfo]          NVARCHAR(MAX) NULL, --any extra miscellaneous data the service wants to tie to the service call
    [ErrorMessage]      NVARCHAR(MAX) NULL, -- optional message containing details of error messages if the response ended up with an error

    [ServerHostName]    NVARCHAR(50) NOT NULL, -- name of the computer/host logging this request
    [ServerIpAddress]   NVARCHAR(50) NOT NULL, -- ip address of the computer/host logging this request 
    [ServerUserName]    NVARCHAR(100) NOT NULL, -- username the program is running under that is logging this request

    CONSTRAINT [PK_ServiceLog] PRIMARY KEY ([ApplicationId], [RequestDate], [Component], [ServiceLogId])
)

GO

CREATE INDEX [IX_ServiceLog_ApplicationId] ON [dbo].[ServiceLog] ([ApplicationId])
GO

CREATE INDEX [IX_ServiceLog_EntityId] ON [dbo].[ServiceLog] ([EntityId])
GO

CREATE INDEX [IX_ServiceLog_TraceId] ON [dbo].[ServiceLog] ([TraceId])
GO

CREATE INDEX [IX_ServiceLog_ResponseCode] ON [dbo].[ServiceLog] ([ResponseCode])
GO

CREATE INDEX [IX_ServiceLog_SessionId] ON [dbo].[ServiceLog] ([SessionId])
GO

CREATE INDEX [IX_ServiceLog_RequestDate] ON [dbo].[ServiceLog] ([RequestDate])
GO

CREATE INDEX [IX_ServiceLog_ResponseDate] ON [dbo].[ServiceLog] ([ResponseDate])
GO
