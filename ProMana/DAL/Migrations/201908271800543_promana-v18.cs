namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav18 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "Description", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.UserInfo", "FullName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.UserInfo", "CurrentJob", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.UserInfo", "Company", c => c.String(maxLength: 200));
            AlterColumn("dbo.UserInfo", "TimeUnit", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInfo", "TimeUnit", c => c.String(maxLength: 10, unicode: false));
            AlterColumn("dbo.UserInfo", "Company", c => c.String(maxLength: 200, unicode: false));
            AlterColumn("dbo.UserInfo", "CurrentJob", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.UserInfo", "FullName", c => c.String(nullable: false, maxLength: 200, unicode: false));
            AlterColumn("dbo.Project", "Description", c => c.String(maxLength: 300));
        }
    }
}
