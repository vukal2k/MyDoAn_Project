namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ResolveType", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ResolveType", "IsActive", c => c.Boolean());
        }
    }
}
