namespace WebApplication15.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.contries",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        from = c.String(),
                        to = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.contries");
        }
    }
}
