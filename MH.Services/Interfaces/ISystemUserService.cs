using MH.Common.DTO;
using MH.Common.Models;
using MH.Common.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Interfaces
{
    public interface ISystemUserService
    {
        Task<ResponseMessage> GetAllSystemUser();
        Task<ResponseMessage> SaveSystemUser(SystemUsers user);
        Task<ResponseMessage> GetSystemUserById(int userID);
        Task<ResponseMessage> DeleteSystemUserById(int userID);
    }
}
