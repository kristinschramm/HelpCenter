namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAppUserIncludeNameFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "NameFirst", c => c.String(nullable: false));
            AddColumn("dbo.AppUsers", "NameLast", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "NameLast");
            DropColumn("dbo.AppUsers", "NameFirst");
        }
    }
}
