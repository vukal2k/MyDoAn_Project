using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<MemberParamsViewModel> GetMemberParams(this Module module)
        {
            var result = module.RoleInProjects.Where(r => r.IsActive).Select(m => new MemberParamsViewModel { RoleId = m.RoleId, Username = m.UserName });
            return result;
        }
    }
}
