using MH.Common.DTO;
using MH.Common.Models;
using MH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MH.API.Controllers
{
    [Route("api/permission")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("GetAllPermission")]
        public async Task<ResponseMessage> GetAllPermission()
        {
            return await _permissionService.GetAllPermission();
        }

        [HttpGet("GetAllPermissionActionByRoleID/{roleID}")]
        public async Task<ResponseMessage> GetAllPermissionActionByRoleID(int roleID)
        {
            return await _permissionService.GetAllPermissionActionByRoleID(roleID);
        }

        [HttpPost("SaveRolePermissionMapping")]
        public async Task<ResponseMessage> SaveRolePermissionMapping(RolePermissionMapping rolePermissionMapping)
        {
            return await _permissionService.SaveRolePermissionMapping(rolePermissionMapping);
        }
    }
}
