using Historical.Reconstruction.WebAPIs.DTOs.CategoryDTOs;
using Historical.Reconstruction.WebAPIs.Services;
using Microsoft.AspNetCore.Mvc;

namespace Historical.Reconstruction.WebAPIs.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> CreateCategory(CreateCategoryRequest request)
        {
            var category = await _categoryService.CreateCategory(request);
            return Ok(category);
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponse>>> GetCategories()
        {
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponse?>> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponse?>> UpdateCategory(Guid id, UpdateCategoryRequest request)
        {
            var category = await _categoryService.UpdateCategory(id, request);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCategory(Guid id)
        {
            var result = await _categoryService.DeleteCategory(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }
    }
}
