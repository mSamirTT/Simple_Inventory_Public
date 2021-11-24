using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace StarterApp.Core.Common
{
    public abstract class BaseEntity
    {
        private List<INotification> _domainEvents;
        public virtual long Id { get; set; }
        public virtual bool isLogicallyDeleted { get; set; }

        #region Public Methods

        public virtual void Delete()
        {
            //isLogicallyDeleted = true;
        }

        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents ??= new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected virtual ICollection<T> UpdateChildCollection<T>(ICollection<T> entityCollection, IEnumerable<T> newCollection) where T : BaseEntity
        {
            var createdDetails = newCollection.Where(x => x.Id <= 0);
            var deletedDetailIds = entityCollection.Select(x => x.Id).Except(newCollection.Select(x => x.Id));
            var updatedDetailId = newCollection.Select(x => x.Id).Intersect(entityCollection.Select(x => x.Id));

            // Filter deleted items
            entityCollection = entityCollection.Where(x => !deletedDetailIds.Contains(x.Id)).ToList();

            // Add created items
            foreach (var item in createdDetails)
            {
                entityCollection.Add(item);
            }


            // Remove then add Updated items
            foreach (var itemId in updatedDetailId)
            {
                var entityItem = entityCollection.First(x => x.Id == itemId);
                var newItem = newCollection.First(x => x.Id == itemId);

                entityCollection.Remove(entityItem);
                entityCollection.Add(newItem);
            }

            return entityCollection;
        }

        #endregion
    }
}
