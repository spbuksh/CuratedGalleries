CREATE TABLE [dbo].[GalleryPublicationPeriod] (
    [ID]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [GalleryID] INT           NOT NULL,
    [Start]     DATETIME2 (7) NOT NULL,
    [End]       DATETIME2 (7) NULL
);



