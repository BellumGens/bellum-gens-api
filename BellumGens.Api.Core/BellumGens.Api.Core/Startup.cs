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
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BellumGens.Api.Core
{
    public class Startup
    {
        public static string PublicClientId { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            PublicClientId = Configuration["publicClientId"];
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

            services.AddMemoryCache();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.ExpireTimeSpan = TimeSpan.FromDays(14);
                    options.SlidingExpiration = true;
                })
                .AddBattleNet(options =>
                {
                    options.ClientId = Configuration["battleNetClientId"];
                    options.ClientSecret = Configuration["battleNetClientSecret"];
                })
                .AddTwitch(options =>
                {
                    options.ClientId = Configuration["twitchClientId"];
                    options.ClientSecret = Configuration["twitchSecret"];
                    options.CallbackPath = "/signin-twitch";
                })
                .AddSteam(options =>
                {
                    options.ApplicationKey = Configuration["steamApiKey"];
                });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.RequireUniqueEmail = false;
            });

            services.AddSingleton<AppConfiguration>();
            services.AddSingleton<ISteamService, SteamServiceProvider>();
            services.AddSingleton<INotificationService, NotificationsService>();
            services.AddSingleton<IEmailSender, EmailServiceProvider>();
            services.AddScoped<IFileService, FileService>();

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

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<BellumGensDbContext>();
                context.Database.Migrate();
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
