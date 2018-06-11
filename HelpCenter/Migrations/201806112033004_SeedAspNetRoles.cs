namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedAspNetRoles : DbMigration
    {
        public override void Up()
        {
            Sql(@"INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1', N'Manager')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'2', N'Technician')
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'3', N'LeaseHolder')");
        }
        
        public override void Down()
        {
        }
    }
}
