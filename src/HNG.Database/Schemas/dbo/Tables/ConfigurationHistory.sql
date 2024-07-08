CREATE TABLE [dbo].[ConfigurationHistory] (
    [HistoryId]         INT IDENTITY (1, 1) NOT NULL,
    [Entity]            NVARCHAR (100)  NOT NULL,
    [Key]               NVARCHAR (500)  NOT NULL,
    [Value]             NVARCHAR (1000) NOT NULL,
    [Status]            INT             NOT NULL,
    [CreatedOn]         DATETIME        NOT NULL,
    [CreatedBy]         NVARCHAR (255)  NOT NULL,
    [ModifiedOn]        DATETIME        NOT NULL,
    [ModifiedBy]        NVARCHAR (255)  NOT NULL,
    CONSTRAINT [PK_ConfigurationHistory] PRIMARY KEY CLUSTERED ([HistoryId])
);

