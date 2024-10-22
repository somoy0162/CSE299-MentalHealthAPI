using MH.Common.DTO;
using MH.Services.Interfaces;
using MH.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MH.API.Controllers
{
    [Route("api/counselorDirectory")]
    [ApiController]
    public class CounselorDirectoryController : ControllerBase
    {
        private readonly ICounselorDirectoryService _counselorDirectoryService;

        public CounselorDirectoryController(ICounselorDirectoryService counselorDirectoryService)
        {
            _counselorDirectoryService = counselorDirectoryService;
        }

        [HttpGet("GetAllCounselor")]
        public async Task<ResponseMessage> GetAllCounselor()
        {
            return await _counselorDirectoryService.GetAllCounselor();
        }
    }
}
