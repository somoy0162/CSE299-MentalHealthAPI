using MH.Common.DTO;
using MH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MH.API.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService userService)
        {
            this._roleService = userService;
        }

        [HttpGet("GetAllRole")]
        public async Task<ResponseMessage> GetAllRole()
        {
            return await _roleService.GetAllRole();
        }
    }
}
