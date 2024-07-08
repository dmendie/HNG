CREATE TABLE [dbo].[Users]
(
	[UserId]            NVARCHAR (255) NOT NULL,
	[Password]          NVARCHAR (MAX) NULL,
    [FirstName]         NVARCHAR (50)  NOT NULL,
    [LastName]          NVARCHAR (50)  NOT NULL,
    [Email]             NVARCHAR (255) NOT NULL,
    [Phone]             NVARCHAR (15) NOT NULL,
	[Status]            INT             CONSTRAINT [DF_User_Status] DEFAULT ((1)) NOT NULL,
    [CreatedOn]         DATETIME        CONSTRAINT [DF_User_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]         NVARCHAR (255)  CONSTRAINT [DF_User_CreatedBy] DEFAULT ('SYSTEM') NOT NULL,
    [ModifiedOn]        DATETIME        CONSTRAINT [DF_User_ModifiedOn] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        NVARCHAR (255)  CONSTRAINT [DF_User_ModifiedBy] DEFAULT ('SYSTEM') NOT NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY ([UserId]), 
)
