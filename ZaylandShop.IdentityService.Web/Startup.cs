using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ZaylandShop.IdentityService.Controllers;
using ZaylandShop.IdentityService.Entities;
using ZaylandShop.IdentityService.Storage;
using ZaylandShop.IdentityService.Web.Configuration;
using ZaylandShop.IdentityService.Web.Configuration.Swagger;

namespace ZaylandShop.IdentityService.Web;

public class Startup
{
    private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddSqlStorage(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("Db") ?? "");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            
            services.AddIdentity<AppUser, IdentityRole>(config =>
                {
                    config.Password.RequiredLength = 6;
                    config.Password.RequireDigit = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddAspNetIdentity<AppUser>()
                .AddInMemoryApiResources(IdentityServiceConfiguration.ApiResources)
                .AddInMemoryIdentityResources(IdentityServiceConfiguration.IdentityResources)
                .AddInMemoryApiScopes(IdentityServiceConfiguration.ApiScopes)
                .AddInMemoryClients(IdentityServiceConfiguration.Clients)
                .AddDeveloperSigningCredential();

            services.AddAuthentication(IdentityConstants.ApplicationScheme);

            services.ConfigureApplicationCookie(option =>
            {
                option.Cookie.Name = "ZaylandShop.IdentityService.Web.Cookie";
                option.LogoutPath = "/v1/auth/logout";
                option.LoginPath = "/v1/auth/login";
            });

            services.AddDomain();
            services.AddConfig(_configuration);

            services.AddMvc()
                .AddApi()
                .AddValidators()
                .AddControllersAsServices()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy(),
                        false));
                });
            
            services.AddSwagger(_configuration);
            services.Configure<AppSwaggerOptions>(_configuration);
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IServiceProvider serviceProvider, 
            ILogger<Startup> logger, 
            IHostApplicationLifetime lifetime, 
            IOptions<AppSwaggerOptions> swaggerOptions,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            MigrationsRunner.ApplyMigrations(logger, serviceProvider, "ZaylandShop.IdentityService.Web").Wait();
            RegisterLifetimeLogging(lifetime, logger);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (swaggerOptions.Value.UseSwagger)
            {
                app.UseSwaggerWithVersion(apiVersionDescriptionProvider);
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            app.UseAuthentication();
            app.UseAuthorization();
        }
        
        private static void RegisterLifetimeLogging(IHostApplicationLifetime lifetime, ILogger<Startup> logger)
        {
            lifetime.ApplicationStarted.Register(() => logger.LogInformation("App started"));
            lifetime.ApplicationStopped.Register(() => logger.LogInformation("App stopped"));
        }
}