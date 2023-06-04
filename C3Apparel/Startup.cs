using System;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Data.Sql;
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
using Microsoft.Extensions.Configuration;

namespace BlankSiteCore
{
    public class Startup
    {
        private IWebHostEnvironment Environment { get; }
        private readonly IConfiguration _configuration;
        private const string AuthCookieName = "identity.auth";

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            _configuration = configuration;
        }

        private void ConfigureMembershipServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>((option) =>
            {
                option.UseSqlServer(_configuration.GetConnectionString("ConnectionString"));
            });
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
            
            services.AddControllersWithViews();
            
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductSettingsRepository, ProductSettingsRepository>();
            services.AddScoped<IPriceCalculator, PriceCalculator>();
            services.AddScoped<IExchangeRateRetriever, ExchangeRateRetriever>();
            services.AddScoped<IProductPricingService, ProductPricingService>();
            services.AddScoped<IProductPricingInfoProvider, ProductPricingInfoProvider>();
            services.AddScoped<IPriceSettingsInfoProvider, PriceSettingsInfoProvider>();
            services.AddScoped<IExchangeRateInfoProvider, ExchangeRateInfoProvider>();
            services.AddScoped<IBrandInfoProvider, BrandInfoProvider>();
            services.AddScoped<IInquirySettingsInfoProvider, InquirySettingsInfoProvider>();
            services.AddScoped<IImportDutyInfoProvider, ImportDutyInfoProvider>();
            
            ConfigureMembershipServices(services);
            
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

            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Pricing", "pricing/{brandId}",
                    defaults: new { controller = "Pricing", action = "PriceListingPage" });
                endpoints.MapControllerRoute("Login", "login",
                    defaults: new { controller = "Account", action = "LoginPage" });
                
                
                //Admin
                endpoints.MapControllerRoute("Brand Listing", "admin/brands",
                    defaults: new { controller = "Brand", action = "BrandListing" });
                
                endpoints.MapControllerRoute("Brand Edit", "admin/brands/{brandId}",
                    defaults: new { controller = "Brand", action = "BrandEdit" });
                //endpoints.MapRazorPages();
            });

        }
    }
}
