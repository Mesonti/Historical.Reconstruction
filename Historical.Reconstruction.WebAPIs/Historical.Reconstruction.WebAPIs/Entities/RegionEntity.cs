namespace Historical.Reconstruction.WebAPIs.Entities
{
    public class RegionEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Country { get; set; } = string.Empty;
        public string? Region { get; set; }
        public string? City { get; set; }
        public List<FindEntity> Finds { get; set; } = new List<FindEntity>();
    }
}
