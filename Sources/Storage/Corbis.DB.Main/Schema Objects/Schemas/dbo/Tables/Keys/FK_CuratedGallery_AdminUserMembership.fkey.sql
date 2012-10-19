ALTER TABLE [dbo].[CuratedGallery]
    ADD CONSTRAINT [FK_CuratedGallery_AdminUserMembership] FOREIGN KEY ([Editor]) REFERENCES [dbo].[AdminUserMembership] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

