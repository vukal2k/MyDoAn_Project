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
    public class TaskBUS
    {
        private UnitOfWork _unitOfWork;

        public TaskBUS()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<Tassk> GetById(int taskId, List<string>errors)
        {
            try
            {
                var result = await _unitOfWork.Tassks.GetById(taskId);
                return result;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return null;
            }
        }

        public async Task<Tassk> CheckSolutionTaskPermission(int taskId,int statusId,string userName,List<string> errors)
        {
            try
            {
                var result = await _unitOfWork.Tassks.GetById(taskId);

                switch (statusId)
                {
                    case TaskStatusKey.InProgress:
                    case TaskStatusKey.Resolved:
                        if(result.StatusId != TaskStatusKey.Closed)
                        {
                            return result;
                        }
                        return null;
                    case TaskStatusKey.Closed:
                        if (result.CreatedBy == userName)
                        {
                            result.StatusId = TaskStatusKey.Closed;
                            _unitOfWork.Tassks.Update(result);

                            var success = await _unitOfWork.CommitAsync() > 0;
                            if (success)
                            {
                                return result;
                            }
                            return null;
                        }
                        else
                        {
                            return null;
                        }
                    default:
                        break;
                }

                return null;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return null;
            }
        }
    }
}
