using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarterApp.Core.Common.Interfaces;
using StarterApp.Infrastructure.Common;
using StarterApp.Infrastructure.Identity;
using StarterApp.Infrastructure.Persistence;
using StarterApp.Infrastructure.Services;
using System;

namespace StarterApp.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServiceCollection(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("StarterApp.Db"));
            }
            else if (configuration.GetValue<bool>("UsePostgre"))
            {
                services.AddEntityFrameworkNpgsql()
                    .AddDbContext<ApplicationDbContext>(options =>
                {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    string connStr;

                    // Use connection string provided at runtime by Heroku.
                    var connUrl = configuration.GetValue<string>("PosgreUrl");
                    // Parse connection URL to connection string for Npgsql
                    connUrl = connUrl.Replace("postgres://", string.Empty);
                    var pgUserPass = connUrl.Split("@")[0];
                    var pgHostPortDb = connUrl.Split("@")[1];
                    var pgHostPort = pgHostPortDb.Split("/")[0];
                    var pgDb = pgHostPortDb.Split("/")[1];
                    var pgUser = pgUserPass.Split(":")[0];
                    var pgPass = pgUserPass.Split(":")[1];
                    var pgHost = pgHostPort.Split(":")[0];
                    var pgPort = pgHostPort.Split(":")[1];
                    connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};Trust Server Certificate=true; Sslmode=Require;";

                    options.UseNpgsql(connStr,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                    options.EnableSensitiveDataLogging();
                });
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
            services.AddTransient<IJsonSerializer, JsonSerializer>();

            return services;
        }
    }
}
