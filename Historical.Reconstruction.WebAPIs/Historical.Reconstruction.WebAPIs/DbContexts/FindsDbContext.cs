using Historical.Reconstruction.WebAPIs.Entities;
using Microsoft.EntityFrameworkCore;

namespace Historical.Reconstruction.WebAPIs.DbContexts
{
    public class FindsDbContext : DbContext
    {
        public DbSet<FindEntity> Sources { get; set; }

        public DbSet<RegionEntity> Regions { get; set; }

        public DbSet<FindCategoryEntity> SourceCategories { get; set; }

        public DbSet<SourceImageEntity> SourceImages { get; set; }

        public FindsDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
