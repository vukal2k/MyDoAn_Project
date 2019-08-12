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

        public async Task<IEnumerable<JobRole>> GetAll()
        {
            return await _unitOfWork.JobRoles.Get(j => j.IsActive == true);
        }

        public async Task<JobRole> GetById(int id)
        {
            return await _unitOfWork.JobRoles.GetById(id);
        }

        public async Task<bool> Insert(JobRole jobRole, List<string>errors)
        {
            try
            {
                jobRole.IsActive = true;
                jobRole.CanDelete = true;
                _unitOfWork.JobRoles.Insert(jobRole);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(JobRole jobRole, List<string> errors)
        {
            try
            {
                jobRole.IsActive = true;
                _unitOfWork.JobRoles.Update(jobRole);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int jobRoleId, List<string> errors)
        {
            try
            {
                var jobRole = await _unitOfWork.JobRoles.GetById(jobRoleId);
                if (jobRole.CanDelete)
                {
                    jobRole.IsActive = false;
                    _unitOfWork.JobRoles.Update(jobRole);

                    return await _unitOfWork.CommitAsync() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        #endregion
    }
}
