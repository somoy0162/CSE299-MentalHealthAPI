using MH.Common.DTO;
using MH.Common.Models;
using MH.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MH.API.Controllers
{
    [Route("api/Resources")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourcesService _resourcesService;
        public ResourcesController(IResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }

        [HttpGet("GetAllFiles")]
        public async Task<ResponseMessage> GetAllFiles()
        {
            return await _resourcesService.GetAllFiles();
        }

        [HttpGet("DownloadFile/{id}")]
        public async Task<FileResult> DownloadFile(int id)
        {
            var responseMessage = await _resourcesService.DownloadFile(id);
            var reports = (Resources)responseMessage.ResponseObj;
            byte[] bytes = (byte[])reports.File;

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reports.FileName);
        }

        [HttpGet("DeleteFile/{id}")]
        public async Task<ResponseMessage> DeleteFile(int id)
        {
            return await _resourcesService.DeleteFile(id);
        }
    }
}
