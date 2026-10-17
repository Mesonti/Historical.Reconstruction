namespace Historical.Reconstruction.WebAPIs.Entities
{
    public class SourceImageEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FindId { get; set; }
        public string StorageKey { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public string? Caption { get; set; }
    }
}
