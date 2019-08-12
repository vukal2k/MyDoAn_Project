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
            return await _unitOfWork.ResolveTypes.Get(r => r.IsActive);
        }

        #region Admin

        public async Task<ResolveType> GetById(int id)
        {
            return await _unitOfWork.ResolveTypes.GetById(id);
        }

        public async Task<bool> Insert(ResolveType resolveType, List<string> errors)
        {
            try
            {
                resolveType.IsActive = true;
                _unitOfWork.ResolveTypes.Insert(resolveType);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(ResolveType resolveType, List<string> errors)
        {
            try
            {
                resolveType.IsActive = true;
                _unitOfWork.ResolveTypes.Update(resolveType);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int resolveTypeId, List<string> errors)
        {
            try
            {
                var resolveType = await _unitOfWork.ResolveTypes.GetById(resolveTypeId);
                resolveType.IsActive = false;
                _unitOfWork.ResolveTypes.Update(resolveType);

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
