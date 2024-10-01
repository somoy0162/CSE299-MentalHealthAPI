using MH.Common.Constants;
using MH.Common.DTO;
using MH.Common.Enums;
using MH.Common.Models;
using MH.DataAccess;
using MH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Services
{
    public class UserSessionService : IUserSessionService
    {
        private readonly MHDbContext _dbContext;
        public UserSessionService(MHDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<ResponseMessage> GetAllUserSession(RequestMessage requestMessage)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                responseMessage.ResponseObj = await _dbContext
                    .UserSession
                    .AsNoTracking()
                    .OrderBy(x => x.UserSessionID)
                    .Skip(requestMessage.Skip)
                    .Take(requestMessage.PageRecordSize)
                    .ToListAsync();

                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> GetUserSessionById(int id)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                responseMessage.ResponseObj = await _dbContext
                    .UserSession
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x =>
                        x.UserSessionID == id &&
                        x.Status == (int)Enums.Status.Active
                    );
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetUserSessionBySystemUserId(int userId)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                responseMessage.ResponseObj = await _dbContext
                    .UserSession
                    .AsNoTracking()
                    .OrderBy(u => u.UserSessionID)
                    .LastOrDefaultAsync(x =>
                        x.SystemUserID == userId &&
                        x.Status == (int)Enums.Status.Active &&
                        x.SessionEnd > DateTime.Now
                    );
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> SaveUserSession(UserSession userSession)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (userSession is null)
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
                    responseMessage.Message = MessageConstant.SaveFailed;
                    return responseMessage;
                }

                if (!CheckedValidation(userSession, responseMessage))
                {
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Warning;
                    return responseMessage;
                }

                if (userSession.UserSessionID > 0)
                {
                    var existingUserSession = await _dbContext
                        .UserSession
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x =>
                            x.UserSessionID == userSession.UserSessionID &&
                            x.Status == (int)Enums.Status.Active
                        );

                    userSession.CreatedDate = existingUserSession.CreatedDate;
                    userSession.CreatedBy = existingUserSession.CreatedBy;
                    _dbContext.UserSession.Update(userSession);
                }
                else
                {
                    userSession.Status = (int)Enums.Status.Active;
                    userSession.CreatedDate = DateTime.Now;
                    userSession.CreatedBy = userSession.SystemUserID;
                    await _dbContext.UserSession.AddAsync(userSession);
                }

                await _dbContext.SaveChangesAsync();
                responseMessage.ResponseObj = userSession;
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

        private static bool CheckedValidation(UserSession objUserSession, ResponseMessage responseMessage)
        {
            if (string.IsNullOrEmpty(objUserSession?.Token))
            {
                responseMessage.Message = MessageConstant.Token;
                return false;

            }
            return true;
        }
    }
}
