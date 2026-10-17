using Historical.Reconstruction.WebAPIs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Historical.Reconstruction.WebAPIs.EntityConfigurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<RegionEntity>
    {
        public void Configure(EntityTypeBuilder<RegionEntity> builder)
        {

            builder.HasKey(r => r.Id);
            builder
                .Property(r => r.Country)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(r => r.Region)
                .HasMaxLength(50);

            builder
                .Property(r => r.City)
                .HasMaxLength(50);
        }
    }
}
