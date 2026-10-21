namespace Historical.Reconstruction.WebAPIs.DTOs.FindDTOs
{
    public class FindResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RegionId { get; set; }
        public Guid FindCategoryId { get; set; }
        public int DateFromYear { get; set; }
        public int DateToYear { get; set; }
        public string Description { get; set; }
        public string? SourceUrl { get; set; }
        public List<FindImageResponse> Images { get; set; } = new();
    }
}
