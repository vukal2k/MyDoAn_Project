using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using DTO;
using COMMON;

namespace DAL
{
    public class Initialaze : CreateDatabaseIfNotExists<DbProManaContext>
    {
        protected override void Seed(DbProManaContext context)
        {
            context = new DbProManaContext();
            IEnumerable<LookupStatus> listStatus = new List<LookupStatus>
            {
                new LookupStatus
                {
                    Id=ProjectStatusKey.Opened,
                    Name=ProjectStatus.Opened,
                    IsActive=true,
                    IsProject=true,
                    IsTask=true
                },
                new LookupStatus
                {
                    Id=ProjectStatusKey.Closed,
                    Name=ProjectStatus.Closed,
                    IsActive=true,
                    IsProject=true,
                    IsTask=true
                },
                new LookupStatus
                {
                    Id=TaskStatusKey.InProgress,
                    Name=COMMON.TaskStatus.InProgress,
                    IsActive=true,
                    IsProject=false,
                    IsTask=true
                },
                new LookupStatus
                {
                    Id=TaskStatusKey.Resolved,
                    Name=COMMON.TaskStatus.Resolved,
                    IsActive=true,
                    IsProject=false,
                    IsTask=true
                },
                new LookupStatus
                {
                    Id=TaskStatusKey.ReOpened,
                    Name=COMMON.TaskStatus.ReOpened,
                    IsActive=true,
                    IsProject=false,
                    IsTask=true
                },
                new LookupStatus
                {
                    Id=RequestStatusKey.Draft,
                    Name=RequestStatus.Draft,
                    IsActive=true,
                    IsProject=false,
                    IsTask=false
                },
                new LookupStatus
                {
                    Id=RequestStatusKey.PendingApproval,
                    Name=RequestStatus.PendingApproval,
                    IsActive=true,
                    IsProject=false,
                    IsTask=false
                },
                new LookupStatus
                {
                    Id=RequestStatusKey.Approved,
                    Name=RequestStatus.Approved,
                    IsActive=true,
                    IsProject=false,
                    IsTask=false
                },
                new LookupStatus
                {
                    Id=RequestStatusKey.Rejected,
                    Name=RequestStatus.Rejected,
                    IsActive=true,
                    IsProject=false,
                    IsTask=false
                },
                new LookupStatus
                {
                    Id=RequestStatusKey.Cancelled,
                    Name=RequestStatus.Cancelled,
                    IsActive=true,
                    IsProject=false,
                    IsTask=false
                },
            };
            context.LookupStatus.AddRange(listStatus);
            context.SaveChanges();
            
            base.Seed(context);
        }
    }
}
