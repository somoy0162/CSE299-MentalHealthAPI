using MH.Common.DTO;
using MH.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Interfaces
{
    public interface IUserSessionService
    {
        Task<ResponseMessage> GetAllUserSession(RequestMessage requestMessage);
        Task<ResponseMessage> SaveUserSession(UserSession userSession);
        Task<ResponseMessage> GetUserSessionById(int id);
        Task<ResponseMessage> GetUserSessionBySystemUserId(int userId);
    }
}
