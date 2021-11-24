using StarterApp.Core.Areas.Supplies.Events;
using StarterApp.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarterApp.Core.Areas.Supplies.Entities
{
    public class SupplyHeader : AuditableEntity, IAggregateRoot
    {
        #region Constructors

        public SupplyHeader()
        {
            // Ef core requires empty constructor
        }
        public SupplyHeader(int transNum, string notes, DateTime supplyDate, ICollection<SupplyDetail> supplyDetails)
        {
            TransactionNumber = transNum;
            Notes = notes;
            SupplyDate = supplyDate;
            SupplyDetails = supplyDetails; 
            AddDomainEvent(new SupplyHeaderCreatedEvent(this));
        }

        #endregion

        #region Columns

        public int TransactionNumber { get; private set; }
        public string Notes { get; private set; }
        public DateTime SupplyDate { get; private set; }

        #endregion

        #region Naviagtion Properties

        public ICollection<SupplyDetail> SupplyDetails { get; private set; }

        #endregion

        #region Domain Methods

        public void AddSupplyDetail(long productId, int qty)
        {
            SupplyDetails.Add(new SupplyDetail(productId, qty));
        }

        public void UpdateChildCollection(ICollection<SupplyDetail> entityCollection, IEnumerable<SupplyDetail> newCollection)
        {
            var createdDetails = newCollection.Where(x => x.Id == 0);
            var deletedDetails = SupplyDetails.Where(x => !newCollection.Select(x => x.Id).Contains(x.Id));

            // Deleted items
            foreach (var item in deletedDetails)
            {
                SupplyDetails.Remove(item);
            }

            // Added items
            foreach (var item in createdDetails)
            {
                SupplyDetails.Add(item);
            }

            // Updated items
            foreach (var item in newCollection.Where(x => x.Id > 0))
            {
                SupplyDetails.First(x => x.Id == item.Id).Update(item.ProductId, item.Quantity);
            }

            AddDomainEvent(new SupplyDetailsChangedEvent(this, newCollection));
        }

        public override void Delete()
        {
            base.Delete();
            AddDomainEvent(new SupplyHeaderDeletedEvent(this));
        }

        #endregion
    }
}
