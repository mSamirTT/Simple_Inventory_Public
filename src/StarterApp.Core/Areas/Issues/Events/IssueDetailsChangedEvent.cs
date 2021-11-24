using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Application.Areas.SupplyArea.Queries;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Interfaces;
using StarterApp.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Issues.Events
{
    public class IssueDetailsChangedEvent : BaseEvent, INotification
    {
        public IssueHeader Entity { get; private set; }
        public IEnumerable<IssueDetail> NewCollection { get; private set; }

        public IssueDetailsChangedEvent(IssueHeader entity, 
            IEnumerable<IssueDetail> newCollection)
        {
            Entity = entity;
            NewCollection = newCollection;
        }
    }
    public class CheckProductQtyWhenIssueDetailsChangedEventHandler : BaseHandler<IssueHeader>, INotificationHandler<IssueDetailsChangedEvent>
    {
        public IRepository<Product> _productRepository { get; private set; }
        public IRepository<IssueHeader> _issueHeaderRepository { get; private set; }
        public CheckProductQtyWhenIssueDetailsChangedEventHandler(IRepository<Product> productRepository,
            IRepository<IssueHeader> issueHeaderRepository) : base()
        {
            _productRepository = productRepository;
            _issueHeaderRepository = issueHeaderRepository;
        }
        public async Task Handle(IssueDetailsChangedEvent @event, CancellationToken cancellationToken)
        {
            var existingItems = (await _issueHeaderRepository.Query
                .Include(x => x.IssueDetails)
                .FirstOrDefaultAsync(x => x.Id == @event.Entity.Id))
                .IssueDetails;

            var removedItemsDictionary = @event.Entity.IssueDetails
                .Where(x => !@event.NewCollection.Select(x => x.Id).Contains(x.Id))
                .GroupBy(x => new { x.ProductId })
                .ToDictionary(x => x.Key.ProductId, x => x.Sum(y => y.Quantity));

            var createdItemsDictionary = @event.NewCollection
                .Where(x => x.Id == 0)
                .GroupBy(x => new { x.ProductId })
                .ToDictionary(x => x.Key.ProductId, x => x.Sum(y => y.Quantity));

            var updatedItemsDictionary = @event.NewCollection
                .Where(x => x.Id != 0)
                .GroupBy(x => new { x.ProductId })
                .ToDictionary(x => x.Key.ProductId, x => x.Sum(y => y.Quantity));

            var previousItemsDictionary = existingItems
                .GroupBy(x => new { x.ProductId })
                .ToDictionary(x => x.Key.ProductId, x => x.Sum(y => y.Quantity));

            foreach (var item in @event.NewCollection)
            {
                var productAvailableQty = await _mediator.Send(new GetProductTotalQtyQuery(item.ProductId, @event.Entity.IssueDate));
                removedItemsDictionary.TryGetValue(item.ProductId, out var removedItemQty);
                createdItemsDictionary.TryGetValue(item.ProductId, out var createdItemQty);
                updatedItemsDictionary.TryGetValue(item.ProductId, out var updatedItemQty);
                previousItemsDictionary.TryGetValue(item.ProductId, out var previousItemQty);

                var netQty = productAvailableQty.TotalQuantity -
                    (createdItemQty + updatedItemQty - previousItemQty) +
                    removedItemQty;

                if (netQty < 0)
                {
                    var productName = (await _productRepository.GetById(item.ProductId)).Name;
                    var warningMessage = $"Not enough quantity for \"{productName}\". Current available is {productAvailableQty.TotalQuantity}";

                    await _mediator.Publish(new WarningNotificationEvent(warningMessage)); 
                    throw new InsufficientProductQtyException(productName, productAvailableQty.TotalQuantity, netQty);
                }
            }
        }
    }
}
