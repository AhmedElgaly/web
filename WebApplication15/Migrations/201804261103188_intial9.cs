namespace WebApplication15.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intial9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.booking", "busid", "dbo.Bus");
            DropIndex("dbo.booking", new[] { "busid" });
            AlterColumn("dbo.booking", "busid", c => c.Int());
            CreateIndex("dbo.booking", "busid");
            AddForeignKey("dbo.booking", "busid", "dbo.Bus", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.booking", "busid", "dbo.Bus");
            DropIndex("dbo.booking", new[] { "busid" });
            AlterColumn("dbo.booking", "busid", c => c.Int(nullable: false));
            CreateIndex("dbo.booking", "busid");
            AddForeignKey("dbo.booking", "busid", "dbo.Bus", "ID", cascadeDelete: true);
        }
    }
}
