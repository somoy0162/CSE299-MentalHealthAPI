using MH.Common.DTO;
using MH.Common.Models;
using MH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MH.API.Controllers
{
    [Route("api/Actions")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private readonly IActionsService _actionsService;
        public ActionsController(IActionsService userService)
        {
            this._actionsService = userService;
        }

        [HttpPost("SaveRoleActionMapping")]
        public async Task<ResponseMessage> SaveRoleActionMapping(RoleActionMapping roleActionMapping)
        {
            return await _actionsService.SaveRoleActionMapping(roleActionMapping);
        }
    }
}
