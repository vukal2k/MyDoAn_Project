using DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ProjectLogBUS
    {
        private UnitOfWork _unitOfWork;
        public async Task<bool> AddLog(ProjectLog projectLog, List<string> errors)
        {
            try
            {
                _unitOfWork.ProjectLogs.Insert(projectLog);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }
    }
}
