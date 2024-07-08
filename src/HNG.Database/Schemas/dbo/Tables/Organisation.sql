CREATE TABLE [dbo].[Organisation]
(
	[OrgId]             NVARCHAR (255)             NOT NULL,
    [Name]              NVARCHAR (200)  NOT NULL,
    [Description]       NVARCHAR (1000) NOT NULL,
	[Status]            INT             CONSTRAINT [DF_Org_Status] DEFAULT ((1)) NOT NULL,
    [CreatedOn]         DATETIME        CONSTRAINT [DF_Org_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]         NVARCHAR (255)  CONSTRAINT [DF_Org_CreatedBy] DEFAULT ('SYSTEM') NOT NULL,
    [ModifiedOn]        DATETIME        CONSTRAINT [DF_Org_ModifiedOn] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        NVARCHAR (255)  CONSTRAINT [DF_Org_ModifiedBy] DEFAULT ('SYSTEM') NOT NULL, 
    CONSTRAINT [PK_Organisation] PRIMARY KEY ([OrgId]),
)
