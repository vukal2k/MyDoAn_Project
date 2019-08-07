namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Solution", new[] { "ResolveType1_Id" });
            DropColumn("dbo.Solution", "ResolveType");
            RenameColumn(table: "dbo.Solution", name: "ResolveType1_Id", newName: "ResolveType");
            AlterColumn("dbo.Solution", "ResolveType", c => c.Int());
            CreateIndex("dbo.Solution", "ResolveType");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Solution", new[] { "ResolveType" });
            AlterColumn("dbo.Solution", "ResolveType", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Solution", name: "ResolveType", newName: "ResolveType1_Id");
            AddColumn("dbo.Solution", "ResolveType", c => c.Int(nullable: false));
            CreateIndex("dbo.Solution", "ResolveType1_Id");
        }
    }
}
