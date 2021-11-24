using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarterApp.Core.Areas.Issues.Entities;

namespace StarterApp.Infrastructure.Persistence.Configurations
{
    public class IssueHeaderConfiguration : IEntityTypeConfiguration<IssueHeader>
    {
        public void Configure(EntityTypeBuilder<IssueHeader> builder)
        {
            builder.HasQueryFilter(t => !t.isLogicallyDeleted);
            builder.HasMany(x => x.IssueDetails)
                .WithOne(x => x.IssueHeader)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
