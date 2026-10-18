using Historical.Reconstruction.WebAPIs.DTOs.CategoryDTOs;

namespace Historical.Reconstruction.WebAPIs.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse> CreateCategory(CreateCategoryRequest request);
        Task<List<CategoryResponse>> GetCategories();
        Task<CategoryResponse?> GetCategoryById(Guid id);
        Task<CategoryResponse?> UpdateCategory(Guid id, UpdateCategoryRequest request);
        Task<bool> DeleteCategory(Guid id);
    }
}
