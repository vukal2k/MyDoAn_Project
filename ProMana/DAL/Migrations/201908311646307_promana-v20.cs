namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav20 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectLog", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectLog", "CreatedDate", c => c.Int(nullable: false));
        }
    }
}
