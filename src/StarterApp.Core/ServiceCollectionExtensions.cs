using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StarterApp.Core.Common.Behaviours;
using System.Reflection;

namespace StarterApp.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServiceCollection(this IServiceCollection services)
        {
            //services.Scan(scan => scan
            //    .FromAssemblyOf<IUnitOfWork>()
            //    .AddClasses(classes => classes.AssignableTo(typeof(BaseHandler<>)))
            //    .AsImplementedInterfaces()
            //    .WithTransientLifetime());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddSignalR();
            return services;
        }
    }
}
