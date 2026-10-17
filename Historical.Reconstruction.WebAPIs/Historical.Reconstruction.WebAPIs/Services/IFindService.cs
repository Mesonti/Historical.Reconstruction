using Historical.Reconstruction.WebAPIs.DTOs;
using Historical.Reconstruction.WebAPIs.Entities;

namespace Historical.Reconstruction.WebAPIs.Services
{
    public interface IFindService
    {
        public Task<FindResponse> CreateFind(CreateFindRequest request);
        public Task<List<FindResponse>> GetAllFinds();
        public Task<FindResponse?> GetFindById(Guid id);
        public Task<FindResponse?> GetFindByName(string name);
        public Task<FindResponse> UpdateFind(Guid id, UpdateFindRequest request);
        public Task<bool> DeleteFind(Guid id);
    }
}
