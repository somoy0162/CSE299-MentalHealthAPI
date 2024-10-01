using MH.Common.DTO;
using MH.Common.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<ResponseMessage> Login(VMLogin vMLogin);
        Task<ResponseMessage> Logout();
        Task<ResponseMessage> Register(VMRegister objVMRegister);
        Task<bool> CheckPermission(string url);
        Task<Boolean> IsPermitRegistration();
    }
}
