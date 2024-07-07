CREATE TABLE [dbo].[UserOrganisation]
(
	[UserId]			NVARCHAR (255)		NOT NULL ,
	[OrgId]             NVARCHAR (255)      NOT NULL, 
    [IsOwner]			BIT		        NOT NULL,
    [Status]            INT             CONSTRAINT [DF_UserOrg_Status] DEFAULT ((1)) NOT NULL,
	[CreatedOn]         DATETIME        CONSTRAINT [DF_UserOrg_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]         NVARCHAR (255)  CONSTRAINT [DF_UserOrg_CreatedBy] DEFAULT ('SYSTEM') NOT NULL,
    [ModifiedOn]        DATETIME        CONSTRAINT [DF_UserOrg_ModifiedOn] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        NVARCHAR (255)  CONSTRAINT [DF_UserOrg_ModifiedBy] DEFAULT ('SYSTEM') NOT NULL, 
    CONSTRAINT [PK_UserOrganisation] PRIMARY KEY ([UserId], [OrgId]),
)
