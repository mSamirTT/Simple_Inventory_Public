using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Areas.Products.Events;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Common;
using System.Collections.Generic;

namespace StarterApp.Core.Areas.Products.Entities
{
    public class Product : AuditableEntity, IAggregateRoot
    {
        #region Constructors

        private Product()
        {
            // Ef core requires empty constructor
        }

        public Product(string name, string description, double price, string thumbnail, long categoryId)
        {
            Name = name;
            Description = description;
            Price = price;
            Thumbnail = thumbnail;
            CategoryId = categoryId;
            AddDomainEvent(new ProductCreatedEvent(this));
        }

        #endregion

        #region Columns

        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Price { get; private set; }
        public string Thumbnail { get; private set; }
        public long CategoryId { get; private set; }

        #endregion

        #region Naviagtion Properties

        public Category Category { get; set; }
        public ICollection<SupplyDetail> SupplyDetails { get; set; }
        public ICollection<IssueDetail> IssueDetails { get; set; }

        #endregion
    }
}
