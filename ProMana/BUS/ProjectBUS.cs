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

                //insert member
                project.RoleInProjects = new List<RoleInProject> { new RoleInProject
                {
                    IsActive = true,
                    RoleId = HardFixJobRole.PM,
                    UserName = project.CreatedBy,
                } };

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
                var owner = await _unitOfWork.RoleInProjects.Get(o => o.ProjectId==project.Id 
                                                                    && o.UserName.Equals(project.CreatedBy) 
                                                                    && o.IsActive && o.RoleId==HardFixJobRole.PM);
                var oldOwner = await _unitOfWork.RoleInProjects.Get(o => o.IsActive && o.RoleId == HardFixJobRole.PM && o.ProjectId == project.Id);
                if (owner.FirstOrDefault()!=null)
                {
                    await _unitOfWork.RoleInProjects.Delete(oldOwner.FirstOrDefault().Id);
                    _unitOfWork.RoleInProjects.Insert(new RoleInProject {
                        IsActive = true,
                        RoleId = HardFixJobRole.PM,
                        UserName = project.CreatedBy,
                        ProjectId=project.Id
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

                //insert member
                project.RoleInProjects = new List<RoleInProject> { new RoleInProject
                {
                    IsActive = true,
                    RoleId = HardFixJobRole.PM,
                    UserName = userCreate,
                } };

                foreach (var member in members)
                {
                    project.RoleInProjects.Add(new RoleInProject
                    {
                        IsActive = true,
                        RoleId = member.RoleId,
                        UserName = member.Username
                    });
                }
            
                _unitOfWork.Projects.Insert(project);


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

        public async Task<Project> Update(Project project, IEnumerable<MemberParamsViewModel> members, List<string> errors)
        {
            try
            {
                if (await Validate(project, errors) == false)
                {
                    return project;
                }
                //delete old member
                var oldMembers = await _unitOfWork.RoleInProjects.Get(o => o.ProjectId == project.Id);
                foreach (var item in oldMembers)
                {
                    await _unitOfWork.RoleInProjects.Delete(item.Id);
                }
                //insert member
                List<RoleInProject> newListMember = new List<RoleInProject>();

                var member = new RoleInProject
                {
                    IsActive = true,
                    RoleId = HardFixJobRole.PM,
                    UserName = project.CreatedBy,
                    ProjectId = project.Id
                };
                 _unitOfWork.RoleInProjects.Insert(member);
                newListMember.Add(member);

                foreach (var item in members)
                {
                    member = new RoleInProject
                    {
                        IsActive = true,
                        RoleId = item.RoleId,
                        UserName = item.Username,
                        ProjectId = project.Id
                    };
                    newListMember.Add(member);
                    _unitOfWork.RoleInProjects.Insert(member);
                }

                var oldProject = await _unitOfWork.Projects.GetById(project.Id);
                oldProject.Name = project.Name;
                oldProject.Code = project.Code;
                oldProject.CreatedBy = project.CreatedBy;
                oldProject.From = project.From;
                oldProject.To = project.To;
                oldProject.Description = project.Description;
                _unitOfWork.Projects.Update(oldProject);


                //commit
                var result = await _unitOfWork.CommitAsync() > 0;
                if (result)
                {
                    oldProject.RoleInProjects = newListMember;
                }

                return oldProject;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return project;
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
