CREATE TABLE [dbo].[GalleryTemplate] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (150)  NOT NULL,
    [Description] NVARCHAR (MAX)  NULL,
    [ImageUrl]    NVARCHAR (2048) NULL,
    [Enabled]     BIT             NOT NULL,
    [Archive]     BIGINT          NOT NULL,
    [IsDefault]   BIT             NOT NULL,
    [DateCreated] DATETIME2 (7)   NOT NULL
);

