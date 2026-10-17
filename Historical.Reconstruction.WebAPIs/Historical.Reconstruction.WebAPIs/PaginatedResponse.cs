namespace Historical.Reconstruction.WebAPIs
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public PaginationParams Pagination { get; set; } = new PaginationParams();
    }
}
