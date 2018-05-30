namespace WebApplication15.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Block",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        busid = c.Int(nullable: false),
                        personid = c.Int(nullable: false),
                        t_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bus", t => t.busid, cascadeDelete: true)
                .ForeignKey("dbo.trips", t => t.t_id)
                .ForeignKey("dbo.Users", t => t.personid, cascadeDelete: true)
                .Index(t => t.busid)
                .Index(t => t.personid)
                .Index(t => t.t_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Block", "personid", "dbo.Users");
            DropForeignKey("dbo.Block", "t_id", "dbo.trips");
            DropForeignKey("dbo.Block", "busid", "dbo.Bus");
            DropIndex("dbo.Block", new[] { "t_id" });
            DropIndex("dbo.Block", new[] { "personid" });
            DropIndex("dbo.Block", new[] { "busid" });
            DropTable("dbo.Block");
        }
    }
}
