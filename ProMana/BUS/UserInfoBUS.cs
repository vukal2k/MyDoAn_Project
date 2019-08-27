using DTO;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class UserInfoBUS
    {
        private UnitOfWork _unitOfWork;

        public UserInfoBUS()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<IEnumerable<UserInfo>> GetAll()
        {
            return await _unitOfWork.UserInfos.Get(j => j.IsActive == true);
        }


        public async Task<UserInfo> GetById(string username)
        {
            return await _unitOfWork.UserInfos.GetById(username);
        }

        public async Task<bool> Create(UserInfo userInfo, List<string> errors)
        {
            try
            {
                if (await Validate(userInfo, errors))
                {
                    userInfo.IsActive = true;
                    _unitOfWork.UserInfos.Insert(userInfo);
                }

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Update(UserInfo userInfo, List<string> errors)
        {
            try
            {
                if (await Validate(userInfo, errors,true))
                {
                    userInfo.IsActive = true;
                    _unitOfWork.UserInfos.Update(userInfo);
                }

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(string userName, List<string> errors)
        {
            try
            {
                var userInfo = await _unitOfWork.UserInfos.GetById(userName);
                userInfo.IsActive = false;
                _unitOfWork.UserInfos.Update(userInfo);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                return false;
            }
        }

        private async Task<bool>Validate(UserInfo userInfo, List<string> errors, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                var isUserNameExists = await _unitOfWork.UserInfos.CheckExist(u => u.UserName.Equals(userInfo.UserName));
                if (isUserNameExists)
                {
                    errors.Add("Username is exists");
                }
            }

            return !errors.Any();
        }
    }
}
