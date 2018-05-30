namespace WebApplication15.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bus",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Driver_name = c.String(nullable: false),
                        color = c.String(nullable: false),
                        Capacity = c.Int(nullable: false),
                        Num_Chairs = c.Int(nullable: false),
                        tripid = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.trips", t => t.tripid, cascadeDelete: true)
                .Index(t => t.tripid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bus", "tripid", "dbo.trips");
            DropIndex("dbo.Bus", new[] { "tripid" });
            DropTable("dbo.Bus");
        }
    }
}
