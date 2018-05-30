namespace WebApplication15.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bill",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        cost = c.Int(),
                        from = c.String(),
                        to = c.String(),
                        type = c.String(),
                        time = c.DateTime(),
                        busid = c.Int(),
                        personid = c.Int(),
                        t_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Bus", t => t.busid)
                .ForeignKey("dbo.trips", t => t.t_id)
                .ForeignKey("dbo.Users", t => t.personid)
                .Index(t => t.busid)
                .Index(t => t.personid)
                .Index(t => t.t_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bill", "personid", "dbo.Users");
            DropForeignKey("dbo.Bill", "t_id", "dbo.trips");
            DropForeignKey("dbo.Bill", "busid", "dbo.Bus");
            DropIndex("dbo.Bill", new[] { "t_id" });
            DropIndex("dbo.Bill", new[] { "personid" });
            DropIndex("dbo.Bill", new[] { "busid" });
            DropTable("dbo.Bill");
        }
    }
}
