namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Description", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "Description");
        }
    }
}
