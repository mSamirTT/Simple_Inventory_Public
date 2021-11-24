using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApp.Core.Areas.Products.Entities;

namespace StarterApp.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasQueryFilter(t => !t.isLogicallyDeleted);
            builder.HasMany(x => x.SupplyDetails)
                .WithOne(x => x.Product)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.IssueDetails)
                .WithOne(x => x.Product)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
