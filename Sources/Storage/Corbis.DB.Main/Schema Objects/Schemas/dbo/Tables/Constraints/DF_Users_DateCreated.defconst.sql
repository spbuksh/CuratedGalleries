ALTER TABLE [dbo].[AdminUsers]
    ADD CONSTRAINT [DF_Users_DateCreated] DEFAULT (getdate()) FOR [DateCreated];

