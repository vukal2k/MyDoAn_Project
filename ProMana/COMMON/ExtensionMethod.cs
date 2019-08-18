using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace COMMON
{
    public static class ExtensionMethod
    {
        public static IEnumerable<UserInfo>GetMember(this Project project)
        {
            var result = project.Modules.SelectMany(r => r.RoleInProjects).GroupBy(m=> m.UserInfo).Select(u=>u.Key);
            return result;
        }

        public static IEnumerable<UserInfo> GetMemberByModule(this Project project, int? moduleId)
        {
            var result = project.Modules.Where(m => m.Id==moduleId).SelectMany(r => r.RoleInProjects).GroupBy(m => m.UserInfo).Select(u => u.Key);
            return result;
        }

        public static IEnumerable<MemberParamsViewModel> GetMemberParams(this Module module)
        {
            var result = module.RoleInProjects.Where(r => r.IsActive).Select(m => new MemberParamsViewModel { RoleId = m.RoleId, Username = m.UserName });
            return result;
        }

        public static int GetRoleInModule(this Project project, string userName)
        {
            if (userName.Equals(project.CreatedBy))
            {
                return HardFixJobRole.PM;
            }

            var watcherModule = project.Modules.Where(m => m.Title.Equals(HardFixJobRoleTitle.Watcher)).FirstOrDefault();
            if (watcherModule != null)
            {
                var watcher = watcherModule.RoleInProjects.Where(u => u.UserName.Equals(userName) && u.IsActive==true).FirstOrDefault();
                if (watcher != null)
                {
                    return HardFixJobRole.Watcher;
                }
            }

            var listTeamLead = project.Modules.Where(m => !m.Title.Equals(HardFixJobRoleTitle.Watcher) && m.IsActive).Select(u => u.TeamLead);
            var teamLead = listTeamLead.Contains(userName);
            if (teamLead)
            {
                return HardFixJobRole.TeamLead;
            }

            return 0;
        }

        public static IEnumerable<Module> GetOwnModule(this Project project, string userName)
        {
            if (!project.CreatedBy.Equals(userName))
            {
                var result = project.Modules.Where(m => !m.Title.Equals(HardFixJobRoleTitle.Watcher)).Where(m => m.IsActive && m.TeamLead.Equals(userName));
                return result;
            }
            else
            {
                return project.Modules.Where(m => !m.Title.Equals(HardFixJobRoleTitle.Watcher) && m.IsActive);
            }
        }

        public static IEnumerable<Module> GetModuleIn(this Project project, string userName)
        {
            IEnumerable<Module> result = null;
            switch (project.GetRoleInModule(userName))
            {
                case HardFixJobRole.PM:
                    result = project.Modules.Where(m => !m.Title.Equals(HardFixJobRoleTitle.Watcher));
                    break;
                case HardFixJobRole.Watcher:
                    result = project.Modules.Where(m => m.Title.Equals(HardFixJobRoleTitle.Watcher));
                    break;
                case HardFixJobRole.TeamLead:
                    result = project.Modules.Where(m => m.IsActive && m.TeamLead.Equals(userName));
                    break;
                default:
                    result = project.Modules.Where(m => !m.Title.Equals(HardFixJobRoleTitle.Watcher)).Where(m => m.IsActive && m.RoleInProjects.Where(r => r.UserName.Equals(userName) && r.IsActive).FirstOrDefault() != null);
                    break;
            }
            return result;
        }
    }
}
