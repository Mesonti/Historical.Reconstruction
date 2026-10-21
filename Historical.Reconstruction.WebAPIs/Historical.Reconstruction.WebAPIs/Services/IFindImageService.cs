using Historical.Reconstruction.WebAPIs.DTOs.FindDTOs;

namespace Historical.Reconstruction.WebAPIs.Services;

public interface IFindImageService
{
    Task<UploadFindImageResult> Upload(
        Guid findId,
        UploadFindImageRequest request,
        CancellationToken cancellationToken = default);
}
