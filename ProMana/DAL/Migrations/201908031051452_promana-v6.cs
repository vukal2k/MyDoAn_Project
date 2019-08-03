namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Project", "StatusId", "dbo.LookupStatus");
            DropForeignKey("dbo.Request", "StatusId", "dbo.LookupStatus");
            DropForeignKey("dbo.Task", "StatusId", "dbo.LookupStatus");
            DropPrimaryKey("dbo.LookupStatus");
            AlterColumn("dbo.LookupStatus", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.LookupStatus", "Id");
            AddForeignKey("dbo.Project", "StatusId", "dbo.LookupStatus", "Id");
            AddForeignKey("dbo.Request", "StatusId", "dbo.LookupStatus", "Id");
            AddForeignKey("dbo.Task", "StatusId", "dbo.LookupStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Task", "StatusId", "dbo.LookupStatus");
            DropForeignKey("dbo.Request", "StatusId", "dbo.LookupStatus");
            DropForeignKey("dbo.Project", "StatusId", "dbo.LookupStatus");
            DropPrimaryKey("dbo.LookupStatus");
            AlterColumn("dbo.LookupStatus", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.LookupStatus", "Id");
            AddForeignKey("dbo.Task", "StatusId", "dbo.LookupStatus", "Id");
            AddForeignKey("dbo.Request", "StatusId", "dbo.LookupStatus", "Id");
            AddForeignKey("dbo.Project", "StatusId", "dbo.LookupStatus", "Id");
        }
    }
}
