using Historical.Reconstruction.WebAPIs.DTOs.RegionDTOs;
using Historical.Reconstruction.WebAPIs.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Historical.Reconstruction.WebAPIs.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RegionResponse>>> GetRegions()
        {
            var results = await _regionService.GetRegions();
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegionResponse>> GetRegion(Guid id)
        {
            var result = await _regionService.GetRegionById(id);
            if (result == null)
                return NotFound();      
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<RegionResponse>> CreateRegion(RegionRequest request)
        {
            var result = await _regionService.CreateRegion(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RegionResponse>> UpdateRegion(Guid id, RegionRequest request)
        {
            var result = await _regionService.UpdateRegion(id, request);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteRegion(Guid id)
        {
            var result = await _regionService.DeleteRegion(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }
    }
}
