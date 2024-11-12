using MH.Common.Constants;
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
    public class ResourcesService : IResourcesService
    {
        private readonly MHDbContext _dbContext;
        public ResourcesService(MHDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseMessage> GetAllFiles()
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var data = await _dbContext
                    .Resources
                    .Select(x => new
                    {
                        ID = x.ID,
                        FileName = x.FileName,
                        DateInserted = x.DateUploaded
                    })
                    .ToListAsync();

                responseMessage.ResponseObj = data;
                responseMessage.TotalCount = data.Count();
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> DownloadFile(int id)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                responseMessage.ResponseObj = await _dbContext
                    .Resources
                    .Where(x => x.ID == id)
                    .Select(s => new Resources() { FileName = s.FileName, File = s.File })
                    .FirstOrDefaultAsync();
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Success;
            }
            catch (Exception ex)
            {
                responseMessage.Message = ex.Message;
                responseMessage.ResponseCode = (int)Enums.ResponseCode.Failed;
            }
            return responseMessage;
        }

        public async Task<ResponseMessage> DeleteFile(int id)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                var existingFile = await _dbContext
                    .Resources
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ID == id);

                _dbContext.Resources.Remove(existingFile);
                await _dbContext.SaveChangesAsync();

                responseMessage.Message = MessageConstant.DeleteSuccess;
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
