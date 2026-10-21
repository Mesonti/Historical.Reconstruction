using System.ComponentModel.DataAnnotations;

namespace Historical.Reconstruction.WebAPIs.DTOs.FindDTOs;

public class UploadFindImageRequest
{
    [Required]
    public IFormFile File { get; set; } = null!;

    [MaxLength(50)]
    public string? Caption { get; set; }
}
