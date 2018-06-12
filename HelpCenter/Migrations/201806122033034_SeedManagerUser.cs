namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using HelpCenter.Models;

    public partial class SeedManagerUser : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'dc253146-192e-4c38-b6e6-e020274f711d', N'manager@help.com', 0, N'AG/emQxgQ4mTc+T16JcqVoeV5G70aKdGpvHsIjW/RcNo2p78EdZM39Ip3y4a8kOlDQ==', N'8a078225-604f-4337-9785-b8c82bac1f41', NULL, 0, 0, NULL, 1, 0, N'manager@help.com')");
            Sql("INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'dc253146-192e-4c38-b6e6-e020274f711d', N'1')");
            var _context = new ApplicationDbContext();
            var managerAppUser = new AppUser()
            {
                Id = "dc253146-192e-4c38-b6e6-e020274f711d",
                NameFirst = "Manager",
                NameLast = "Manager",
                EmailAddress = "manager@help.com",
                PhoneNumber = "4148470000"
            };
            _context.AppUsers.Add(managerAppUser);
            _context.SaveChanges();
        }

        public override void Down()
        {
        }
    }
}