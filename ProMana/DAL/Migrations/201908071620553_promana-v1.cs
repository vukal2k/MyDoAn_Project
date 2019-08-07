namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Solution", "ResolveType1_Id", "dbo.ResolveType");
            DropIndex("dbo.Request", new[] { "CreatedBy" });
            DropIndex("dbo.Request", new[] { "ApprovalBy" });
            DropIndex("dbo.Module", new[] { "TeamLead" });
            RenameColumn(table: "dbo.Request", name: "CreatedBy", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Request", name: "ApprovalBy", newName: "CreatedBy");
            RenameColumn(table: "dbo.Task", name: "CreatedBy", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.Task", name: "AssignedTo", newName: "CreatedBy");
            RenameColumn(table: "dbo.Request", name: "__mig_tmp__0", newName: "ApprovalBy");
            RenameColumn(table: "dbo.Task", name: "__mig_tmp__1", newName: "AssignedTo");
            RenameIndex(table: "dbo.Task", name: "IX_AssignedTo", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.Task", name: "IX_CreatedBy", newName: "IX_AssignedTo");
            RenameIndex(table: "dbo.Task", name: "__mig_tmp__0", newName: "IX_CreatedBy");
            DropPrimaryKey("dbo.RoleInProject");
            DropPrimaryKey("dbo.ResolveType");
            AlterColumn("dbo.RoleInProject", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Request", "ApprovalBy", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Request", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Module", "TeamLead", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Solution", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.ResolveType", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.RoleInProject", "Id");
            AddPrimaryKey("dbo.ResolveType", "Id");
            CreateIndex("dbo.Request", "CreatedBy");
            CreateIndex("dbo.Request", "ApprovalBy");
            CreateIndex("dbo.Module", "TeamLead");
            CreateIndex("dbo.Solution", "CreatedBy");
            AddForeignKey("dbo.Solution", "CreatedBy", "dbo.UserInfo", "UserName");
            AddForeignKey("dbo.Solution", "ResolveType1_Id", "dbo.ResolveType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Solution", "ResolveType1_Id", "dbo.ResolveType");
            DropForeignKey("dbo.Solution", "CreatedBy", "dbo.UserInfo");
            DropIndex("dbo.Solution", new[] { "CreatedBy" });
            DropIndex("dbo.Module", new[] { "TeamLead" });
            DropIndex("dbo.Request", new[] { "ApprovalBy" });
            DropIndex("dbo.Request", new[] { "CreatedBy" });
            DropPrimaryKey("dbo.ResolveType");
            DropPrimaryKey("dbo.RoleInProject");
            AlterColumn("dbo.ResolveType", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Solution", "CreatedBy", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Module", "TeamLead", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Request", "CreatedBy", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Request", "ApprovalBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.RoleInProject", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.ResolveType", "Id");
            AddPrimaryKey("dbo.RoleInProject", "Id");
            RenameIndex(table: "dbo.Task", name: "IX_CreatedBy", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.Task", name: "IX_AssignedTo", newName: "IX_CreatedBy");
            RenameIndex(table: "dbo.Task", name: "__mig_tmp__0", newName: "IX_AssignedTo");
            RenameColumn(table: "dbo.Task", name: "AssignedTo", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.Request", name: "ApprovalBy", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Task", name: "CreatedBy", newName: "AssignedTo");
            RenameColumn(table: "dbo.Task", name: "__mig_tmp__1", newName: "CreatedBy");
            RenameColumn(table: "dbo.Request", name: "CreatedBy", newName: "ApprovalBy");
            RenameColumn(table: "dbo.Request", name: "__mig_tmp__0", newName: "CreatedBy");
            CreateIndex("dbo.Module", "TeamLead");
            CreateIndex("dbo.Request", "ApprovalBy");
            CreateIndex("dbo.Request", "CreatedBy");
            AddForeignKey("dbo.Solution", "ResolveType1_Id", "dbo.ResolveType", "Id");
        }
    }
}
