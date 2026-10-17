using Historical.Reconstruction.WebAPIs.DbContexts;
using Historical.Reconstruction.WebAPIs.DTOs.RegionDTOs;
using Historical.Reconstruction.WebAPIs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Historical.Reconstruction.WebAPIs.Services
{
    public class RegionService : IRegionService
    {
        private readonly FindsDbContext _dbContext;

        public RegionService(FindsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<RegionResponse> CreateRegion(RegionRequest request)
        {
            var entity = new RegionEntity
            {
                Country = request.Country,
                Region = request.Region,
                City = request.City
            };
            _dbContext.Regions.Add(entity);
            await _dbContext.SaveChangesAsync();
            return MapToResponse(entity);
        }

        public async Task<RegionResponse?> GetRegionById(Guid id)
        {
            var region = await _dbContext.Regions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            if (region != null)
                return MapToResponse(region);
            return null;
        }

        public async Task<List<RegionResponse>> GetRegions()
        {
            var regions = await _dbContext.Regions
                .AsNoTracking()
                .ToListAsync();
            return regions.Select(MapToResponse).ToList();
        }

        public async Task<RegionResponse?> UpdateRegion(Guid id, RegionRequest request)
        {
            var entity = await _dbContext.Regions.FirstOrDefaultAsync(r=>r.Id == id);
            if (entity == null)
                return null;

            entity.Country = request.Country;
            entity.Region = request.Region;
            entity.City = request.City;

            _dbContext.Regions.Update(entity);
            await _dbContext.SaveChangesAsync();
            return MapToResponse(entity);
        }

        public async Task<bool> DeleteRegion(Guid id)
        {
            var entity = await _dbContext.Regions.FirstOrDefaultAsync(r=>r.Id == id);
            if (entity == null)
                return false;

            _dbContext.Regions.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private static RegionResponse MapToResponse(RegionEntity entity)
        {
            return new RegionResponse
            {
                Id = entity.Id,
                Country = entity.Country,
                Region = entity.Region,
                City = entity.City,
            };
        }
    }
}
