namespace WebApplication15.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.booking",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumOFChair = c.Int(nullable: false),
                        booked = c.Boolean(),
                        payment = c.Boolean(nullable: false),
                        cost = c.Int(nullable: false),
                        busid = c.Int(nullable: false),
                        personid = c.Int(nullable: false),
                        time = c.DateTime(nullable: false),
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
            DropForeignKey("dbo.booking", "personid", "dbo.Users");
            DropForeignKey("dbo.booking", "t_id", "dbo.trips");
            DropForeignKey("dbo.booking", "busid", "dbo.Bus");
            DropIndex("dbo.booking", new[] { "t_id" });
            DropIndex("dbo.booking", new[] { "personid" });
            DropIndex("dbo.booking", new[] { "busid" });
            DropTable("dbo.booking");
        }
    }
}
