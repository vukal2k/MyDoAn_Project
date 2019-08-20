using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DTO;
using Repository;
using COMMON;
using System.Data.SqlClient;
using System.Data;

namespace BUS
{
    public class ProjectBUS
    {
        private UnitOfWork _unitOfWork;

        public ProjectBUS()
        {
            _unitOfWork = new UnitOfWork();
        }

        #region Admin
        public async Task<IEnumerable<Project>> GetAll()
        {
            return await _unitOfWork.Projects.Get(j => j.IsActive == true);
        }

        public async Task<bool> Insert(Project project, List<string> errors)
        {
            try
            {
                project.IsActive = true;
                project.StatusId = ProjectStatusKey.Opened;
                project.CreatedDate = DateTime.Now;

                _unitOfWork.Projects.Insert(project);


                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(Project project, List<string> errors)
        {
            try
            {
                project.IsActive = true;
                var owner = await _unitOfWork.RoleInProjects.Get(o => o.ModuleId==project.Id 
                                                                    && o.UserName.Equals(project.CreatedBy) 
                                                                    && o.IsActive && o.RoleId==HardFixJobRole.PM);
                var oldOwner = await _unitOfWork.RoleInProjects.Get(o => o.IsActive && o.RoleId == HardFixJobRole.PM && o.ModuleId == project.Id);
                if (owner.FirstOrDefault()!=null)
                {
                    await _unitOfWork.RoleInProjects.Delete(oldOwner.FirstOrDefault().Id);
                    _unitOfWork.RoleInProjects.Insert(new RoleInProject {
                        IsActive = true,
                        RoleId = HardFixJobRole.PM,
                        UserName = project.CreatedBy,
                        ModuleId=project.Id
                    });
                }
                _unitOfWork.Projects.Update(project);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int projectId, List<string> errors)
        {
            try
            {
                var project = await _unitOfWork.Projects.GetById(projectId);
                project.IsActive = false;
                _unitOfWork.Projects.Update(project);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }
        #endregion

        #region Public method
        public async Task<IEnumerable<Project>> GetMyProject(string username)
        {
            SqlParameter[] prams =
            {
             new SqlParameter { ParameterName = "@username", Value = username , DbType = DbType.String }
            };

            var result = await _unitOfWork.Projects.Get(StoreProcedure.GetMyProject, prams);
            //var result = await _unitOfWork.UserInfos.Get(u => u.IsActive);
            return result;
        }
        public async Task<bool> Create(Project project,IEnumerable<MemberParamsViewModel>members,string userCreate, List<string>errors)
        {
            try
            {
                if (await Validate(project, errors) == false)
                {
                    return false;
                }
                //insert project
                project.CreatedBy = userCreate;
                project.StatusId = ProjectStatusKey.Opened;
                project.CreatedDate = DateTime.Now;


                //insert watcher
                var listMember = members.Select(m => new RoleInProject { IsActive = true, RoleId = HardFixJobRole.Watcher, UserName = m.Username }).ToList();
                listMember.Add(new RoleInProject {
                    IsActive = true,
                    RoleId = HardFixJobRole.PM,
                    UserName = userCreate
                });
                project.Modules = new List<Module>()
                {
                    new Module
                    {
                        IsActive=true,
                        Title=HardFixJobRoleTitle.Watcher,
                        RoleInProjects= listMember
                    }
                };


                //commit
                _unitOfWork.Projects.Insert(project);
                var result = await _unitOfWork.CommitAsync() > 0;
                
                return result;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(Project project, IEnumerable<MemberParamsViewModel> members, List<string> errors)
        {
            try
            {
                if (await Validate(project, errors) == false)
                {
                    return false;
                }

                //delete old member
                var module = await _unitOfWork.Modules.GetOne(m => m.ProjectId == project.Id && m.Title.Equals(HardFixJobRoleTitle.Watcher));
                var watchers = await _unitOfWork.RoleInProjects.Get(m => m.IsActive && m.ModuleId == module.Id);
                foreach (var item in watchers)
                {
                    await _unitOfWork.RoleInProjects.Delete(item);
                }
                
                //insert new member
                var listMember = members.Select(m => new RoleInProject { IsActive = true, RoleId = m.RoleId, UserName = m.Username, ModuleId = module.Id }).ToList();
                foreach (var item in listMember)
                {
                    _unitOfWork.RoleInProjects.Insert(item);
                }

                _unitOfWork.Projects.Update(project);

                //commit
                project.StatusId = ProjectStatusKey.Opened;
                project.IsActive = true;
                var result = await _unitOfWork.CommitAsync() > 0;

                return result;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<Project> GetById (int projectId)
        {
            var project = await _unitOfWork.Projects.GetById(projectId);
            return project;
        }

        public async Task<IEnumerable<UserInfo>>GetUserDoNotInProject(int projectId)
        {
            SqlParameter[] prams =
            {
             new SqlParameter { ParameterName = "@projectId", Value = projectId , DbType = DbType.Int32 }
            };

            var result = await _unitOfWork.UserInfos.Get(StoreProcedure.GetUserDoNotInProject, prams);
            //var result = await _unitOfWork.UserInfos.Get(u => u.IsActive);
            return result;
        }

        public async Task<IEnumerable<UserInfo>> GetUsercCanNotWatcher(int projectId)
        {
            var result = await _unitOfWork.UserInfos.Get(u => u.IsActive);
            return result;
        }

        public async Task<IEnumerable<UserInfo>> GetUserNotWatcher(int projectId)
        { 
            var watchers = await _unitOfWork.RoleInProjects.Get(u => u.IsActive && u.Module.Title.Equals(HardFixJobRoleTitle.Watcher));
            var watcherIds = watchers.Select(u => u.UserName);
            var result = await _unitOfWork.UserInfos.Get(u => u.IsActive && !watcherIds.Contains(u.UserName));
            return result;
        }

        public async Task<GanttChartViewModel> GetGanttChart(int projectId, int page)
        {
            GanttChartViewModel result = new GanttChartViewModel()
            {
                ListDay = new List<DateTime>(),
                Page = page,
                ListTask = new List<TaskViewModel>()
            };

            var currentDate = DateTime.Today;
            var startDate = StartOfWeek(currentDate, DayOfWeek.Sunday).AddDays(page*7);
            var endDate = StartOfWeek(currentDate, DayOfWeek.Saturday).AddDays(page * 7);

            //insert list date
            DateTime dateTemp = startDate;
            while (dateTemp<=endDate)
            {
                result.ListDay.Add(dateTemp);
                dateTemp = dateTemp.AddDays(1);
            }

            //insert list task
            var project = await _unitOfWork.Projects.GetById(projectId);
            result.Project = project;
            var taskDateTo = DateTime.MinValue;
            var taskDateFrom = DateTime.MinValue;
            foreach (var module in project.Modules.Where(m => m.IsActive))
            {
                foreach (var task in module.Tasks.Where(m => m.IsActive && m.IsTask))
                {
                    taskDateTo = new DateTime(task.To.Year, task.To.Month, task.To.Day);
                    taskDateFrom = new DateTime(task.From.Year, task.From.Month, task.From.Day);
                    if (taskDateTo>=startDate && taskDateTo<=endDate)
                    {
                        result.ListTask.Add(new TaskViewModel {
                            Task=task,
                            FromIndex= taskDateFrom.Subtract(startDate).Days > 0 ? taskDateFrom.Subtract(startDate).Days+1 : 1,
                            ToIndex = 8-endDate.Subtract(taskDateTo).Days,
                        });
                    }
                    else if (taskDateFrom >= startDate && taskDateFrom <= endDate)
                    {
                        result.ListTask.Add(new TaskViewModel
                        {
                            Task = task,
                            FromIndex = taskDateFrom.Subtract(startDate).Days+1,
                            ToIndex = endDate.Subtract(startDate).Days > 0 ? 8-endDate.Subtract(startDate).Days : 8,
                        });
                    }
                    else if(taskDateFrom < startDate && taskDateTo > endDate)
                    {
                        result.ListTask.Add(new TaskViewModel
                        {
                            Task = task,
                            FromIndex = 1,
                            ToIndex = 8,
                        });
                    }
                }
            }

            return result;
        }
        #endregion

        #region private method
        private async Task<bool> Validate (Project project, List<string> errors)
        {
            var projectCodeExist = await _unitOfWork.Projects.Get(p => p.Code.Equals(project.Code) && project.Id!=p.Id);
            bool isCodeExist = projectCodeExist.Any();
            if (isCodeExist)
            {
                errors.Add("Project code is exists");
            }

            if (errors.Any())
            {
                return false;
            }
            return true;
        }

        private DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            //int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            //return dt.AddDays(-1 * diff).Date;
            return dt.AddDays(startOfWeek - dt.DayOfWeek);
        }
        #endregion
    }
}
