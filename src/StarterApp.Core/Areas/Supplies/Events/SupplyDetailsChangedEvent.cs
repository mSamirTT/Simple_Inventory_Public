using MediatR;
using Microsoft.EntityFrameworkCore;
using StarterApp.Application.Areas.SupplyArea.Queries;
using StarterApp.Core.Areas.Issues.Events;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Interfaces;
using StarterApp.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Supplies.Events
{
    public class SupplyDetailsChangedEvent : BaseEvent, INotification
    {
        public SupplyHeader Entity { get; private set; }
        public IEnumerable<SupplyDetail> NewCollection { get; private set; }
        public SupplyDetailsChangedEvent(SupplyHeader entity, 
            IEnumerable<SupplyDetail> newCollection)
        {
            Entity = entity;
            NewCollection = newCollection;
        }
    }
    public class CheckProductQtyWhenSupplyDetailsChangedEventHandler : BaseHandler<SupplyHeader>, INotificationHandler<SupplyDetailsChangedEvent>
    {
        public IRepository<Product> _productRepository { get; private set; }
        public IRepository<SupplyHeader> _supplyHeaderRepository { get; private set; }

        public CheckProductQtyWhenSupplyDetailsChangedEventHandler(IRepository<Product> productRepository,
            IRepository<SupplyHeader> supplyHeaderRepository) : base()
        {
            _productRepository = productRepository;
            _supplyHeaderRepository = supplyHeaderRepository;
        }
        
        public async Task Handle(SupplyDetailsChangedEvent @event, CancellationToken cancellationToken)
        {
            var existingItems = (await _supplyHeaderRepository.Query
                            .Include(x => x.SupplyDetails)
                            .FirstOrDefaultAsync(x => x.Id == @event.Entity.Id))
                            .SupplyDetails;

            var removedItemsDictionary = @event.Entity.SupplyDetails
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
                var productAvailableQty = await _mediator.Send(new GetProductTotalQtyQuery(item.ProductId, @event.Entity.SupplyDate));
                removedItemsDictionary.TryGetValue(item.ProductId, out var removedItemQty);
                createdItemsDictionary.TryGetValue(item.ProductId, out var createdItemQty);
                updatedItemsDictionary.TryGetValue(item.ProductId, out var updatedItemQty);
                previousItemsDictionary.TryGetValue(item.ProductId, out var previousItemQty);

                var netQty = productAvailableQty.TotalQuantity +
                    (createdItemQty + updatedItemQty - previousItemQty) -
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
