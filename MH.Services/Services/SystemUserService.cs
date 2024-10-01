using MH.Common.Constants;
using MH.Common.DTO;
using MH.Common.Enums;
using MH.Common.Models;
using MH.Common.VM;
using MH.DataAccess;
using MH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        //private readonly IEmailService _emailService;
        public SystemUserService(
            MHDbContext dbContext,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        //public async Task<ResponseMessage> SendMailForgotPassword(string userName)
        //{
        //    ResponseMessage responseMessage = new ResponseMessage();
        //    try
        //    {
        //        string domainUrl = _configuration.GetSection("Auth").GetSection("DomainUrl").Value.ToString();
        //        //var userName = vmUser.UserName;
        //        var systemUser = await GetSystemUserByName(userName);
        //        if (systemUser == null || string.IsNullOrEmpty(systemUser.Email))
        //        {
        //            responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
        //            responseMessage.Message = "We could not found valid user";
        //            return responseMessage;
        //        }

        //        string generateToken = GenerateToken(systemUser.ID);
        //        string resetLink = $"{domainUrl}reset-password/{generateToken}";
        //        string html = @$"
        //            <p> Welcome HIH reset password. </p>
        //            <a href='{resetLink}'>Reset Password</a>";

        //        if (!await _emailService.SendEmailToSendGrid(systemUser.Email, "Password Reset For HIH", "", html))
        //        {
        //            responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
        //            responseMessage.Message = "Something went wrong,Please try again later";
        //            return responseMessage;
        //        }

        //        await SaveForgotTokenAsync(systemUser.ID, generateToken);

        //        responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
        //        responseMessage.Message = "We have send a reset password url into mail";
        //    }
        //    catch (Exception ex)
        //    {
        //        responseMessage.Message = ex.Message;
        //        responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
        //    }
        //    return responseMessage;
        //}

        public async Task<SystemUsers> GetSystemUserByName(string userName) => await _dbContext.SystemUsers
            .AsNoTracking().Where(x => x.UserName == userName).FirstOrDefaultAsync();

        private static string GenerateToken(int systemuserId)
        {
            string dataToHash = $"{systemuserId}-{DateTime.UtcNow.Ticks}";
            byte[] dataToHashBytes = Encoding.UTF8.GetBytes(dataToHash);

            using SHA256 sha256 = SHA256.Create();
            byte[] hashedData = sha256.ComputeHash(dataToHashBytes);

            var builder = new StringBuilder();
            foreach (byte b in hashedData)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

        private async Task<bool> SaveForgotTokenAsync(int userId, string token)
        {
            var forgotPassWordToken = new ForgotPasswordToken()
            {
                SystemUserID = userId,
                VerifiedToken = token,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };
            await _dbContext.ForgotPasswordToken.AddAsync(forgotPassWordToken);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<ResponseMessage> UpdatePassword(VMForgotPassword vmForgotPassword)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (!await IsValidToken(vmForgotPassword.Token))
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = "Invalid Request";
                    return responseMessage;
                }

                var existForgotPasswordToken = await _dbContext
                    .ForgotPasswordToken
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.VerifiedToken == vmForgotPassword.Token);

                if (existForgotPasswordToken is null)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = "Invalid Request";
                    return responseMessage;
                }

                var systemUser = await _dbContext
                    .SystemUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ID == existForgotPasswordToken.SystemUserID);

                if (systemUser is null)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = "Invalid Request";
                    return responseMessage;
                }

                systemUser.Password = BCrypt.Net.BCrypt.HashPassword(vmForgotPassword.Password);
                //systemUser.Password = vmForgotPassword.Password;
                existForgotPasswordToken.IsVerified = true;
                _dbContext.SystemUsers.Update(systemUser);
                _dbContext.ForgotPasswordToken.Update(existForgotPasswordToken);
                await _dbContext.SaveChangesAsync();

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                responseMessage.Message = MessageConstant.SavedSuccessfully;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        private async Task<bool> IsValidToken(string token)
        {
            var currentTime = DateTime.UtcNow;
            return await _dbContext
                .ForgotPasswordToken
                .AnyAsync(x => x.VerifiedToken == token && !x.IsVerified.Value && EF.Functions.DateDiffMinute(currentTime, x.CreatedAt) <= 5);
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
    }
}
