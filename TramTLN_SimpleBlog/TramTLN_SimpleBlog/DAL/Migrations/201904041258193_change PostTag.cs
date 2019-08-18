namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePostTag : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TagPost", name: "TagId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.TagPost", name: "PostId", newName: "TagId");
            RenameColumn(table: "dbo.TagPost", name: "__mig_tmp__0", newName: "PostId");
            RenameIndex(table: "dbo.TagPost", name: "IX_TagId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.TagPost", name: "IX_PostId", newName: "IX_TagId");
            RenameIndex(table: "dbo.TagPost", name: "__mig_tmp__0", newName: "IX_PostId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.TagPost", name: "IX_PostId", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.TagPost", name: "IX_TagId", newName: "IX_PostId");
            RenameIndex(table: "dbo.TagPost", name: "__mig_tmp__0", newName: "IX_TagId");
            RenameColumn(table: "dbo.TagPost", name: "PostId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.TagPost", name: "TagId", newName: "PostId");
            RenameColumn(table: "dbo.TagPost", name: "__mig_tmp__0", newName: "TagId");
        }
    }
}
