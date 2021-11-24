using MediatR;
using Microsoft.Extensions.Logging;
using StarterApp.Core.Common;
using StarterApp.Infrastructure.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StarterApp.Infrastructure.Common
{
    public class DomainEventsDispatcher : IDomainEventsDispatcher
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        #endregion

        #region Constructor

        public DomainEventsDispatcher(ILogger<DomainEventsDispatcher> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        #endregion

        #region IDomainEventDispatcher

        public async Task DispatchEventsAsync(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

            var domainEntities = dbContext.ChangeTracker.Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents).ToList();

            while (domainEvents.Any())
            {
                domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

                var tasks = domainEvents.Select(async domainEvent =>
                {
                    await _mediator.Publish(domainEvent);
                });

                await Task.WhenAll(tasks);

                domainEntities = dbContext.ChangeTracker.Entries<BaseEntity>()
                    .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

                domainEvents = domainEntities
                    .SelectMany(x => x.Entity.DomainEvents).ToList();
            }
        }

        #endregion
    }
}