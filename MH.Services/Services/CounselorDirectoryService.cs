using MH.Common.DTO;
using MH.Common.Enums;
using MH.DataAccess;
using MH.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Services
{
    public class CounselorDirectoryService : ICounselorDirectoryService
    {
        private readonly MHDbContext _dbContext;

        public CounselorDirectoryService(MHDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseMessage> GetAllCounselor()
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var counselors = await _dbContext
                    .SystemUsers
                    .Select(x => new
                    {
                        x.ID,
                        x.Name,
                        x.Email,
                        x.PhoneNumber,
                        x.Role,
                        x.Gender,
                        RoleName = _dbContext.Role
                                    .Where(r => r.RoleID == x.Role)
                                    .Select(r => r.RoleName)
                                    .FirstOrDefault()
                    })
                    .Where(x => x.Role == (int)Enums.UserType.Counselor)
                    .ToListAsync();

                responseMessage.ResponseObj = counselors;
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
