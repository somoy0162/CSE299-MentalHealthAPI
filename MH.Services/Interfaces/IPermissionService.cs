using MH.Common.DTO;
using MH.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Interfaces
{
    public interface IPermissionService
    {
        Task<ResponseMessage> GetAllPermission();
        Task<ResponseMessage> GetAllPermissionActionByRoleID(int roleID);
      
        Task<ResponseMessage> SaveRolePermissionMapping(RolePermissionMapping rolePermissionMapping);

    }
}
