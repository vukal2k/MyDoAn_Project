using DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class TaskTypeBUS
    {
        private UnitOfWork _unitOfWork;

        public TaskTypeBUS()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<IEnumerable<TaskType>> GetAll()
        {
            return await _unitOfWork.TaskTypes.Get(r => r.IsActive);
        }

        #region Admin
        public async Task<TaskType> GetById(int id)
        {
            return await _unitOfWork.TaskTypes.GetById(id);
        }
        public async Task<bool> Insert(TaskType taskType, List<string> errors)
        {
            try
            {
                taskType.IsActive = true;
                _unitOfWork.TaskTypes.Insert(taskType);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(TaskType taskType, List<string> errors)
        {
            try
            {
                taskType.IsActive = true;
                _unitOfWork.TaskTypes.Update(taskType);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int taskTypeId, List<string> errors)
        {
            try
            {
                var taskType = await _unitOfWork.TaskTypes.GetById(taskTypeId);
                taskType.IsActive = false;
                _unitOfWork.TaskTypes.Update(taskType);

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
