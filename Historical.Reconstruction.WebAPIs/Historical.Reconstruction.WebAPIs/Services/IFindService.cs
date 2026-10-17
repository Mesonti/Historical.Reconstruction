using Historical.Reconstruction.WebAPIs.DTOs.FindDTOs;

namespace Historical.Reconstruction.WebAPIs.Services
{
    public interface IFindService
    {
        Task<FindResponse> CreateFind(CreateFindRequest request);
        Task<PaginatedResponse<FindResponse>> GetAllFinds(PaginationParams paginationParams);
        Task<FindResponse?> GetFindById(Guid id);
        Task<FindResponse?> GetFindByName(string name);
        Task<FindResponse> UpdateFind(Guid id, UpdateFindRequest request);
        Task<bool> DeleteFind(Guid id);
    }
}
