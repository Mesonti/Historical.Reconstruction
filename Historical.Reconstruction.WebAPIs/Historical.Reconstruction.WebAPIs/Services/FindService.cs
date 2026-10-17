using Historical.Reconstruction.WebAPIs.DbContexts;
using Historical.Reconstruction.WebAPIs.DTOs.FindDTOs;
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

        public async Task<PaginatedResponse<FindResponse>> GetAllFinds(PaginationParams paginationParams)
        {
            var query = _dbContext.Sources.AsNoTracking().AsQueryable();
            var totalCount = await query.CountAsync();

            var finds = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();
            var response = new PaginatedResponse<FindResponse>
            {
                Items = finds.Select(MapToFindResponse).ToList(),
                Pagination = new PaginationParams
                {
                    PageNumber = paginationParams.PageNumber,
                    PageSize = paginationParams.PageSize,
                    TotalCount = totalCount
                }
            };
            return response;
        }

        public async Task<FindResponse?> GetFindById(Guid id)
        {
            var find = await _dbContext.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (find != null)
                return MapToFindResponse(find);
            return null;
        }

        public async Task<FindResponse?> GetFindByName(string name)
        {
            var find = await _dbContext.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Name == name);

            if (find != null)
                return MapToFindResponse(find);
            return null;
        }

        public async Task<FindResponse?> UpdateFind(Guid id, UpdateFindRequest request)
        {
            var source = await _dbContext.Sources
                .FirstOrDefaultAsync(s => s.Id == id);
            if (source == null)
                return null;

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
                .FirstOrDefaultAsync(s => s.Id == id);

            if (source == null)
                return false;

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
