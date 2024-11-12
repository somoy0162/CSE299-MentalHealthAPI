using MH.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.Services.Interfaces
{
    public interface IResourcesService
    {
        Task<ResponseMessage> GetAllFiles ();
        Task<ResponseMessage> DownloadFile(int id);
        Task<ResponseMessage> DeleteFile(int id);
    }
}
