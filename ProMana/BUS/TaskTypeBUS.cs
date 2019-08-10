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
            return await _unitOfWork.TaskTypes.GetAll();
        }
    }
}
