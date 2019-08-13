namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResolveType", "Description", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResolveType", "Description");
        }
    }
}
