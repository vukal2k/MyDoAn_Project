using DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ResolveTypeBUS
    {
        private UnitOfWork _unitOfWork;

        public ResolveTypeBUS()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<IEnumerable<ResolveType>> GetAll()
        {
            return await _unitOfWork.ResolveTypes.GetAll();
        }
    }
}
