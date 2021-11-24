using StarterApp.Core.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace StarterApp.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetClientSessionId()
        {
            if (_httpContextAccessor?.HttpContext?.Request?.Headers != null)
            {
                if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(
                    "x-session-id", out var clientIdValue))
                    return clientIdValue;
            }

            return string.Empty;
        }

        public string GetClientCorrelationId()
        {
            if (_httpContextAccessor?.HttpContext?.Request?.Headers != null)
            {
                if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(
                    "x-correlation-id", out var clientIdValue))
                    return clientIdValue;
            }

            return string.Empty;
        }
        public string UserId => _httpContextAccessor
            .HttpContext?
            .User?
            .FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
