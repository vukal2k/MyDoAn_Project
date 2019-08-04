﻿using DTO;
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

        public SolutionBUS()
        {
            _unitOfWork = new UnitOfWork();
        }

        #region Public method
        public async Task<bool> Create(Solutionn solution,string userName, List<string> errors)
        {
            try
            {
                solution.CreatedBy = userName;
                solution.CreatedDateTime = DateTime.Now;

                _unitOfWork.Solutions.Insert(solution);

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
