ALTER TABLE [dbo].[Portfolio]
    ADD CONSTRAINT [DF_Profile_DateCreated] DEFAULT (getdate()) FOR [DateCreated];

