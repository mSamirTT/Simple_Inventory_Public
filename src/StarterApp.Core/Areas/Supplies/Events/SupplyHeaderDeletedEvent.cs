using MediatR;
using StarterApp.Application.Areas.SupplyArea.Queries;
using StarterApp.Core.Areas.Issues.Events;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Interfaces;
using StarterApp.Core.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Supplies.Events
{
    public class SupplyHeaderDeletedEvent : BaseEvent, INotification
    {
        public SupplyHeader Entity { get; private set; }
        public SupplyHeaderDeletedEvent(SupplyHeader supplyheader)
        {
            Entity = supplyheader;
        }
    }
    public class SupplyHeaderDeletedHandler : BaseHandler<SupplyHeader>, INotificationHandler<SupplyHeaderDeletedEvent>
    {
        public IRepository<Product> _productRepository { get; private set; }

        public SupplyHeaderDeletedHandler(IRepository<Product> productRepository) : base()
        {
            _productRepository = productRepository;
        }
        
        public async Task Handle(SupplyHeaderDeletedEvent @event, CancellationToken cancellationToken)
        {
            var removedItemsDictionary = @event.Entity.SupplyDetails
                .GroupBy(x => new { x.ProductId })
                .ToDictionary(x => x.Key.ProductId, x => x.Sum(y => y.Quantity));

            foreach (var item in @event.Entity.SupplyDetails)
            {
                var productAvailableQty = await _mediator.Send(new GetProductTotalQtyQuery(item.ProductId, @event.Entity.SupplyDate));
                removedItemsDictionary.TryGetValue(item.ProductId, out var removedItemQty);

                var netQty = productAvailableQty.TotalQuantity - removedItemQty;

                if (netQty < 0)
                {
                    var productName = (await _productRepository.GetById(item.ProductId)).Name;
                    var warningMessage = $"Not enough quantity for \"{productName}\" after deletion. Current available is {productAvailableQty.TotalQuantity}";

                    await _mediator.Publish(new WarningNotificationEvent(warningMessage)); 
                    throw new InsufficientProductQtyException(productName, productAvailableQty.TotalQuantity, netQty);
                }
            }
        }
    }
}
