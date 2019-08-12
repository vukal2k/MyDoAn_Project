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

        public async Task<DTO.Task> GetById(int taskId, List<string>errors)
        {
            try
            {
                var result = await _unitOfWork.Tasks.GetById(taskId);
                return result;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return null;
            }
        }

        public async Task<DTO.Task> CheckSolutionTaskPermission(int taskId,int statusId,string userName,List<string> errors)
        {
            try
            {
                var result = await _unitOfWork.Tasks.GetById(taskId);

                switch (statusId)
                {
                    case TaskStatusKey.InProgress:
                        if (result.StatusId != TaskStatusKey.Closed)
                        {
                            result.StatusId = TaskStatusKey.InProgress;
                            _unitOfWork.Tasks.Update(result);

                            var success = await _unitOfWork.CommitAsync() > 0;
                            if (success)
                            {
                                return result;
                            }
                        }
                        return null;
                    case TaskStatusKey.Resolved:
                        if(result.StatusId != TaskStatusKey.Closed)
                        {
                            return result;
                        }
                        return null;
                    case TaskStatusKey.Closed:
                    case TaskStatusKey.ReOpened:
                        if (result.CreatedBy == userName)
                        {
                            result.StatusId = statusId;
                            _unitOfWork.Tasks.Update(result);

                            var success2 = await _unitOfWork.CommitAsync() > 0;
                            if (success2)
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

        public async Task<bool> Insert(DTO.Task task,string userName,List<string> errors)
        {
            try
            {
                if(task.Level<=0 || task.Level > 5)
                {
                    errors.Add("Level between from 1 to 5");
                    return false;
                }

                task.CreatedBy = userName;
                task.StatusId = TaskStatusKey.Opened;
                task.IsActive = true;
                _unitOfWork.Tasks.Insert(task);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<DTO.Task> Update(DTO.Task task, List<string> errors)
        {
            try
            {
                if (task.Level <= 0 || task.Level > 5)
                {
                    errors.Add("Level between from 1 to 5");
                    return null;
                }

                var updateTask = await _unitOfWork.Tasks.GetById(task.Id);
                updateTask.From = task.From;
                updateTask.To = task.To;
                updateTask.Description = task.Description;
                updateTask.AssignedTo = task.AssignedTo;
                updateTask.Level = task.Level;
                updateTask.TaskTypeId = task.TaskTypeId;
                updateTask.ModuleId = task.ModuleId;
                updateTask.Title = task.Title;

                _unitOfWork.Tasks.Update(updateTask);
                var result = await _unitOfWork.CommitAsync() > 0;

                return updateTask;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return null;
            }
        }
    }
}
