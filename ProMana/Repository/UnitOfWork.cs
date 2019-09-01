using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork
    {
        public UnitOfWork()
        {
            _dbContext = new DbProManaContext();
            JobRoles = new Repository<JobRole>(_dbContext); 
            Modules = new Repository<Module>(_dbContext);
            Projects = new Repository<Project>(_dbContext);
            ProjectLogs = new Repository<ProjectLog>(_dbContext);
            Requests = new Repository<Request>(_dbContext);
            RoleInProjects = new Repository<RoleInProject>(_dbContext);
            Tasks = new Repository<DTO.Task>(_dbContext);
            TaskTypes = new Repository<TaskType>(_dbContext);
            UserInfos = new Repository<UserInfo>(_dbContext);
            Solutions = new Repository<Solution>(_dbContext);
            ResolveTypes = new Repository<ResolveType>(_dbContext);
            LookupStatuss = new Repository<LookupStatus>(_dbContext);
        }

        #region Propeties
        DbContext _dbContext;
        public Repository<JobRole> JobRoles { get; set; }
        public Repository<Module> Modules { get; set; }
        public Repository<Project> Projects { get; set; }
        public Repository<ProjectLog> ProjectLogs { get; set; }
        public Repository<Request> Requests { get; set; }
        public Repository<RoleInProject> RoleInProjects { get; set; }
        public Repository<DTO.Task> Tasks { get; set; }
        public Repository<TaskType> TaskTypes { get; set; }
        public Repository<UserInfo> UserInfos { get; set; }
        public Repository<Solution> Solutions { get; set; }
        public Repository<ResolveType> ResolveTypes { get; set; }
        public Repository<LookupStatus> LookupStatuss { get; set; }
        #endregion

        #region Methods

        /// <summary>
        ///     Save changes into database.
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        /// <summary>
        ///     Save changes into database asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        ///     Begin transaction scope.
        /// </summary>
        /// <returns></returns>
        public DbContextTransaction BeginTransactionScope()
        {
            return _dbContext.Database.BeginTransaction();
        }

        /// <summary>
        ///     Begin transaction scope.
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public DbContextTransaction BeginTransactionScope(IsolationLevel isolationLevel)
        {
            return _dbContext.Database.BeginTransaction(isolationLevel);
        }

        #endregion
    }
}
