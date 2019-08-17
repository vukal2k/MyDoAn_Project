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
                _unitOfWork.Projects.Update(project);
                
                //commit
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
            //SqlParameter[] prams =
            //{
            // new SqlParameter { ParameterName = "@projectId", Value = projectId , DbType = DbType.Int32 }
            //};

            //var result = await _unitOfWork.UserInfos.Get(StoreProcedure.GetUserDoNotInProject, prams);
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
        #endregion
    }
}
