ALTER TABLE [dbo].[GalleryPublicationPeriod]
    ADD CONSTRAINT [FK_GalleryPublicationPeriod_CuratedGallery] FOREIGN KEY ([GalleryID]) REFERENCES [dbo].[CuratedGallery] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

