using Historical.Reconstruction.WebAPIs.DTOs.FindDTOs;

namespace Historical.Reconstruction.WebAPIs.Services;

public enum UploadFindImageError
{
    None,
    FindNotFound,
    InvalidFile
}

public record UploadFindImageResult(
    FindImageResponse? Image,
    UploadFindImageError Error = UploadFindImageError.None,
    string? Message = null);
