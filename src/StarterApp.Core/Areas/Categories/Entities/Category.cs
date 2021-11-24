using StarterApp.Core.Areas.Categories.Events;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common;
using System.Collections.Generic;

namespace StarterApp.Core.Areas.Categories.Entities
{
    public class Category : AuditableEntity, IAggregateRoot
    {
        #region Constructors

        private Category()
        {
            // Ef core requires empty constructor
        }

        public Category(string name, string thumbnail)
        {
            Name = name;
            Thumbnail = thumbnail;
            AddDomainEvent(new CategoryCreatedEvent(this));
        }

        #endregion

        #region Columns

        public string Name { get; private set; }
        public string Thumbnail { get; private set; }
        public ICollection<Product> Products { get; private set; }

        #endregion
    }
}
