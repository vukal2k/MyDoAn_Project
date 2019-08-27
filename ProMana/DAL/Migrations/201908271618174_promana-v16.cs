namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav16 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserInfo", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserInfo", "PhoneNumber", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
