ALTER TABLE [dbo].[AdminUserMembership]
    ADD CONSTRAINT [FK_AdminUserMembership_AdminUserProfile] FOREIGN KEY ([ProfileID]) REFERENCES [dbo].[AdminUserProfile] ([ID]) ON DELETE CASCADE ON UPDATE NO ACTION;

