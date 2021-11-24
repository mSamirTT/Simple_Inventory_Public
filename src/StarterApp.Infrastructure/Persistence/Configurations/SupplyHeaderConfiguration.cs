using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApp.Core.Areas.Supplies.Entities;

namespace StarterApp.Infrastructure.Persistence.Configurations
{
    public class SupplyHeaderConfiguration : IEntityTypeConfiguration<SupplyHeader>
    {
        public void Configure(EntityTypeBuilder<SupplyHeader> builder)
        {
            builder.HasQueryFilter(t => !t.isLogicallyDeleted);
            builder.HasMany(x => x.SupplyDetails)
                .WithOne(x => x.SupplyHeader)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
