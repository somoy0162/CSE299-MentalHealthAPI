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
    public class RoleService : IRoleService
    {
        private readonly MHDbContext _dbContext;
        public RoleService(MHDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResponseMessage> GetAllRole()
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Roles> roles = new List<Roles>();
                roles = await _dbContext
                    .Role
                    .OrderBy(x => x.RoleID)
                    .ToListAsync();
                responseMessage.ResponseObj = roles;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

    }
}
