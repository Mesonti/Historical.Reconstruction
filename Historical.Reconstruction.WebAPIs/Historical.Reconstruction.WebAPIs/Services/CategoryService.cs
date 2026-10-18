using Historical.Reconstruction.WebAPIs.DbContexts;
using Historical.Reconstruction.WebAPIs.DTOs.CategoryDTOs;
using Historical.Reconstruction.WebAPIs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Historical.Reconstruction.WebAPIs.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly FindsDbContext _dbContext;
        public CategoryService(FindsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CategoryResponse> CreateCategory(CreateCategoryRequest request)
        {
            if (request.ParentId.HasValue && !await _dbContext.FindCategories.AnyAsync(c => c.Id == request.ParentId.Value))
            {
                throw new InvalidOperationException("Parent category not found");
            }

            var category = new FindCategoryEntity
            {
                Name = request.Name,
                ParentId = request.ParentId
            };

            _dbContext.FindCategories.Add(category);
            await _dbContext.SaveChangesAsync();
            return MapToCategoryResponse(category);
        }

        public async Task<bool> DeleteCategory(Guid id)
        {
            var category = await _dbContext.FindCategories
            .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return false;

            var hasChildren = await _dbContext.FindCategories
                .AnyAsync(c => c.ParentId == id);
            if (hasChildren)
            {
                throw new InvalidOperationException("Cannot delete category with children");
            }

            _dbContext.FindCategories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<CategoryResponse>> GetCategories()
        {
            return await _dbContext.FindCategories
                .AsNoTracking()
                .Select(c => MapToCategoryResponse(c))
                .ToListAsync();
        }

        public async Task<CategoryResponse?> GetCategoryById(Guid id)
        {
            var category = await _dbContext.FindCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            if(category == null) 
                return null;   
            return MapToCategoryResponse(category);
        }

        public async Task<CategoryResponse?> UpdateCategory(Guid id, UpdateCategoryRequest request)
        {
            var category = await _dbContext.FindCategories.FirstOrDefaultAsync(c => c.Id == id);
            if(category == null)
            {
                return null;
            }
            category.Name = request.Name;
            category.ParentId = request.ParentId;

            await _dbContext.SaveChangesAsync();
            return MapToCategoryResponse(category);
        }

        private static CategoryResponse MapToCategoryResponse(FindCategoryEntity category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId
            };
        }
    }
}
