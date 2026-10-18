using System.ComponentModel.DataAnnotations;

namespace Historical.Reconstruction.WebAPIs.DTOs.CategoryDTOs
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}
