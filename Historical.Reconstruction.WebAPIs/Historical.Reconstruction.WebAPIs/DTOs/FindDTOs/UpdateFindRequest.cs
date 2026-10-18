using System.ComponentModel.DataAnnotations;

namespace Historical.Reconstruction.WebAPIs.DTOs.FindDTOs
{
    public class UpdateFindRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Region ID is required")]
        public Guid RegionId { get; set; }

        [Required(ErrorMessage = "Find category ID is required")]
        public Guid FindCategoryId { get; set; }

        public int DateFromYear { get; set; }

        public int DateToYear { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string? SourceUrl { get; set; }
    }
}
