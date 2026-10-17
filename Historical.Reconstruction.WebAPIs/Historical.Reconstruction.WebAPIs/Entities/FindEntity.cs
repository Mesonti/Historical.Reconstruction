namespace Historical.Reconstruction.WebAPIs.Entities
{

    public class FindEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public Guid RegionId { get; set; }
        public RegionEntity Region { get; set; } = null!;
        public Guid FindCategoryId { get; set; }
        public FindCategoryEntity FindCategory { get; set; } = null!;
        public int DateFromYear { get; set; }
        public int DateToYear { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? SourceUrl { get; set; }
        public List<SourceImageEntity> Images { get; set; } = new List<SourceImageEntity>();
    }
}
