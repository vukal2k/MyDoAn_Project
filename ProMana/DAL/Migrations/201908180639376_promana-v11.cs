namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Task", "Severity", c => c.String());
            AddColumn("dbo.Task", "Priority", c => c.String());
            DropColumn("dbo.Task", "Level");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Task", "Level", c => c.Int(nullable: false));
            DropColumn("dbo.Task", "Priority");
            DropColumn("dbo.Task", "Severity");
        }
    }
}
