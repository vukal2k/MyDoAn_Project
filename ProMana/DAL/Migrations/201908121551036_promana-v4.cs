namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskType", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskType", "IsActive", c => c.Boolean());
        }
    }
}
