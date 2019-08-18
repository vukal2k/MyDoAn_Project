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
                task.CreatedBy = userName;
                task.StatusId = TaskStatusKey.Opened;
                task.IsActive = true;
                task.IsTask = true;
                _unitOfWork.Tasks.Insert(task);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> InsertRequest(DTO.Task task, string userName, List<string> errors)
        {
            try
            {
                var module = await _unitOfWork.Modules.GetById(task.ModuleId);
                if (module.Title.Equals(HardFixJobRoleTitle.Watcher))
                {
                    task.AssignedTo = module.Project.CreatedBy;
                }
                else
                {
                    task.AssignedTo = module.TeamLead;
                }
                task.CreatedBy = userName;
                task.StatusId = RequestStatusKey.PendingApproval;
                task.IsActive = true;
                task.IsTask = false;
                _unitOfWork.Tasks.Insert(task);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<DTO.Task> Update(DTO.Task task,List<string> errors, string userName=null)
        {
            try
            {
                var updateTask = await _unitOfWork.Tasks.GetById(task.Id);
                updateTask.From = task.From;
                updateTask.To = task.To;
                updateTask.Description = task.Description;
                updateTask.AssignedTo = task.AssignedTo;
                updateTask.Severity = task.Severity;
                updateTask.TaskType = task.TaskType;
                updateTask.ModuleId = task.ModuleId;
                updateTask.Title = task.Title;
                updateTask.IsTask = true;
                if (userName != null)
                {
                    updateTask.CreatedBy = userName;
                }

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

        public async Task<DTO.Task> UpdateRequest(DTO.Task task, List<string> errors)
        {
            try
            {
                var updateTask = await _unitOfWork.Tasks.GetById(task.Id);
                updateTask.From = task.From;
                updateTask.To = task.To;
                updateTask.Description = task.Description;
                updateTask.Severity = task.Severity;
                updateTask.TaskType = task.TaskType;
                updateTask.ModuleId = task.ModuleId;
                updateTask.Title = task.Title;
                updateTask.IsTask = false;

                var module = await _unitOfWork.Modules.GetById(task.ModuleId);
                if (module.Title.Equals(HardFixJobRoleTitle.Watcher))
                {
                    updateTask.AssignedTo = module.Project.CreatedBy;
                }
                else
                {
                    updateTask.AssignedTo = module.TeamLead;
                }

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
