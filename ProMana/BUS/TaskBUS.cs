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
    }
}
