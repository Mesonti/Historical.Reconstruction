using System.ComponentModel.DataAnnotations;

namespace Historical.Reconstruction.WebAPIs.DTOs.RegionDTOs
{
    public class RegionRequest
    {
        [Required(ErrorMessage = "Country cannot be empty")]
        public string Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
    }
}
