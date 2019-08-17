namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Task", "TaskTypeId", "dbo.TaskType");
            DropForeignKey("dbo.RoleInProject", "ProjectId", "dbo.Project");
            DropIndex("dbo.RoleInProject", new[] { "ProjectId" });
            DropIndex("dbo.Task", new[] { "TaskTypeId" });
            AddColumn("dbo.RoleInProject", "ModuleId", c => c.Int(nullable: false));
            AddColumn("dbo.Task", "TaskType", c => c.String());
            AddColumn("dbo.Task", "IsTask", c => c.Boolean(nullable: false));
            CreateIndex("dbo.RoleInProject", "ModuleId");
            AddForeignKey("dbo.RoleInProject", "ModuleId", "dbo.Module", "Id");
            DropColumn("dbo.RoleInProject", "ProjectId");
            DropColumn("dbo.Task", "TaskTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Task", "TaskTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.RoleInProject", "ProjectId", c => c.Int(nullable: false));
            DropForeignKey("dbo.RoleInProject", "ModuleId", "dbo.Module");
            DropIndex("dbo.RoleInProject", new[] { "ModuleId" });
            DropColumn("dbo.Task", "IsTask");
            DropColumn("dbo.Task", "TaskType");
            DropColumn("dbo.RoleInProject", "ModuleId");
            CreateIndex("dbo.Task", "TaskTypeId");
            CreateIndex("dbo.RoleInProject", "ProjectId");
            AddForeignKey("dbo.RoleInProject", "ProjectId", "dbo.Project", "Id");
            AddForeignKey("dbo.Task", "TaskTypeId", "dbo.TaskType", "Id");
        }
    }
}
