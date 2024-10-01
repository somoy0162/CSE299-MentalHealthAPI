using MH.Common.Constants;
using MH.Common.DTO;
using MH.Common.Enums;
using MH.Common.Models;
using MH.Common.VM;
using MH.Common.Helpers;
using MH.DataAccess;
using MH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly MHDbContext _dbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SecurityService(
            MHDbContext dbContext,
            IServiceScopeFactory serviceScopeFactor,
            IAccessTokenService accessTokenService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _dbContext = dbContext;
            _serviceScopeFactory = serviceScopeFactor;
            _accessTokenService = accessTokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ResponseMessage> Login(VMLogin vmLogin)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (vmLogin is null)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = "Invalid Request";
                    return responseMessage;
                }

                var systemUser = await _dbContext
                    .SystemUsers
                    .AsNoTracking()
                    .Where(x =>
                        x.UserName == vmLogin.UserName
                    )
                    .FirstOrDefaultAsync();

                if (systemUser is null)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = "Invalid username or password";
                    return responseMessage;
                }

                if (!BCrypt.Net.BCrypt.Verify(vmLogin.Password, systemUser.Password))
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = "Invalid username or password";
                    return responseMessage;
                }

                vmLogin.SystemUserID = systemUser.ID;
                vmLogin.Password = String.Empty;
                var accessToken = await _accessTokenService.Create(new Common.Models.AccessToken()
                {
                    SystemUserID = systemUser.ID,
                });

                vmLogin.Token = accessToken.Token;
                vmLogin.UserName = systemUser.UserName;
                vmLogin.Role = systemUser.Role;
                var permissions = await GetAllPermissionByRoleID(systemUser.Role);
                responseMessage.ResponseObj = new { vmLogin, permissions };
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.Message = MessageConstant.LoginSuccess;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        private async Task<List<Permission>> GetAllPermissionByRoleID(int? roleID)
        {
            var ids = await _dbContext
                .RolePermissionMapping
                .AsNoTracking()
                .Where(r => r.RoleID == roleID)
                .Select(s => s.PermissionID)
                .Distinct()
                .ToArrayAsync();

            return await _dbContext
                .Permission
                .AsNoTracking()
                .Where(p => ids.Contains(p.PermissionID))
                .ToListAsync();
        }

        public async Task<Boolean> IsPermitRegistration()
        {
            var actionID = await _dbContext
                .Actions
                .Where(x => x.ActionName.Equals("registration"))
                .Select(x => x.ActionID)
                .FirstOrDefaultAsync();

            var eistingRoleActionMapping = await _dbContext
                .RoleActionMapping
                .AsNoTracking()
                .AnyAsync(x => x.RoleID == 1 && x.ActionID == actionID);

            return eistingRoleActionMapping;
        }

        public async Task<ResponseMessage> Logout()
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                int userId = _httpContextAccessor.TokenValue<int>(JwtClaim.UserId);
                var userSession = await _dbContext
                    .UserSession
                    .AsNoTracking()
                    .Where(x =>
                        x.SystemUserID == userId
                    )
                    .ToListAsync();

                if (userSession.Count == 0)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                    responseMessage.Message = MessageConstant.LogOutSuccessfully;
                    return responseMessage;
                }

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetService<MHDbContext>();
                    foreach (var session in userSession)
                    {
                        session.SessionEnd = DateTime.Now;
                        session.Status = (int)Enums.Status.Inactive;
                    }
                    db.UserSession.UpdateRange(userSession);
                    await db.SaveChangesAsync();
                }

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.Message = MessageConstant.LogOutSuccessfully;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }
        public async Task<bool> CheckPermission(string url)
        {
            return await Task.FromResult(true);
        }
        public async Task<ResponseMessage> Register(VMRegister objVMRegister)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (objVMRegister == null)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.Invaliddatafound;
                    return responseMessage;
                }
                else
                {
                    if (CheckedValidation(objVMRegister, responseMessage))
                    {
                        if (objVMRegister.Password != objVMRegister.ConfirmPassword)
                        {
                            responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                            responseMessage.Message = MessageConstant.Confirmpasswordnotmatch;
                            return responseMessage;
                        }

                        SystemUsers objSystemUser = new SystemUsers();
                        objSystemUser.UserName = objVMRegister.UserName;
                        objSystemUser.Name = objVMRegister.Name;
                        objSystemUser.Email = objVMRegister.Email;
                        objSystemUser.PhoneNumber = objVMRegister.PhoneNumber;
                        objSystemUser.Password = BCrypt.Net.BCrypt.HashPassword(objVMRegister.Password);
                        //objSystemUser.Password = objVMRegister.Password;

                        await _dbContext.SystemUsers.AddAsync(objSystemUser);
                        await _dbContext.SaveChangesAsync();

                        objSystemUser.Password = string.Empty;
                        responseMessage.ResponseObj = objSystemUser;
                        responseMessage.Message = MessageConstant.RegisterSuccessfully;
                        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }
        private bool CheckedValidation(VMRegister objVMRegister, ResponseMessage responseMessage)
        {
            SystemUsers existingSystemUser = new SystemUsers();
            if (String.IsNullOrEmpty(objVMRegister.UserName))
            {
                responseMessage.Message = MessageConstant.UsernameRequired;
                return false;
            }
            if (String.IsNullOrEmpty(objVMRegister.Email))
            {
                responseMessage.Message = MessageConstant.EmailRequired;
                return false;
            }
            if (String.IsNullOrEmpty(objVMRegister.Password))
            {
                responseMessage.Message = MessageConstant.PasswordRequired;
                return false;
            }
            existingSystemUser = _dbContext
                .SystemUsers
                .Where(x => x.UserName.ToLower() == objVMRegister.UserName.ToLower()).AsNoTracking().FirstOrDefault();

            if (existingSystemUser != null)
            {
                responseMessage.Message = MessageConstant.DuplicateUserName;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                return false;
            }
            existingSystemUser = _dbContext
                .SystemUsers
                .Where(x => x.Email == objVMRegister.Email).AsNoTracking().FirstOrDefault();

            if (existingSystemUser != null)
            {
                responseMessage.Message = MessageConstant.EmailAlreadyExist;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                return false;
            }
            return true;
        }
    }
}
