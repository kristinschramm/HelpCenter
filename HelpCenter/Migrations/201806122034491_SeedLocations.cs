namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class populatelocations : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT [dbo].[Locations] ON");
            Sql("INSERT INTO [dbo].[Locations]([Id], [Name], [Address], [City], [State], [Zip]) VALUES(1, N'Selenium Crossings', N'1888 N Water Ave', N'Milwaukee', N'WI', N'53202')");
            Sql("INSERT INTO [dbo].[Locations] ([Id], [Name], [Address], [City], [State], [Zip]) VALUES(2, N'Selenium River Run', N'270 E Highland Ave', N'Milwaukee', N'WI', N'53202')");
            Sql("INSERT INTO [dbo].[Locations] ([Id], [Name], [Address], [City], [State], [Zip]) VALUES(3, N'Selenium Estates', N'313 N Plankinton Ave ', N'Milwaukee', N'WI', N'53203')");
            Sql("SET IDENTITY_INSERT[dbo].[Locations] OFF ");
        }


        public override void Down()
        {
        }
    }
}