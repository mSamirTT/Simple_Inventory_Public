using MediatR;
using Microsoft.AspNetCore.SignalR;
using StarterApp.Core.Common.Interfaces;
using StarterApp.Core.Common.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Areas.Issues.Events
{
    public class WarningNotificationEvent : INotification
    {
        public string Message { get; private set; }
        public WarningNotificationEvent(string message)
        {
            Message = message;
        }
    }
    public class WarningNotificationEventHandler : INotificationHandler<WarningNotificationEvent>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IHubContext<ApplicationHub> _signalRHub;

        public WarningNotificationEventHandler(IHubContext<ApplicationHub> signalRHub,
            ICurrentUserService currenUserServicetUserService)
        {
            _signalRHub = signalRHub;
            _currentUserService = currenUserServicetUserService;
        }

        public async Task Handle(WarningNotificationEvent @event, CancellationToken cancellationToken)
        {
            await _signalRHub.Clients.All.SendAsync("WarningMessage", _currentUserService.GetClientSessionId(), @event.Message);
        }
    }
}
