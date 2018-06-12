namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedWorkOrderComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkOrderComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkOrderId = c.Int(nullable: false),
                        Comment = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        CommentorId = c.Int(nullable: false),
                        Commentor_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.Commentor_Id)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId, cascadeDelete: true)
                .Index(t => t.WorkOrderId)
                .Index(t => t.Commentor_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrderComments", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkOrderComments", "Commentor_Id", "dbo.AppUsers");
            DropIndex("dbo.WorkOrderComments", new[] { "Commentor_Id" });
            DropIndex("dbo.WorkOrderComments", new[] { "WorkOrderId" });
            DropTable("dbo.WorkOrderComments");
        }
    }
}
