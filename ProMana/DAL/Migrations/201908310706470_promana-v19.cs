namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInfo", "Email", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInfo", "Email");
        }
    }
}
