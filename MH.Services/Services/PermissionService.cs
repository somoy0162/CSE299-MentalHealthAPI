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
    public class PermissionService : IPermissionService
    {
        private readonly MHDbContext _dbContext;
        public PermissionService(MHDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ResponseMessage> GetAllPermission()
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<Permission> lstPermission = new List<Permission>();
                lstPermission = await _dbContext
                    .Permission
                    .ToListAsync();

                foreach (Permission permission in lstPermission)
                {
                    permission.Actions = _dbContext
                        .Actions
                        .Where(x => x.PermissionID == permission.PermissionID)
                        .ToList();
                }

                responseMessage.TotalCount = lstPermission.Count;
                responseMessage.ResponseObj = lstPermission;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }

            return responseMessage;
        }

        public async Task<ResponseMessage> GetAllPermissionActionByRoleID(int roleID)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                List<RolePermissionMapping> rolePermissionMappings = new List<RolePermissionMapping>();
                List<RoleActionMapping> roleActionMappings = new List<RoleActionMapping>();

                rolePermissionMappings = await _dbContext.RolePermissionMapping.Where(x => x.RoleID == roleID).ToListAsync();
                roleActionMappings = await _dbContext.RoleActionMapping.Where(x => x.RoleID == roleID).ToListAsync();

                List<Permission> permissions = await _dbContext
                    .Permission
                    .Where(p =>
                        rolePermissionMappings
                        .Select(p => p.PermissionID)
                        .Contains(p.PermissionID))
                    .ToListAsync();

                List<Actions> actions = await _dbContext
                    .Actions
                    .Where(p =>
                        roleActionMappings
                        .Select(p => p.ActionID)
                        .Contains(p.ActionID))
                    .ToListAsync();

                responseMessage.ResponseObj = new { permissions, actions };
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> SaveRolePermissionMapping(RolePermissionMapping rolePermissionMapping)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (rolePermissionMapping != null)
                {
                    RolePermissionMapping existRolePermissionMapping = await _dbContext
                        .RolePermissionMapping
                        .AsNoTracking()
                        .Where(x =>
                            x.RoleID == rolePermissionMapping.RoleID &&
                            x.PermissionID == rolePermissionMapping.PermissionID)
                        .FirstOrDefaultAsync();

                    if (existRolePermissionMapping != null)
                    {
                        List<RoleActionMapping> lstRoleAction = await _dbContext
                            .RoleActionMapping
                            .AsNoTracking()
                            .Where(x =>
                                x.RoleID == existRolePermissionMapping.RoleID &&
                                x.PermissionID == existRolePermissionMapping.PermissionID)
                            .ToListAsync();

                        if (lstRoleAction != null && lstRoleAction.Count > 0)
                        {
                            _dbContext.RoleActionMapping.RemoveRange(lstRoleAction);
                        }
                        _dbContext.RolePermissionMapping.Remove(existRolePermissionMapping);
                        _dbContext.SaveChanges();
                        responseMessage.Message = "Access Off";
                    }
                    else
                    {
                        _dbContext.RolePermissionMapping.Add(rolePermissionMapping);
                        _dbContext.SaveChanges();
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
