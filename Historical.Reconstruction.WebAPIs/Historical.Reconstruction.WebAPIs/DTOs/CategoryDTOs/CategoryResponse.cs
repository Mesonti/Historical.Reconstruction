namespace Historical.Reconstruction.WebAPIs.DTOs.CategoryDTOs
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }
}
