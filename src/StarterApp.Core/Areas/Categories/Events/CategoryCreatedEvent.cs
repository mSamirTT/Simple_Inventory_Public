using MediatR;
using StarterApp.Core.Areas.Categories.Entities;
using StarterApp.Core.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Categories.Events
{
    public class CategoryCreatedEvent : BaseEvent, INotification
    {
        public Category Category { get; private set; }
        public CategoryCreatedEvent(Category category)
        {
            Category = category;
        }
    }
    public class CategoryCreatedHandler : BaseHandler<Category>, INotificationHandler<CategoryCreatedEvent>
    {
        public Task Handle(CategoryCreatedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
