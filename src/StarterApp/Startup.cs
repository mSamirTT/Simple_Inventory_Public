using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StarterApp.Core;
using StarterApp.Core.Common;
using StarterApp.Core.Common.SignalR;
using StarterApp.Extensions;
using StarterApp.Filters;
using StarterApp.Infrastructure;
using StarterApp.Infrastructure.Persistence;
using StarterApp.ViewModels;

namespace StarterApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            services.AddCors();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddHttpContextAccessor();
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddControllers(options =>
            {
                options.Filters.Add(new ApiExceptionFilter());
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCoreServiceCollection();
            services.AddInfrastructureServiceCollection(Configuration, Environment);
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.AddAuthServiceCollection(Configuration.GetSection("JwtSettings").Get<JwtSettings>());
            services.AddSwaggerServiceCollection();

            services.AddControllersWithViews(); 
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot/ClientApp/build";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            ServiceProviderFactory.Initialize(httpContextAccessor);
            app.UseCors(builder =>
                builder
                .WithOrigins("http://localhost:5001", "https://localhost:5001", "http://localhost:3000", "https://localhost:3000")
                .AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api V1");
            });

            app.UseRouting();
            app.UseAuth();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ApplicationHub>("applicationhub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            });

            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
            });
        }
    }
}
