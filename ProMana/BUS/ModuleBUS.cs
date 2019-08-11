using DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ModuleBUS
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public async Task<IEnumerable<Module>>GetByProject(int projectId, List<string>errors)
        {
            try
            {
                return await _unitOfWork.Modules.Get(m => m.IsActive && m.ProjectId == projectId);
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return null;
            }
        }

        public async Task<bool>Insert(Module module, List<string> errors)
        {
            try
            {
                module.IsActive = true;
                _unitOfWork.Modules.Insert(module);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<DTO.Module>GetById (int id)
        {
            return await _unitOfWork.Modules.GetById(id);
        }

        public async Task<bool> Update(Module module, List<string> errors)
        {
            try
            {
                module.IsActive = true;
                _unitOfWork.Modules.Update(module);
                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<DTO.Module> Delete(int moduleId, List<string> errors)
        {
            try
            {
                var module = await _unitOfWork.Modules.GetById(moduleId);
                module.IsActive = false;
                _unitOfWork.Modules.Update(module);
                var result = await _unitOfWork.CommitAsync()>0;

                return result? module:null;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return null;
            }
        }
    }
}
