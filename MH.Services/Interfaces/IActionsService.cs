using MH.Common.DTO;
using MH.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Interfaces
{
    public interface IActionsService
    {
        Task<ResponseMessage> SaveRoleActionMapping(RoleActionMapping roleActionMapping);
    }
}
