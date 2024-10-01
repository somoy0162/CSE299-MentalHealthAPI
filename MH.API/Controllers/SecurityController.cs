using MH.Common.DTO;
using MH.Common.VM;
using MH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MH.API.Controllers
{
    [Route("api/security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly ISystemUserService _systemUserService;
        public SecurityController(
            ISecurityService securityService,
            ISystemUserService systemUserService
            )
        {
            _securityService = securityService;
            _systemUserService = systemUserService;
        }

        [HttpGet("IsPermitRegistration")]
        public async Task<Boolean> IsPermitRegistration()
        {
            return await _securityService.IsPermitRegistration();
        }

        [HttpPost("Login")]
        public async Task<ResponseMessage> Login(VMLogin vmLogin)
        {
            return await _securityService.Login(vmLogin);
        }

        [HttpGet("Logout")]
        public async Task<ResponseMessage> Logout()
        {
            return await _securityService.Logout();
        }

        [HttpPost("Register")]
        public async Task<ResponseMessage> Register(VMRegister objVMRegister)
        {
            return await _securityService.Register(objVMRegister);
        }

        //[HttpGet("SendEmailForgotPassword/{userName}")]
        //public async Task<ResponseMessage> SendEmailForgotPassword(string userName)
        //{
        //    return await _systemUserService.SendMailForgotPassword(userName);
        //}

        [HttpPost("UpdatePassword")]
        public async Task<ResponseMessage> Updatepassword(VMForgotPassword vmForgotPassword)
        {
            return await _systemUserService.UpdatePassword(vmForgotPassword);
        }
    }
}
