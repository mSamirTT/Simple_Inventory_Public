using Microsoft.AspNetCore.Http;
using System;

namespace StarterApp.Core.Common
{
    public static class ServiceProviderFactory
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Initialize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static IServiceProvider ServiceProvider => _httpContextAccessor?.HttpContext?.RequestServices;
    }
}
