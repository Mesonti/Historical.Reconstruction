namespace Historical.Reconstruction.WebAPIs.Entities
{
    public class  FindCategoryEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public FindCategoryEntity? Parent { get; set; }
        public List<FindCategoryEntity> Children { get; set; } = new List<FindCategoryEntity>();
    }
}
