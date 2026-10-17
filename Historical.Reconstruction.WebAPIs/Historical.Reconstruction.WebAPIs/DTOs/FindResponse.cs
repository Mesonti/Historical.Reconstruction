namespace Historical.Reconstruction.WebAPIs.DTOs
{
    public class FindResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid RegionId { get; set; }
        public Guid FindCategoryId { get; set; }
        public int DateFromYear { get; set; }
        public int DateToYear { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? SourceUrl { get; set; }
    }
}
