using System;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Frontend.Data.Identity;
using C3Apparel.Frontend.Data.Membership;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BlankSiteCore
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        private const string AuthCookieName = "identity.auth";

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        private static void ConfigureMembershipServices(IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    // Note: These settings are effective only when password policies are turned off in the administration settings.
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 0;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();;

            services.AddAuthorization();
            services.AddAuthentication();

            services.ConfigureApplicationCookie(c =>
            {
                c.LoginPath = new PathString("/login");
                c.ExpireTimeSpan = TimeSpan.FromDays(14);
                c.SlidingExpiration = true;
                c.Cookie.Name = AuthCookieName;
            });

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            if (Environment.IsDevelopment())
            {

            }

            // Configure views to be picked up from feature folders
            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Features/{1}/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Features/Shared/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Components/ViewComponents/{0}" + RazorViewEngine.ViewExtension);
            });
            
            ConfigureMembershipServices(services);
            services.AddControllersWithViews();
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductPricingInfoProvider, ProductPricingInfoProvider>();
            services.AddScoped<IPriceSettingsInfoProvider, PriceSettingsInfoProvider>();
            services.AddScoped<IProductPricingService, ProductPricingService>();
            services.AddScoped<IProductSettingsRepository, ProductSettingsRepository>();
            services.AddScoped<IPriceCalculator, PriceCalculator>();
            services.AddScoped<IExchangeRateRetriever, ExchangeRateRetriever>();
            services.AddScoped<IExchangeRateInfoProvider, ExchangeRateInfoProvider>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value;
                
                await next();

                string errorPath = null;
                switch (context.Response.StatusCode)
                {
                    case 404:
                        errorPath = "/page-not-found";
                        break;
                }

                if (!string.IsNullOrWhiteSpace(errorPath) &&
                    !(context.Request.Path.Value?.Equals(errorPath, StringComparison.InvariantCultureIgnoreCase) ??
                      false))
                {
                    context.Request.Path = errorPath;
                    await next();
                }
            });
            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseCors();

            app.UseAuthentication();
            // app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
            });

        }
    }
}
