namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLocationsLatLng : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE dbo.Locations SET Lat = 43.055862, Lng = -87.900033 WHERE Id = 1");
            Sql("UPDATE dbo.Locations SET Lat = 43.044897, Lng = -87.90967 WHERE Id = 2");
            Sql("UPDATE dbo.Locations SET Lat = 43.034135, Lng = -87.91194 WHERE Id = 3");
        }
        
        public override void Down()
        {
        }
    }
}
