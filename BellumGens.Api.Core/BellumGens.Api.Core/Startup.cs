using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BellumGens.Api.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using BellumGens.Api.Core.Providers;

namespace BellumGens.Api.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BellumGensDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BellumGensDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<AppConfiguration>();
            services.AddScoped<ISteamService, SteamServiceProvider>();
            services.AddScoped<INotificationService, NotificationsService>();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
                options.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
 
                    // Custom
                    "image/svg+xml",
                    "application/font-woff2"
                };
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            if (env.IsDevelopment())
            {
                app.UseCors(o => o.AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowCredentials()
                                  .WithOrigins(new string[] {
                                      "http://localhost:4200",
                                      "http://localhost:4000",
                                      "http://localhost:4201",
                                      "http://localhost:4001"
                                  }));
            }
            else
            {
                app.UseCors(o => o.AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowCredentials()
                                  .WithOrigins(new string[] { 
                                      "https://bellumgens.com",
                                      "https://www.bellumgens.com",
                                      "https://eb-league.com",
                                      "https://www.eb-league.com",
                                      "http://staging.bellumgens.com",
                                      "http://staging.eb-league.com" 
                                  }));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
