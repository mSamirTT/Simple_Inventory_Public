using MediatR;
using StarterApp.Core.Areas.Products.Entities;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Products.Events
{
    public class ProductCreatedEvent : BaseEvent, INotification
    {
        public Product Product { get; private set; }
        public ProductCreatedEvent(Product product)
        {
            Product = product;
        }
    }
    public class ProductCreatedHandler : BaseHandler<Product>, INotificationHandler<ProductCreatedEvent>
    {
        public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
