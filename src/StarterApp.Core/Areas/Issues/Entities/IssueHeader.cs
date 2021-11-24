using StarterApp.Core.Areas.Issues.Events;
using StarterApp.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarterApp.Core.Areas.Issues.Entities
{
    public class IssueHeader : AuditableEntity, IAggregateRoot
    {
        #region Constructors

        public IssueHeader()
        {
            // Ef core requires empty constructor
        }
        public IssueHeader(int transNum, string notes, DateTime issueDate, ICollection<IssueDetail> issueDetails)
        {
            TransactionNumber = transNum;
            Notes = notes;
            IssueDate = issueDate;
            IssueDetails = issueDetails;
            AddDomainEvent(new IssueHeaderCreatedEvent(this));
        }

        #endregion

        #region Columns

        public int TransactionNumber { get; private set; }
        public string Notes { get; private set; }
        public DateTime IssueDate { get; private set; }

        #endregion

        #region Naviagtion Properties

        public ICollection<IssueDetail> IssueDetails { get; private set; }

        #endregion

        #region Domain Methods

        public void AddIssueDetail(long productId, int qty)
        {
            IssueDetails.Add(new IssueDetail(productId, qty));
        }

        public void UpdateChildCollection(IEnumerable<IssueDetail> newCollection)
        {
            var createdDetails = newCollection.Where(x => x.Id == 0);
            var deletedDetails = IssueDetails.Where(x => !newCollection.Select(x => x.Id).Contains(x.Id));

            // Deleted items
            foreach (var item in deletedDetails)
            {
                IssueDetails.Remove(item);
            }

            // Added items
            foreach (var item in createdDetails)
            {
                IssueDetails.Add(item);
            }

            // Updated items
            foreach (var item in newCollection.Where(x => x.Id > 0))
            {
                IssueDetails
                    .First(x => x.Id == item.Id)
                    .Update(item.ProductId, item.Quantity);
            }

            AddDomainEvent(new IssueDetailsChangedEvent(this, newCollection));
        }

        #endregion
    }
}
