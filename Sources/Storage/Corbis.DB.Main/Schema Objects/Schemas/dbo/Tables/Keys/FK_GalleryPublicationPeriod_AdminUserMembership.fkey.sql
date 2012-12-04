ALTER TABLE [dbo].[GalleryPublicationPeriod]
    ADD CONSTRAINT [FK_GalleryPublicationPeriod_AdminUserMembership] FOREIGN KEY ([PublisherID]) REFERENCES [dbo].[AdminUserMembership] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

