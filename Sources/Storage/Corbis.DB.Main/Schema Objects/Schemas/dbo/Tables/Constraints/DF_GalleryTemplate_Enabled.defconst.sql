ALTER TABLE [dbo].[GalleryTemplate]
    ADD CONSTRAINT [DF_GalleryTemplate_Enabled] DEFAULT ((1)) FOR [Enabled];

