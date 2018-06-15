namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLocationWithLatLng : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Lat", c => c.Double(nullable: false));
            AddColumn("dbo.Locations", "Lng", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "Lng");
            DropColumn("dbo.Locations", "Lat");
        }
    }
}
