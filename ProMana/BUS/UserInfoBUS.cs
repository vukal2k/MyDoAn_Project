using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class UserInfoBUS
    {
         private UnitOfWork _unitOfWork;

        public UserInfoBUS()
        {
            _unitOfWork = new UnitOfWork();
        }
    }
}
