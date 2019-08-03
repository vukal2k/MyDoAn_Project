namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobRole",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(storeType: "ntext"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleInProject",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.UserName)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .ForeignKey("dbo.JobRole", t => t.RoleId)
                .Index(t => t.ProjectId)
                .Index(t => t.RoleId)
                .Index(t => t.UserName);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        From = c.DateTime(nullable: false, storeType: "date"),
                        To = c.DateTime(storeType: "date"),
                        CreatedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDate = c.DateTime(nullable: false, storeType: "date"),
                        StatusId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LookupStatus", t => t.StatusId)
                .ForeignKey("dbo.UserInfo", t => t.CreatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.LookupStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        IsProject = c.Boolean(nullable: false),
                        IsTask = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(nullable: false, storeType: "ntext"),
                        ProjectId = c.Int(),
                        CreatedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ApprovalBy = c.String(maxLength: 50, unicode: false),
                        ApprovalDateTime = c.DateTime(),
                        StatusId = c.Int(nullable: false),
                        IsActive = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .ForeignKey("dbo.UserInfo", t => t.CreatedBy)
                .ForeignKey("dbo.UserInfo", t => t.ApprovalBy)
                .ForeignKey("dbo.LookupStatus", t => t.StatusId)
                .Index(t => t.ProjectId)
                .Index(t => t.CreatedBy)
                .Index(t => t.ApprovalBy)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.UserInfo",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 50, unicode: false),
                        FullName = c.String(nullable: false, maxLength: 200, unicode: false),
                        CurrentJob = c.String(nullable: false, maxLength: 200, unicode: false),
                        Company = c.String(maxLength: 200, unicode: false),
                        CountExperience = c.Double(),
                        TimeUnit = c.String(maxLength: 10, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserName);
            
            CreateTable(
                "dbo.Module",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        ProjectId = c.Int(nullable: false),
                        TeamLead = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfo", t => t.TeamLead)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .Index(t => t.ProjectId)
                .Index(t => t.TeamLead);
            
            CreateTable(
                "dbo.Task",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        CreatedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        AssignedTo = c.String(nullable: false, maxLength: 50, unicode: false),
                        TaskTypeId = c.Int(nullable: false),
                        ModuleId = c.Int(),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        Level = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        Description = c.String(storeType: "ntext"),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Module", t => t.ModuleId)
                .ForeignKey("dbo.TaskType", t => t.TaskTypeId)
                .ForeignKey("dbo.UserInfo", t => t.CreatedBy)
                .ForeignKey("dbo.UserInfo", t => t.AssignedTo)
                .ForeignKey("dbo.LookupStatus", t => t.StatusId)
                .Index(t => t.CreatedBy)
                .Index(t => t.AssignedTo)
                .Index(t => t.TaskTypeId)
                .Index(t => t.ModuleId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Solution",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskId = c.Int(nullable: false),
                        ResolveType = c.Int(nullable: false),
                        Reason = c.String(nullable: false, storeType: "ntext"),
                        Solution = c.String(nullable: false, storeType: "ntext"),
                        Description = c.String(storeType: "ntext"),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDateTime = c.DateTime(nullable: false),
                        ResolveType1_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResolveType", t => t.ResolveType1_Id)
                .ForeignKey("dbo.Task", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId)
                .Index(t => t.ResolveType1_Id);
            
            CreateTable(
                "dbo.ResolveType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectLog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.Int(nullable: false),
                        ProjectId = c.Int(),
                        CreatedBy = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedDate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Project", t => t.ProjectId)
                .ForeignKey("dbo.UserInfo", t => t.CreatedBy)
                .Index(t => t.ProjectId)
                .Index(t => t.CreatedBy);
            
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleInProject", "RoleId", "dbo.JobRole");
            DropForeignKey("dbo.RoleInProject", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Module", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Task", "StatusId", "dbo.LookupStatus");
            DropForeignKey("dbo.Request", "StatusId", "dbo.LookupStatus");
            DropForeignKey("dbo.Task", "AssignedTo", "dbo.UserInfo");
            DropForeignKey("dbo.Task", "CreatedBy", "dbo.UserInfo");
            DropForeignKey("dbo.RoleInProject", "UserName", "dbo.UserInfo");
            DropForeignKey("dbo.Request", "ApprovalBy", "dbo.UserInfo");
            DropForeignKey("dbo.Request", "CreatedBy", "dbo.UserInfo");
            DropForeignKey("dbo.Project", "CreatedBy", "dbo.UserInfo");
            DropForeignKey("dbo.ProjectLog", "CreatedBy", "dbo.UserInfo");
            DropForeignKey("dbo.ProjectLog", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Module", "TeamLead", "dbo.UserInfo");
            DropForeignKey("dbo.Task", "TaskTypeId", "dbo.TaskType");
            DropForeignKey("dbo.Solution", "TaskId", "dbo.Task");
            DropForeignKey("dbo.Solution", "ResolveType1_Id", "dbo.ResolveType");
            DropForeignKey("dbo.Task", "ModuleId", "dbo.Module");
            DropForeignKey("dbo.Request", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.Project", "StatusId", "dbo.LookupStatus");
            DropIndex("dbo.ProjectLog", new[] { "CreatedBy" });
            DropIndex("dbo.ProjectLog", new[] { "ProjectId" });
            DropIndex("dbo.Solution", new[] { "ResolveType1_Id" });
            DropIndex("dbo.Solution", new[] { "TaskId" });
            DropIndex("dbo.Task", new[] { "StatusId" });
            DropIndex("dbo.Task", new[] { "ModuleId" });
            DropIndex("dbo.Task", new[] { "TaskTypeId" });
            DropIndex("dbo.Task", new[] { "AssignedTo" });
            DropIndex("dbo.Task", new[] { "CreatedBy" });
            DropIndex("dbo.Module", new[] { "TeamLead" });
            DropIndex("dbo.Module", new[] { "ProjectId" });
            DropIndex("dbo.Request", new[] { "StatusId" });
            DropIndex("dbo.Request", new[] { "ApprovalBy" });
            DropIndex("dbo.Request", new[] { "CreatedBy" });
            DropIndex("dbo.Request", new[] { "ProjectId" });
            DropIndex("dbo.Project", new[] { "StatusId" });
            DropIndex("dbo.Project", new[] { "CreatedBy" });
            DropIndex("dbo.RoleInProject", new[] { "UserName" });
            DropIndex("dbo.RoleInProject", new[] { "RoleId" });
            DropIndex("dbo.RoleInProject", new[] { "ProjectId" });
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.ProjectLog");
            DropTable("dbo.TaskType");
            DropTable("dbo.ResolveType");
            DropTable("dbo.Solution");
            DropTable("dbo.Task");
            DropTable("dbo.Module");
            DropTable("dbo.UserInfo");
            DropTable("dbo.Request");
            DropTable("dbo.LookupStatus");
            DropTable("dbo.Project");
            DropTable("dbo.RoleInProject");
            DropTable("dbo.JobRole");
        }
    }
}
