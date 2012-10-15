ALTER TABLE [dbo].[GalleryTemplate]
    ADD CONSTRAINT [DF_GalleryTemplate_DateCreated] DEFAULT (getutcdate()) FOR [DateCreated];

