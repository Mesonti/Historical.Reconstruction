namespace Historical.Reconstruction.WebAPIs.DTOs.FindDTOs;

public class FindImageResponse
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string? Caption { get; set; }
}
