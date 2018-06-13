namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsWorkOrderCommentAppUserIdAsString : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.WorkOrderComments", new[] { "Commentor_Id" });
            DropColumn("dbo.WorkOrderComments", "CommentorId");
            RenameColumn(table: "dbo.WorkOrderComments", name: "Commentor_Id", newName: "CommentorId");
            AlterColumn("dbo.WorkOrderComments", "CommentorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.WorkOrderComments", "CommentorId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.WorkOrderComments", new[] { "CommentorId" });
            AlterColumn("dbo.WorkOrderComments", "CommentorId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.WorkOrderComments", name: "CommentorId", newName: "Commentor_Id");
            AddColumn("dbo.WorkOrderComments", "CommentorId", c => c.Int(nullable: false));
            CreateIndex("dbo.WorkOrderComments", "Commentor_Id");
        }
    }
}
