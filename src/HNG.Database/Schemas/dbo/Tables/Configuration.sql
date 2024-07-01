CREATE TABLE [dbo].[Configuration] (
    [Entity]            NVARCHAR (100)  NOT NULL,
    [Key]               NVARCHAR (500)  NOT NULL,
    [Value]             NVARCHAR (1000) NOT NULL,
    [Status]            INT             CONSTRAINT [DF_Configuration_Status] DEFAULT ((1)) NOT NULL,
    [CreatedOn]         DATETIME        CONSTRAINT [DF_Configuration_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]         NVARCHAR (255)  CONSTRAINT [DF_Configuration_CreatedBy] DEFAULT ('SYSTEM') NOT NULL,
    [ModifiedOn]        DATETIME        CONSTRAINT [DF_Configuration_ModifiedOn] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]        NVARCHAR (255)  CONSTRAINT [DF_Configuration_ModifiedBy] DEFAULT ('SYSTEM') NOT NULL,
    CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED ([Entity] ASC, [Key] ASC)
);

