namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskType", "Description", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskType", "Description");
        }
    }
}
