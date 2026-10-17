using Historical.Reconstruction.WebAPIs.DTOs.FindDTOs;
using Historical.Reconstruction.WebAPIs.Services;
using Microsoft.AspNetCore.Mvc;

namespace Historical.Reconstruction.WebAPIs.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FindController : ControllerBase
    {
        private readonly IFindService _findService;

        public FindController(IFindService findService)
        {
            _findService = findService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<FindResponse>>> GetFinds([FromQuery] PaginationParams paginationParams)
        {
            var finds = await _findService.GetAllFinds(paginationParams);
            return Ok(finds);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FindResponse>> GetFindById(Guid id)
        {
            var find = await _findService.GetFindById(id);
            if (find == null)
                return NotFound();
            return Ok(find);
        }

        [HttpGet("search/{name}")]
        public async Task<ActionResult<FindResponse>> GetFindByName(string name)
        {
            var find = await _findService.GetFindByName(name);
            if (find == null)
                return NotFound();
            return Ok(find);
        }

        [HttpPost]
        public async Task<ActionResult<FindResponse>> CreateFind(CreateFindRequest request)
        {
            var find = await _findService.CreateFind(request);
            return Ok(find);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FindResponse>> UpdateFind(Guid id, UpdateFindRequest request)
        {
            var find = await _findService.UpdateFind(id, request);
            if (find == null)
                return NotFound();
            return Ok(find);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteFind(Guid id)
        {
            var result = await _findService.DeleteFind(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }
    }
}
