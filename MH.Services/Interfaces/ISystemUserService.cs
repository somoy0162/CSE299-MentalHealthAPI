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
        //Task<ResponseMessage> GetSystemUsersByRole(RequestMessage requestMessage);
        //Task<ResponseMessage> GetSystemUserByUserName(RequestMessage requestMessage);
        //Task<ResponseMessage> GetSystemUserByEmail(RequestMessage requestMessage);
        Task<ResponseMessage> DeleteSystemUserById(int userID);
        //Task<ResponseMessage> GetDropdownOrganizationByUserID(RequestMessage requestMessage);
        //Task<ResponseMessage> GetSystemUserDetails(RequestMessage requestMessage);
        //Task<ResponseMessage> UpdateSystemUser(RequestMessage requestMessage);
        Task<ResponseMessage> UpdatePassword(VMForgotPassword vmForgotPassword);
        //Task<ResponseMessage> ChangePassword(RequestMessage requestMessage);
        //Task<ResponseMessage> getSystemUsersForDropdown(RequestMessage requestMessage);
        //Task<ResponseMessage> getAssigneeSystemUsersForDropdown(RequestMessage requestMessage);
        //Task<ResponseMessage> GetSystemUsers(RequestMessage requestMessage);
        //Task<ResponseMessage> ResetPassword(RequestMessage requestMessage);
        //Task<ResponseMessage> SendMailForgotPassword(string userName);
    }
}
