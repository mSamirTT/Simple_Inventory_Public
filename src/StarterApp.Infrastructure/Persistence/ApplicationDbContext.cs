using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using StarterApp.Core.Common.Interfaces;
using StarterApp.Infrastructure.Identity;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Common;
using StarterApp.Infrastructure.Common;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Areas.Dashboard.Entities;

namespace StarterApp.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IUnitOfWork
    {
        #region Private Feilds

        private readonly IDomainEventsDispatcher _domainEventsDispatcher;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        #endregion

        #region Constructor

        public ApplicationDbContext(
            DbContextOptions options,
            ICurrentUserService currentUserService,
            IDateTime dateTime,
            IDomainEventsDispatcher domainEventsDispatcher) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _domainEventsDispatcher = domainEventsDispatcher;
        }

        #endregion

        #region DBSet

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SupplyDetail> SupplyDetails { get; set; }
        public DbSet<SupplyHeader> SupplyHeaders { get; set; }
        public DbSet<IssueDetail> IssueDetails { get; set; }
        public DbSet<IssueHeader> IssueHeaders { get; set; }
        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<LogDashboard> LogDashboard { get; set; }

        #endregion

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Domain Events Dispatching
             await _domainEventsDispatcher.DispatchEventsAsync(this);

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = _dateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = _dateTime.Now;
                        BeginAuditModified(entry);
                        break;
                }
            }

            // Auditing
            var Added = ChangeTracker.Entries<AuditableEntity>().Where(p => p.State == EntityState.Added).ToList();
            var Deleted = ChangeTracker.Entries<AuditableEntity>().Where(p => p.State == EntityState.Deleted).ToList();
            Added.ForEach(e => BeginAuditAddedOrDeleted(e, EntityState.Added));
            Deleted.ForEach(e => BeginAuditAddedOrDeleted(e, EntityState.Deleted));

            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        #region Private Methods

        private void BeginAuditModified(EntityEntry entry)
        {
            foreach (var property in entry.Properties)
            {
                if (!property.IsModified)
                    continue;

                var auditEntry = new AuditTrail
                {
                    Table = entry.Entity.GetType().Name,
                    Column = property.Metadata.Name,
                    Action = entry.State.ToString(),
                    Value = property.OriginalValue?.ToString(),
                    NewValue = property.CurrentValue?.ToString(),
                    Date = DateTime.Now,
                    User = null // TODO
                };

                AuditTrail.Add(auditEntry);
            }
        }
        private void BeginAuditAddedOrDeleted(EntityEntry entry, EntityState action)
        {
            var auditEntry = new AuditTrail
            {
                Table = entry.Entity.GetType().Name,
                Column = action == EntityState.Added ? null :
                    entry.Metadata.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault(),
                Action = action.ToString(),
                Value = action == EntityState.Added ? null : (entry.Entity as BaseEntity).Id.ToString(),
                Date = DateTime.Now,
                User = null // TODO
            };
            AuditTrail.Add(auditEntry);
        }

        #endregion
    }
}
