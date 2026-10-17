using Historical.Reconstruction.WebAPIs.DbContexts;
using Historical.Reconstruction.WebAPIs.DTOs;
using Historical.Reconstruction.WebAPIs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Historical.Reconstruction.WebAPIs.Services
{
    public class FindService : IFindService
    {
        private readonly FindsDbContext _dbContext;

        public FindService(FindsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FindResponse> CreateFind(CreateFindRequest request)
        {
            var entity = new FindEntity
            {
                Name = request.Name,
                RegionId = request.RegionId,
                FindCategoryId = request.FindCategoryId,
                DateFromYear = request.DateFromYear,
                DateToYear = request.DateToYear,
                Description = request.Description,
                SourceUrl = request.SourceUrl
            };
            await _dbContext.Sources.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return MapToFindResponse(entity);
        }

        public async Task<List<FindResponse>> GetAllFinds()
        {
            var finds = await _dbContext.Sources
                .AsNoTracking()
                .ToListAsync();
            return finds.Select(e => MapToFindResponse(e)).ToList();
        }

        public async Task<FindResponse?> GetFindById(Guid id)
        {
            var find = await _dbContext.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            if (find != null)
            {
                return MapToFindResponse(find);
            }
            else
            {
                return null;
            }
        }

        public async Task<FindResponse?> GetFindByName(string name)
        {
            var find = await _dbContext.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Name == name);
            if (find != null)
            {
                return MapToFindResponse(find);
            }
            else
            {
                return null;
            }
        }

        public async Task<FindResponse> UpdateFind(Guid id, UpdateFindRequest request)
        {
            var source = await _dbContext.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            if (source == null)
            {
                throw new InvalidOperationException($"Find with id {id} not found");
            }
            source.Name = request.Name;
            source.Description = request.Description;
            source.RegionId = request.RegionId;
            source.FindCategoryId = request.FindCategoryId;
            source.DateFromYear = request.DateFromYear;
            source.DateToYear = request.DateToYear;
            source.SourceUrl = request.SourceUrl;
            await _dbContext.SaveChangesAsync();
            return MapToFindResponse(source);
        }

        public async Task<bool> DeleteFind(Guid id)
        {
            var source = await _dbContext.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
            if (source == null)
            {
                throw new InvalidOperationException($"Find with id {id} not found");
            }
            _dbContext.Sources.Remove(source);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private static FindResponse MapToFindResponse(FindEntity entity)
        {
            return new FindResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                RegionId = entity.RegionId,
                FindCategoryId = entity.FindCategoryId,
                DateFromYear = entity.DateFromYear,
                DateToYear = entity.DateToYear,
                Description = entity.Description,
                SourceUrl = entity.SourceUrl
            };
        }
    }
}
