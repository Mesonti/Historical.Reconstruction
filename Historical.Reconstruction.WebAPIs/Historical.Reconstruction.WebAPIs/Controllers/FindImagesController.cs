using Historical.Reconstruction.WebAPIs.DTOs.FindDTOs;
using Historical.Reconstruction.WebAPIs.Services;
using Microsoft.AspNetCore.Mvc;

namespace Historical.Reconstruction.WebAPIs.Controllers;

[ApiController]
[Route("api/finds/{findId:guid}/images")]
public class FindImagesController(IFindImageService imageService) : ControllerBase
{
    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<ActionResult<FindImageResponse>> Upload(
        Guid findId,
        [FromForm] UploadFindImageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await imageService.Upload(findId, request, cancellationToken);
        return result.Error switch
        {
            UploadFindImageError.FindNotFound => NotFound(result.Message),
            UploadFindImageError.InvalidFile => BadRequest(result.Message),
            _ => Created(result.Image!.Url, result.Image)
        };
    }
}
