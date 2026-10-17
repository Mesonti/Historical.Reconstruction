using Historical.Reconstruction.WebAPIs.DTOs.RegionDTOs;

namespace Historical.Reconstruction.WebAPIs.Services
{
    public interface IRegionService
    {
        Task<List<RegionResponse>> GetRegions();
        Task<RegionResponse?> GetRegionById(Guid id);
        Task<RegionResponse> CreateRegion(RegionRequest request);
        Task<RegionResponse?> UpdateRegion(Guid id, RegionRequest request);
        Task<bool> DeleteRegion(Guid id);
    }
}
