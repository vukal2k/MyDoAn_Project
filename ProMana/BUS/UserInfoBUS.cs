using DTO;
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

        public async Task<IEnumerable<UserInfo>> GetAll()
        {
            return await _unitOfWork.UserInfos.Get(j => j.IsActive == true);
        }
        

        public async Task<UserInfo> GetById(string username)
        {
            return await _unitOfWork.UserInfos.GetById(username);
        }
    }
}
