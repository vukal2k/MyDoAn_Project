using COMMON;
using DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ModuleBUS
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();
        private ProjectLogBUS _projectLog=new ProjectLogBUS();

        public async Task<IEnumerable<Module>>GetByProject(int projectId, List<string>errors)
        {
            try
            {
                return await _unitOfWork.Modules.Get(m => m.IsActive && m.ProjectId == projectId);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return null;
            }
        }

        public async Task<bool>Insert(Module module,IEnumerable<MemberParamsViewModel> members, List<string> errors)
        {
            try
            {
                module.IsActive = true;
                var listMember = members.Select(m => new RoleInProject { IsActive = true, RoleId = m.RoleId, UserName = m.Username }).ToList();
                listMember.Add(new RoleInProject { IsActive=true,RoleId=HardFixJobRole.TeamLead,UserName=module.TeamLead, JoinDate=DateTime.Now});
                for (int i = 0; i < listMember.Count; i++)
                {
                    listMember[i].JoinDate = DateTime.Now;
                }
                module.RoleInProjects = listMember;

                _unitOfWork.Modules.Insert(module);
                var resultCommit = await _unitOfWork.CommitAsync() > 0;
                if (resultCommit)
                {
                    await _projectLog.AddLog(new ProjectLog
                    {
                        Content = $"Create module {module.Title}",
                        CreatedBy = module.Project.CreatedBy,
                        CreatedDate = DateTime.Now,
                        ProjectId = module.ProjectId
                    }, new List<string>());
                }
                return resultCommit;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<DTO.Module>GetById (int id)
        {
            return await _unitOfWork.Modules.GetById(id);
        }

        public async Task<bool> Update(Module module, IEnumerable<MemberParamsViewModel> members, List<string> errors)
        {
            try
            {
                //delete old member
                var oldMembers = await _unitOfWork.RoleInProjects.Get(m => m.ModuleId == module.Id);
                foreach (var member in oldMembers)
                {
                    member.IsActive = false;
                    _unitOfWork.RoleInProjects.Update(member);
                }


                //insert new member
                var listMember = members.Select(m => new RoleInProject { IsActive = true, RoleId = m.RoleId, UserName = m.Username, ModuleId=module.Id}).ToList();
                listMember.Add(new RoleInProject { IsActive = true, RoleId = HardFixJobRole.TeamLead, UserName = module.TeamLead , ModuleId = module.Id });
                foreach (var item in listMember)
                {
                    if(!oldMembers.Any(m => m.UserName.Equals(item.UserName)))
                    {
                        item.JoinDate = DateTime.Now;
                    }
                    else
                    {
                        item.JoinDate = oldMembers.Where(m => m.UserName.Equals(item.UserName)).FirstOrDefault().JoinDate;
                    }
                    item.IsActive = true;
                    _unitOfWork.RoleInProjects.Insert(item);
                }

                //update module
                module.IsActive = true;
                _unitOfWork.Modules.Update(module);

                var resultCommit = await _unitOfWork.CommitAsync() > 0;
                if (resultCommit)
                {
                    await _projectLog.AddLog(new ProjectLog
                    {
                        Content = $"Update module {module.Title}",
                        CreatedBy = module.Project.CreatedBy,
                        CreatedDate = DateTime.Now,
                        ProjectId = module.ProjectId
                    }, new List<string>());
                }
                return resultCommit;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<DTO.Module> Delete(int moduleId, List<string> errors)
        {
            try
            {
                //delete old member
                var oldMembers = await _unitOfWork.RoleInProjects.Get(m => m.ModuleId == moduleId);
                foreach (var member in oldMembers)
                {
                    member.IsActive = false;
                    _unitOfWork.RoleInProjects.Update(member);
                }

                var module = await _unitOfWork.Modules.GetById(moduleId);
                module.IsActive = false;
                _unitOfWork.Modules.Update(module);
                var result = await _unitOfWork.CommitAsync()>0;

                var resultCommit = await _unitOfWork.CommitAsync() > 0;
                if (resultCommit)
                {
                    await _projectLog.AddLog(new ProjectLog
                    {
                        Content = $"Delete module {module.Title}",
                        CreatedBy = module.Project.CreatedBy,
                        CreatedDate = DateTime.Now,
                        ProjectId = module.ProjectId
                    }, new List<string>());
                }

                return result? module:null;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<UserInfo>> GetMembers(int moduleId,string currentUser,List<string> errors)
        {
            try
            {
                var module = await _unitOfWork.Modules.GetById(moduleId);
                IEnumerable<UserInfo> roleInProject = null;
                if (module.Project.CreatedBy.Equals(currentUser))
                {
                    roleInProject = module.RoleInProjects.Where(rp => rp.IsActive).GroupBy(r => r.UserName).Select(r=>r.FirstOrDefault().UserInfo);
                }
                else
                {
                    roleInProject = module.RoleInProjects.Where(rp => rp.IsActive && rp.RoleId!=HardFixJobRole.TeamLead).GroupBy(r => r.UserName).Select(r => r.FirstOrDefault().UserInfo);
                }
                return roleInProject;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return null;
            }
        }
    }
}
