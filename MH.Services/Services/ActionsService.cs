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
    public class ActionsService : IActionsService
    {
        private readonly MHDbContext _dbContext;
        public ActionsService(MHDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResponseMessage> SaveRoleActionMapping(RoleActionMapping roleActionMapping)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (roleActionMapping != null)
                {
                    RoleActionMapping exisRoleActionMapping = await _dbContext
                        .RoleActionMapping
                        .AsNoTracking()
                        .Where(x => x.RoleID == roleActionMapping.RoleID && x.ActionID == roleActionMapping.ActionID)
                        .FirstOrDefaultAsync();

                    if (exisRoleActionMapping != null)
                    {
                        _dbContext.RoleActionMapping.Remove(exisRoleActionMapping);
                        await _dbContext.SaveChangesAsync();
                        responseMessage.Message = "Access Off";
                    }
                    else
                    {
                        await _dbContext.RoleActionMapping.AddAsync(roleActionMapping);
                        await _dbContext.SaveChangesAsync();
                        responseMessage.Message = "Access On";
                    }
                    responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
                }
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
