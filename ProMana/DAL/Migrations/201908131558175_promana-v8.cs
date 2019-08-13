namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav8 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.RoleInProject");
            AlterColumn("dbo.RoleInProject", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.RoleInProject", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.RoleInProject");
            AlterColumn("dbo.RoleInProject", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.RoleInProject", "Id");
        }
    }
}
