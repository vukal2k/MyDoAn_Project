using DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class JobRoleBUS
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        #region public region
        public async Task<IEnumerable<JobRole>> GetSoftRole()
        {
            return await _unitOfWork.JobRoles.Get(j => j.IsActive == true && j.CanDelete == true);
        }
        #endregion
    }
}
