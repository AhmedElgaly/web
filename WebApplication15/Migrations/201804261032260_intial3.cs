namespace WebApplication15.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.trips",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        distance = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        from = c.String(nullable: false),
                        to = c.String(nullable: false),
                        price = c.Int(nullable: false),
                        duration = c.Int(nullable: false),
                        num_buses = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.trips");
        }
    }
}
