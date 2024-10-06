﻿using MH.Common.DTO;
using MH.Common.Models;
using MH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MH.API.Controllers
{
    [Route("api/systemUser")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        private readonly ISystemUserService _systemUserService;

        public SystemUserController(ISystemUserService systemUserService)
        {
            _systemUserService = systemUserService;
        }

        [HttpGet("GetAllSystemUser")]
        public async Task<ResponseMessage> GetAllSystemUser()
        {
            return await _systemUserService.GetAllSystemUser();
        }

        [HttpPost("SaveSystemUser")]
        public async Task<ResponseMessage> SaveSystemUser(SystemUsers user)
        {
            return await _systemUserService.SaveSystemUser(user);
        }

        [HttpGet("GetSystemUserById/{userID}")]
        public async Task<ResponseMessage> GetSystemUserById(int userID)
        {
            return await _systemUserService.GetSystemUserById(userID);
        }

        [HttpGet("DeleteSystemUserById/{userID}")]
        public async Task<ResponseMessage> DeleteSystemUserById(int userID)
        {
            return await _systemUserService.DeleteSystemUserById(userID);
        }
    }
}