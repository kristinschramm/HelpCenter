namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedWorkOrderCategories : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT [dbo].[WorkOrderCategories] ON");
            Sql("INSERT INTO dbo.WorkOrderCategories (Id, Name) VALUES (1, 'Applicance')");
            Sql("INSERT INTO dbo.WorkOrderCategories (Id, Name) VALUES (2, 'Electrical')");
            Sql("INSERT INTO dbo.WorkOrderCategories (Id, Name) VALUES (3, 'Internet')");
            Sql("INSERT INTO dbo.WorkOrderCategories (Id, Name) VALUES (4, 'Plumbing')");
            Sql("INSERT INTO dbo.WorkOrderCategories (Id, Name) VALUES (5, 'Other')");
            Sql("SET IDENTITY_INSERT [dbo].[WorkOrderCategories] OFF");
        }
        
        public override void Down()
        {
        }
    }
}
