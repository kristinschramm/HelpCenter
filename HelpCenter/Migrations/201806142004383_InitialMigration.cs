namespace HelpCenter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        NameFirst = c.String(nullable: false),
                        NameLast = c.String(nullable: false),
                        PhoneNumber = c.String(),
                        EmailAddress = c.String(),
                        LocationId = c.Int(),
                        UnitId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Units", t => t.UnitId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.UnitId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Zip = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: false)
                .Index(t => t.LocationId);
            
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
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionDateTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        WorkOrderId = c.Int(),
                        LeaseHolderId = c.String(maxLength: 128),
                        LocationId = c.Int(nullable: false),
                        UnitId = c.Int(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.LeaseHolderId)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Units", t => t.UnitId, cascadeDelete: true)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId)
                .Index(t => t.WorkOrderId)
                .Index(t => t.LeaseHolderId)
                .Index(t => t.LocationId)
                .Index(t => t.UnitId);
            
            CreateTable(
                "dbo.WorkOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        CategoryId = c.Int(),
                        StatusId = c.Int(nullable: false),
                        StatusDateTime = c.DateTime(nullable: false),
                        RequestorId = c.String(maxLength: 128),
                        LocationId = c.Int(),
                        UnitId = c.Int(),
                        AssignedUserId = c.String(maxLength: 128),
                        CreateDateTime = c.DateTime(nullable: false),
                        ModifiedDateTime = c.DateTime(nullable: false),
                        ExpectedCompletionDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.AssignedUserId)
                .ForeignKey("dbo.WorkOrderCategories", t => t.CategoryId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.AppUsers", t => t.RequestorId)
                .ForeignKey("dbo.WorkOrderStatus", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("dbo.Units", t => t.UnitId)
                .Index(t => t.CategoryId)
                .Index(t => t.StatusId)
                .Index(t => t.RequestorId)
                .Index(t => t.LocationId)
                .Index(t => t.UnitId)
                .Index(t => t.AssignedUserId);
            
            CreateTable(
                "dbo.WorkOrderCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkOrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsOpen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WorkOrderComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkOrderId = c.Int(nullable: false),
                        Comment = c.String(),
                        CreateDateTime = c.DateTime(nullable: false),
                        CommentorId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.CommentorId)
                .ForeignKey("dbo.WorkOrders", t => t.WorkOrderId, cascadeDelete: true)
                .Index(t => t.WorkOrderId)
                .Index(t => t.CommentorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkOrderComments", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkOrderComments", "CommentorId", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transactions", "WorkOrderId", "dbo.WorkOrders");
            DropForeignKey("dbo.WorkOrders", "UnitId", "dbo.Units");
            DropForeignKey("dbo.WorkOrders", "StatusId", "dbo.WorkOrderStatus");
            DropForeignKey("dbo.WorkOrders", "RequestorId", "dbo.AppUsers");
            DropForeignKey("dbo.WorkOrders", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.WorkOrders", "CategoryId", "dbo.WorkOrderCategories");
            DropForeignKey("dbo.WorkOrders", "AssignedUserId", "dbo.AppUsers");
            DropForeignKey("dbo.Transactions", "UnitId", "dbo.Units");
            DropForeignKey("dbo.Transactions", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Transactions", "LeaseHolderId", "dbo.AppUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AppUsers", "UnitId", "dbo.Units");
            DropForeignKey("dbo.Units", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.AppUsers", "LocationId", "dbo.Locations");
            DropIndex("dbo.WorkOrderComments", new[] { "CommentorId" });
            DropIndex("dbo.WorkOrderComments", new[] { "WorkOrderId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.WorkOrders", new[] { "AssignedUserId" });
            DropIndex("dbo.WorkOrders", new[] { "UnitId" });
            DropIndex("dbo.WorkOrders", new[] { "LocationId" });
            DropIndex("dbo.WorkOrders", new[] { "RequestorId" });
            DropIndex("dbo.WorkOrders", new[] { "StatusId" });
            DropIndex("dbo.WorkOrders", new[] { "CategoryId" });
            DropIndex("dbo.Transactions", new[] { "UnitId" });
            DropIndex("dbo.Transactions", new[] { "LocationId" });
            DropIndex("dbo.Transactions", new[] { "LeaseHolderId" });
            DropIndex("dbo.Transactions", new[] { "WorkOrderId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Units", new[] { "LocationId" });
            DropIndex("dbo.AppUsers", new[] { "UnitId" });
            DropIndex("dbo.AppUsers", new[] { "LocationId" });
            DropTable("dbo.WorkOrderComments");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.WorkOrderStatus");
            DropTable("dbo.WorkOrderCategories");
            DropTable("dbo.WorkOrders");
            DropTable("dbo.Transactions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.EMails");
            DropTable("dbo.Units");
            DropTable("dbo.Locations");
            DropTable("dbo.AppUsers");
        }
    }
}
