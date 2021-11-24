using MediatR;
using StarterApp.Core.Areas.Supplies.Entities;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Supplies.Events
{
    public class SupplyHeaderCreatedEvent : BaseEvent, INotification
    {
        public SupplyHeader SupplyHeader { get; private set; }
        public SupplyHeaderCreatedEvent(SupplyHeader supplyheader)
        {
            SupplyHeader = supplyheader;
        }
    }
    public class SupplyHeaderCreatedHandler : BaseHandler<SupplyHeader>, INotificationHandler<SupplyHeaderCreatedEvent>
    {
        public Task Handle(SupplyHeaderCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
