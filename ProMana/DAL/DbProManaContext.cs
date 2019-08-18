using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DTO;
using System.Collections.Generic;
using COMMON;

namespace DAL
{
    public partial class DbProManaContext : DbContext
    {
        public DbProManaContext()
            : base("name=DbProManaContext")
        {
            Database.SetInitializer(new Initialaze());
            //IEnumerable<LookupStatus> listStatus = new List<LookupStatus>
            //{
            //    new LookupStatus
            //    {
            //        Id=ProjectStatusKey.Opened,
            //        Name=ProjectStatus.Opened,
            //        IsActive=true,
            //        IsProject=true,
            //        IsTask=true
            //    },
            //    new LookupStatus
            //    {
            //        Id=ProjectStatusKey.Closed,
            //        Name=ProjectStatus.Closed,
            //        IsActive=true,
            //        IsProject=true,
            //        IsTask=true
            //    },
            //    new LookupStatus
            //    {
            //        Id=TaskStatusKey.InProgress,
            //        Name=COMMON.TaskStatus.InProgress,
            //        IsActive=true,
            //        IsProject=false,
            //        IsTask=true
            //    },
            //    new LookupStatus
            //    {
            //        Id=TaskStatusKey.Resolved,
            //        Name=COMMON.TaskStatus.Resolved,
            //        IsActive=true,
            //        IsProject=false,
            //        IsTask=true
            //    },
            //    new LookupStatus
            //    {
            //        Id=TaskStatusKey.ReOpened,
            //        Name=COMMON.TaskStatus.ReOpened,
            //        IsActive=true,
            //        IsProject=false,
            //        IsTask=true
            //    },
            //    new LookupStatus
            //    {
            //        Id=RequestStatusKey.Draft,
            //        Name=RequestStatus.Draft,
            //        IsActive=true,
            //        IsProject=false,
            //        IsTask=false
            //    },
            //    new LookupStatus
            //    {
            //        Id=RequestStatusKey.PendingApproval,
            //        Name=RequestStatus.PendingApproval,
            //        IsActive=true,
            //        IsProject=false,
            //        IsTask=false
            //    },
            //    new LookupStatus
            //    {
            //        Id=RequestStatusKey.Approved,
            //        Name=RequestStatus.Approved,
            //        IsActive=true,
            //        IsProject=false,
            //        IsTask=false
            //    },
            //    new LookupStatus
            //    {
            //        Id=RequestStatusKey.Rejected,
            //        Name=RequestStatus.Rejected,
            //        IsActive=true,
            //        IsProject=false,
            //        IsTask=false
            //    },
            //    new LookupStatus
            //    {
            //        Id=RequestStatusKey.Cancelled,
            //        Name=RequestStatus.Cancelled,
            //        IsActive=true,
            //        IsProject=false,
            //        IsTask=false
            //    },
            //};
            //LookupStatus.AddRange(listStatus);
            //SaveChanges();
        }
        public virtual DbSet<JobRole> JobRoles { get; set; }
        public virtual DbSet<LookupStatus> LookupStatus { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectLog> ProjectLogs { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<ResolveType> ResolveTypes { get; set; }
        public virtual DbSet<RoleInProject> RoleInProjects { get; set; }
        public virtual DbSet<Solution> Solutions { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskType> TaskTypes { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }
        public virtual DbSet<LookupSeverity> LookupSeverities { get; set; }
        public virtual DbSet<LookupPriority> LookupPriorities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobRole>()
                .HasMany(e => e.RoleInProjects)
                .WithRequired(e => e.JobRole)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LookupStatus>()
                .Property(c => c.Id).HasDatabaseGeneratedOption(null);

            modelBuilder.Entity<LookupStatus>()
                .HasMany(e => e.Projects)
                .WithRequired(e => e.LookupStatu)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LookupStatus>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.LookupStatu)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LookupStatus>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.LookupStatus)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.TeamLead)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Modules)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.RoleInProjects)
                .WithRequired(e => e.Module)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProjectLog>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Request>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Request>()
                .Property(e => e.ApprovalBy)
                .IsUnicode(false);

            modelBuilder.Entity<ResolveType>()
                .HasMany(e => e.Solutions)
                .WithOptional(e => e.ResolveType1)
                .HasForeignKey(e => e.ResolveType);
            

            modelBuilder.Entity<LookupSeverity>()
                .HasMany(e => e.Tasks)
                .WithOptional(e => e.LookupSeverity)
                .HasForeignKey(e => e.Severity);

            modelBuilder.Entity<LookupPriority>()
                .HasMany(e => e.Tasks)
                .WithOptional(e => e.LookupPriority)
                .HasForeignKey(e => e.Priority);

            modelBuilder.Entity<RoleInProject>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Solution>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Task>()
                .Property(e => e.AssignedTo)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.FullName)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.CurrentJob)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.Company)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.TimeUnit)
                .IsUnicode(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.Modules)
                .WithOptional(e => e.UserInfo)
                .HasForeignKey(e => e.TeamLead);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.Projects)
                .WithRequired(e => e.UserInfo)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.ProjectLogs)
                .WithRequired(e => e.UserInfo)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.Requests)
                .WithOptional(e => e.UserInfo)
                .HasForeignKey(e => e.ApprovalBy);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.Requests1)
                .WithRequired(e => e.UserInfo1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.RoleInProjects)
                .WithRequired(e => e.UserInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.Solutions)
                .WithRequired(e => e.UserInfo)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.Tasks)
                .WithRequired(e => e.UserInfo)
                .HasForeignKey(e => e.AssignedTo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .HasMany(e => e.Tasks1)
                .WithRequired(e => e.UserInfo1)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);
        }
    }
}
