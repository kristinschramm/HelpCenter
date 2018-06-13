namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateEmailModel : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EMails");
        }
    }
}
