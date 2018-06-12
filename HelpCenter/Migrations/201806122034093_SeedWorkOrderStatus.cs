namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class SeedWorkOrderStatus : DbMigration
    {
        public override void Up()
        {
            Sql(@"SET IDENTITY_INSERT dbo.WorkOrderStatus ON
                INSERT INTO dbo.WorkOrderStatus (Id, Name, IsOpen) VALUES (1, 'NEW', 1)
                INSERT INTO dbo.WorkOrderStatus (Id, Name, IsOpen) VALUES (2, 'ASSIGNED', 1)
                INSERT INTO dbo.WorkOrderStatus (Id, Name, IsOpen) VALUES (3, 'IN PROGRESS', 1)
                INSERT INTO dbo.WorkOrderStatus (Id, Name, IsOpen) VALUES (4, 'COMPLETED', 0)");
        }

        public override void Down()
        {
        }
    }
}
