using MediatR;
using StarterApp.Application.Areas.SupplyArea.Queries;
using StarterApp.Core.Areas.Issues.Entities;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common.Exceptions;
using StarterApp.Core.Common.Interfaces;
using StarterApp.Core.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Issues.Events
{
    public class IssueHeaderCreatedEvent : BaseEvent, INotification
    {
        public IssueHeader Entity { get; private set; }
        public IssueHeaderCreatedEvent(IssueHeader entity)
        {
            Entity = entity;
        }
    }
    public class IssueHeaderCreatedHandler : BaseHandler<IssueHeader>, INotificationHandler<IssueHeaderCreatedEvent>
    {
        public IRepository<Product> _productRepository { get; private set; }
        public IssueHeaderCreatedHandler(IRepository<Product> productRepository) : base()
        {
            _productRepository = productRepository;
        }
        
        public async Task Handle(IssueHeaderCreatedEvent @event, CancellationToken cancellationToken)
        {
            var createdItemsDictionary = @event.Entity.IssueDetails
                .GroupBy(x => new { x.ProductId })
                .ToDictionary(x => x.Key.ProductId, x => x.Sum(y => y.Quantity));

            foreach (var (productId, qty) in createdItemsDictionary)
            {
                var productAvailableQty = await _mediator.Send(new GetProductTotalQtyQuery(productId, @event.Entity.IssueDate));
                createdItemsDictionary.TryGetValue(productId, out var createdItemQty);

                var netQty = productAvailableQty.TotalQuantity - createdItemQty;
                if (netQty < 0)
                {
                    var productName = (await _productRepository.GetById(productId)).Name;
                    var warningMessage = $"Not enough quantity for \"{productName}\". Current available is {productAvailableQty.TotalQuantity}";

                    await _mediator.Publish(new WarningNotificationEvent(warningMessage));
                    throw new InsufficientProductQtyException(productName, productAvailableQty.TotalQuantity, netQty);
                }
            }
        }
    }
}
