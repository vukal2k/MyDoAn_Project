namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobRole", "CanDelete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobRole", "CanDelete");
        }
    }
}
