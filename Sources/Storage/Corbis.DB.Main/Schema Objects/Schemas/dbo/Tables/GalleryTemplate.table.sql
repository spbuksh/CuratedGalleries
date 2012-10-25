CREATE TABLE [dbo].[GalleryTemplate] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [Enabled]     BIT           NOT NULL,
    [PackageID]   BIGINT        NOT NULL,
    [IsDefault]   BIT           NOT NULL,
    [DateCreated] DATETIME2 (7) NOT NULL
);



