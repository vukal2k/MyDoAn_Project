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
            var result = project.RoleInProjects.GroupBy(m => m.UserInfo)
                                .Select(u => u.Key);
            return result;
        }
    }
}
