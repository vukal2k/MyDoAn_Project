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
        private ProjectLogBUS _projectLog;

        public SolutionBUS()
        {
            _unitOfWork = new UnitOfWork();
            _projectLog = new ProjectLogBUS();
        }

        #region Public method
        public async Task<bool> Create(Solution solution,string userName,int statusId,List<string> errors)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetById(solution.TaskId);
                var statusFrom = task.LookupStatus.Name;
                task.StatusId = statusId;
                _unitOfWork.Tasks.Update(task);

                solution.CreatedBy = userName;
                solution.CreatedDateTime = DateTime.Now;

                _unitOfWork.Solutions.Insert(solution);

                var result = await _unitOfWork.CommitAsync() > 0;

                if (result)
                {
                    var statusTo = await _unitOfWork.LookupStatuss.GetById(statusId);
                    await _projectLog.AddLog(new ProjectLog
                    {
                        Content = $"Change status task {task.Title} from {statusFrom} to {statusTo.Name}",
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        ProjectId = task.Module.ProjectId
                    }, new List<string>());
                }
                return result;
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
