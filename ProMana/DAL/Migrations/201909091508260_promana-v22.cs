namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoleInProject", "JoinDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoleInProject", "JoinDate");
        }
    }
}
