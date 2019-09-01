namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Project", "Description", c => c.String(maxLength: 300));
            AlterColumn("dbo.ProjectLog", "Content", c => c.String(maxLength: 3000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectLog", "Content", c => c.Int(nullable: false));
            AlterColumn("dbo.Project", "Description", c => c.String(storeType: "ntext"));
        }
    }
}
