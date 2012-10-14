CREATE TABLE [dbo].[EventLog] (
    [ID]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [DateTimeUTC]        DATETIME2 (7)  NOT NULL,
    [Level]              NVARCHAR (50)  NULL,
    [Message]            NVARCHAR (MAX) NULL,
    [Exception]          NVARCHAR (MAX) NULL,
    [Application]        NVARCHAR (255) NULL,
    [ApplicationVersion] NVARCHAR (50)  NULL,
    [Browser]            NVARCHAR (255) NULL,
    [UserID]             NVARCHAR (100) NULL
);

