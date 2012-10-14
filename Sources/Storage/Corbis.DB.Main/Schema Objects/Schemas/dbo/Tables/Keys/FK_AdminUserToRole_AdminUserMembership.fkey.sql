ALTER TABLE [dbo].[AdminUserToRole]
    ADD CONSTRAINT [FK_AdminUserToRole_AdminUserMembership] FOREIGN KEY ([MemberID]) REFERENCES [dbo].[AdminUserMembership] ([ID]) ON DELETE CASCADE ON UPDATE NO ACTION;

