namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LookupStatus", "Name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.LookupStatus", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LookupStatus", "Title", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.LookupStatus", "Name");
        }
    }
}
