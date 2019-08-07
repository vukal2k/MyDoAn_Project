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
    public class SolutionBUS
    {
        private UnitOfWork _unitOfWork;

        public SolutionBUS()
        {
            _unitOfWork = new UnitOfWork();
        }

        #region Public method
        public async Task<bool> Create(Solution solution,string userName,int statusId,List<string> errors)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetById(solution.TaskId);
                task.StatusId = statusId;
                _unitOfWork.Tasks.Update(task);

                solution.CreatedBy = userName;
                solution.CreatedDateTime = DateTime.Now;

                _unitOfWork.Solutions.Insert(solution);

                return await _unitOfWork.CommitAsync() > 0;
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
