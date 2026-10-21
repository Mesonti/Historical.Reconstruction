using Historical.Reconstruction.WebAPIs.DbContexts;
using Historical.Reconstruction.WebAPIs.DTOs.FindDTOs;
using Historical.Reconstruction.WebAPIs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Historical.Reconstruction.WebAPIs.Services;

public class FindImageService(
    FindsDbContext dbContext,
    IWebHostEnvironment environment,
    ILogger<FindImageService> logger) : IFindImageService
{
    public async Task<UploadFindImageResult> Upload(
        Guid findId,
        UploadFindImageRequest request,
        CancellationToken cancellationToken)
    {
        if (!await dbContext.Sources.AnyAsync(f => f.Id == findId, cancellationToken))
            return new(null, UploadFindImageError.FindNotFound, "Находка не найдена.");

        if (request.File.Length == 0)
            return new(null, UploadFindImageError.InvalidFile, "Файл пуст.");

        if (request.File.Length > 5 * 1024 * 1024)
            return new(null, UploadFindImageError.InvalidFile, "Максимальный размер картинки — 5 МБ.");

        await using var input = request.File.OpenReadStream();
        var header = new byte[8];
        var read = await input.ReadAtLeastAsync(header, header.Length,
            throwOnEndOfStream: false, cancellationToken: cancellationToken);
        var jpeg = read >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF;
        var png = read == 8 && header.AsSpan().SequenceEqual(
            new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });

        if (!jpeg && !png)
            return new(null, UploadFindImageError.InvalidFile, "Допустимы только JPEG и PNG.");

        var fileName = $"{Guid.NewGuid():N}{(jpeg ? ".jpg" : ".png")}";
        var directory = Path.Combine(environment.WebRootPath, "uploads");
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, fileName);
        var maxOrder = await dbContext.SourceImages.Where(i => i.FindId == findId)
            .MaxAsync(i => (int?)i.SortOrder, cancellationToken);
        var image = new SourceImageEntity
        {
            FindId = findId,
            StorageKey = $"/uploads/{fileName}",
            Caption = request.Caption?.Trim(),
            SortOrder = (maxOrder ?? -1) + 1
        };

        var fileCreated = false;
        try
        {
            await using (var output = new FileStream(path, FileMode.CreateNew,
                FileAccess.Write, FileShare.None, 81920, useAsync: true))
            {
                fileCreated = true;
                await output.WriteAsync(header.AsMemory(0, read), cancellationToken);
                await input.CopyToAsync(output, cancellationToken);
            }

            dbContext.SourceImages.Add(image);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            if (fileCreated)
            {
                try { System.IO.File.Delete(path); }
                catch (Exception cleanupError)
                {
                    logger.LogError(cleanupError, "Не удалось удалить файл {Path}", path);
                }
            }
            throw;
        }

        return new(new FindImageResponse
        {
            Id = image.Id,
            Url = image.StorageKey,
            Caption = image.Caption,
            SortOrder = image.SortOrder
        });
    }
}

