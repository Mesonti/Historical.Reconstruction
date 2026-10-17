namespace Historical.Reconstruction.WebAPIs.DTOs.RegionDTOs
{
    public class RegionResponse
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
    }
}
