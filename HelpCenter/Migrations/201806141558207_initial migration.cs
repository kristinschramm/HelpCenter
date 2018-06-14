namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmigration : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkOrderComments", new[] { "Commentor_Id" });
            DropColumn("dbo.WorkOrderComments", "CommentorId");
            RenameColumn(table: "dbo.WorkOrderComments", name: "Commentor_Id", newName: "CommentorId");
            CreateTable(
                "dbo.EMails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateDateTime = c.DateTime(nullable: false),
                        ToEmailAddress = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        Sent = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.WorkOrderComments", "CommentorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.WorkOrderComments", "CommentorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkOrderComments", new[] { "CommentorId" });
            AlterColumn("dbo.WorkOrderComments", "CommentorId", c => c.Int(nullable: false));
            DropTable("dbo.EMails");
            RenameColumn(table: "dbo.WorkOrderComments", name: "CommentorId", newName: "Commentor_Id");
            AddColumn("dbo.WorkOrderComments", "CommentorId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkOrderComments", "Commentor_Id");
        }
    }
}
