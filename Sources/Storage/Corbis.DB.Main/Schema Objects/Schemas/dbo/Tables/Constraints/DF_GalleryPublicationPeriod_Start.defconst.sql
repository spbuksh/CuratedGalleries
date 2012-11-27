ALTER TABLE [dbo].[GalleryPublicationPeriod]
    ADD CONSTRAINT [DF_GalleryPublicationPeriod_Start] DEFAULT (getutcdate()) FOR [Start];

