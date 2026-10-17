using Historical.Reconstruction.WebAPIs.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Historical.Reconstruction.WebAPIs.EntityConfigurations
{
    public class FindCategoryConfiguration : IEntityTypeConfiguration<FindCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<FindCategoryEntity> builder)
        {
            builder.HasKey(fc => fc.Id);

            builder
                .Property(fc => fc.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasOne(fc => fc.Parent)
                .WithMany(fc => fc.Children)
                .HasForeignKey(fc => fc.ParentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
