using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using StarterApp.Core.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Common.Behaviours
{
    public class RequestLogger<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;
        private readonly IIdentityService _identityService;

        public RequestLogger(ILogger<TRequest> logger, IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userName = _identityService.UserName;
            var userId = _identityService.UserId;
            _logger.LogInformation("StarterApp Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);
        }
    }
}
