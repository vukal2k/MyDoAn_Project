using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MemberParamsViewModel
    {
        public string Username { get; set; }
        public int RoleId { get; set; }
    }

    public class MemberDetailViewModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public string RoleTitle { get; set; }
    }
}
