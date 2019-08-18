namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class promanav12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LookupPriority",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.LookupSeverity",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Name);
            
            AlterColumn("dbo.Task", "Severity", c => c.String(maxLength: 50));
            AlterColumn("dbo.Task", "Priority", c => c.String(maxLength: 50));
            CreateIndex("dbo.Task", "Severity");
            CreateIndex("dbo.Task", "Priority");
            AddForeignKey("dbo.Task", "Priority", "dbo.LookupPriority", "Name");
            AddForeignKey("dbo.Task", "Severity", "dbo.LookupSeverity", "Name");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Task", "Severity", "dbo.LookupSeverity");
            DropForeignKey("dbo.Task", "Priority", "dbo.LookupPriority");
            DropIndex("dbo.Task", new[] { "Priority" });
            DropIndex("dbo.Task", new[] { "Severity" });
            AlterColumn("dbo.Task", "Priority", c => c.String());
            AlterColumn("dbo.Task", "Severity", c => c.String());
            DropTable("dbo.LookupSeverity");
            DropTable("dbo.LookupPriority");
        }
    }
}
