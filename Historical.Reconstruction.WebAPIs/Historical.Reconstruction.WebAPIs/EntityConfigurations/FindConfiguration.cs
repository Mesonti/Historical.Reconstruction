using Historical.Reconstruction.WebAPIs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Historical.Reconstruction.WebAPIs.EntityConfigurations
{
    public class FindConfiguration : IEntityTypeConfiguration<FindEntity>
    {
        public void Configure(EntityTypeBuilder<FindEntity> builder)
        {
            builder.HasKey(find => find.Id);

            builder
                .Property(find => find.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(find => find.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder
                .Property(find => find.SourceUrl)
                .HasMaxLength(300);

            builder
                .HasOne(find => find.Region)
                .WithMany(region => region.Finds)
                .HasForeignKey(find => find.RegionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(find => find.FindCategory)
                .WithMany()
                .HasForeignKey(find => find.FindCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(find => find.Images)
                .WithOne()
                .HasForeignKey(image => image.FindId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
