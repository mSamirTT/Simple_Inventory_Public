using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using StarterApp.Common.Filters;
using System.IO;

namespace StarterApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerServiceCollection(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "StarterApp", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                    Enter 'Bearer' [space] and then your token in the text input below.
                    Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "SwaggerApiXmlDocumentation.xml");
                options.IncludeXmlComments(filePath, includeControllerXmlComments: true);
            });

            return services;
        }
    }
}