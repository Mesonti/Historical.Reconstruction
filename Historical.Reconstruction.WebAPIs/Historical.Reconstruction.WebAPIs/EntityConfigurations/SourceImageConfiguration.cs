using Historical.Reconstruction.WebAPIs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Historical.Reconstruction.WebAPIs.EntityConfigurations
{
    public class SourceImageConfiguration : IEntityTypeConfiguration<SourceImageEntity>
    {
        public void Configure(EntityTypeBuilder<SourceImageEntity> builder)
        {
            builder.HasKey(si => si.Id);
            builder.Property(si => si.StorageKey).IsRequired().HasMaxLength(200);
            builder.Property(si => si.Caption).HasMaxLength(50);
        }
    }
}
