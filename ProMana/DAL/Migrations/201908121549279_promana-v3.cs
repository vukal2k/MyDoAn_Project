namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResolveType", "IsActive", c => c.Boolean());
            AddColumn("dbo.TaskType", "IsActive", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskType", "IsActive");
            DropColumn("dbo.ResolveType", "IsActive");
        }
    }
}
