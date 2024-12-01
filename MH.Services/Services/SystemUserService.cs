using MH.Common.Constants;
using MH.Common.DTO;
using MH.Common.Enums;
using MH.Common.Models;
using MH.Common.VM;
using MH.DataAccess;
using MH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Services
{
    public class SystemUserService : ISystemUserService
    {
        private readonly MHDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public SystemUserService(
            MHDbContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
       
        public async Task<ResponseMessage> GetAllSystemUser()
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var systemUsers = await _dbContext
                    .SystemUsers
                    .Select(x => new
                    {
                        x.ID,
                        x.Name,
                        x.UserName,
                        x.Email,
                        x.PhoneNumber,
                        x.Role,
                        RoleName = _dbContext.Role
                                    .Where(r => r.RoleID == x.Role)
                                    .Select(r => r.RoleName)
                                    .FirstOrDefault(),
                        GenderName = _dbContext.Gender
                                      .Where(g => g.GenderID == x.Gender)
                                      .Select(g => g.GenderName)
                                      .FirstOrDefault()
                    })
                    .ToListAsync();

                responseMessage.ResponseObj = systemUsers;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> SaveSystemUser(SystemUsers user)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (user is null)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.SaveFailed;
                    return responseMessage;
                }
                if (!string.IsNullOrEmpty(user.Password) && user.Password != user.ConfirmPassword)
                {
                    responseMessage.Message = MessageConstant.ConfirmPasswordNotMatch;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                    return responseMessage;
                }
                if (user.ID > 0)
                {
                    var existingUser = await _dbContext
                        .SystemUsers
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ID == user.ID);

                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    }
                    existingUser.Name = user.Name;
                    existingUser.Email = user.Email;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.Role = user.Role;
                    _dbContext.SystemUsers.Update(existingUser);
                }
                else
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    await _dbContext.SystemUsers.AddAsync(user);
                }

                await _dbContext.SaveChangesAsync();

                user.Password = string.Empty;
                user.ConfirmPassword = string.Empty;
                responseMessage.ResponseObj = user;
                responseMessage.Message = MessageConstant.SavedSuccessfully;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> GetSystemUserById(int userID)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var existingUser = await _dbContext
                    .SystemUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ID == userID);

                responseMessage.ResponseObj = existingUser;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteSystemUserById(int userID)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var existingUser = await _dbContext
                    .SystemUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ID == userID);

                if (existingUser != null)
                {
                    _dbContext.SystemUsers.Remove(existingUser);
                    await _dbContext.SaveChangesAsync();

                    responseMessage.Message = MessageConstant.DeleteSuccess;
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                }
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> UpdatePersonalDetails(VMLogin user)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var existing = await _dbContext
                    .SystemUsers
                    .Where(x => x.ID == user.SystemUserID)
                    .FirstOrDefaultAsync();

                existing.Name = user.Name;
                existing.PhoneNumber = user.PhoneNumber;
                existing.Email = user.Email;

                _dbContext.Update(existing);
                await _dbContext.SaveChangesAsync();
                responseMessage.ResponseObj = existing;
                responseMessage.Message = "Save Changes successfully";
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> UpdatePassword(VMPasswordChange param)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (param.NewPassword != param.ConfirmPassword)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = "New password and confirm password doesn't match";
                    return responseMessage;
                }

                SystemUsers existing = await _dbContext
                    .SystemUsers
                    .Where(x => x.ID == param.SystemUserID)
                    .FirstOrDefaultAsync();

                if (!BCrypt.Net.BCrypt.Verify(param.CurrentPassword, existing.Password))
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = "Current password incorrect";
                    return responseMessage;
                }

                existing.Password = BCrypt.Net.BCrypt.HashPassword(param.NewPassword);

                _dbContext.Update(existing);
                await _dbContext.SaveChangesAsync();
                responseMessage.ResponseObj = existing;
                responseMessage.Message = "Save password successfully";
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> GetAllGender()
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Gender> genders = new List<Gender>();
                genders = await _dbContext
                    .Gender
                    .OrderBy(x => x.GenderID)
                    .ToListAsync();
                responseMessage.ResponseObj = genders;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }
    }
}