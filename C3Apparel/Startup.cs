using System;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Data.Sql;
using C3Apparel.Web.Membership;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace BlankSiteCore
{
    public class Startup
    {
        private IWebHostEnvironment Environment { get; }
        private readonly IConfiguration _configuration;
        //private const string AuthCookieName = "identity.auth";

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            _configuration = configuration;
        }

        private void ConfigureMembershipServices(IServiceCollection services)
        {
            /*services.AddDbContext<ApplicationDbContext>((option) =>
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
            */
            services.AddAuthorization();
            services.AddAuthentication();

            /*services.ConfigureApplicationCookie(c =>
            {
                c.LoginPath = new PathString("/login");
                c.ExpireTimeSpan = TimeSpan.FromDays(14);
                c.SlidingExpiration = true;
                c.Cookie.Name = AuthCookieName;
            });*/

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
            services.AddScoped<IPriceListPriceInfoProvider, PriceListPriceInfoProvider>();
            services.AddScoped<IExchangeRateInfoProvider, ExchangeRateInfoProvider>();
            services.AddScoped<IBrandInfoProvider, BrandInfoProvider>();
            services.AddScoped<IInquirySettingsInfoProvider, InquirySettingsInfoProvider>();
            services.AddScoped<IImportDutyInfoProvider, ImportDutyInfoProvider>();
            services.AddScoped<IUserProvider, UserProvider>();
            services.AddScoped<IPriceListService, PriceListService>();
            ConfigureMembershipServices(services);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var rewriteOptions = new RewriteOptions();
            rewriteOptions.AddRedirect("$^", "login");
            if (System.IO.File.Exists("rewrite.config"))
            {
                var iisUrlRewriteStreamReader =
                    System.IO.File.OpenText("rewrite.config");

                rewriteOptions.AddRedirect("$^", "login");
                rewriteOptions.AddIISUrlRewrite(iisUrlRewriteStreamReader);
            }
            app.UseRewriter(rewriteOptions);
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

            
            app.UseRouting();
            
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Pricing", "pricing/{countrycode}",
                    defaults: new { controller = "Pricing", action = "PriceListingPage" });
                
                endpoints.MapControllerRoute("Pricing", "pricing-inquiry/{countrycode}",
                    defaults: new { controller = "Pricing", action = "CustomerPricingInquiryPage" });
                
                //endpoints.MapControllerRoute("Login", "login",
                //    defaults: new { controller = "Account", action = "LoginPage" });
                
                
                //Admin
                endpoints.MapControllerRoute("Dashboard", "admin",
                    defaults: new { controller = "Dashboard", action = "Index" });
                
                endpoints.MapControllerRoute("Brand Listing", "admin/brands",
                    defaults: new { controller = "Brand", action = "BrandListing" });
                
                endpoints.MapControllerRoute("Brand Edit", "admin/brands/{brandId}",
                    defaults: new { controller = "Brand", action = "BrandEdit" });
                
                endpoints.MapControllerRoute("Import Duty", "admin/import-duty",
                    defaults: new { controller = "ImportDuty", action = "Index" });
                
                endpoints.MapControllerRoute("FreightSettings", "admin/freight-settings",
                    defaults: new { controller = "FreightWeightSettings", action = "Index" });
                
                endpoints.MapControllerRoute("Exchange Rates Listing", "admin/exchange-rates",
                    defaults: new { controller = "ExchangeRates", action = "ExchangeRatesListing" });
                
                endpoints.MapControllerRoute("Exchange Rates Edit", "admin/exchange-rates/{id}",
                    defaults: new { controller = "ExchangeRates", action = "ExchangeRatesEdit" });
                
                endpoints.MapControllerRoute("Product Pricing Listing", "admin/product-pricings",
                    defaults: new { controller = "ProductPricing", action = "ProductPricingListing" });
                
                endpoints.MapControllerRoute("Product Pricing Edit", "admin/product-pricings/{id}",
                    defaults: new { controller = "ProductPricing", action = "ProductPricingEdit" });
                
                endpoints.MapControllerRoute("Upload pricings page", "admin/upload-pricings",
                    defaults: new { controller = "ProductPricing", action = "UploadPage" });

                endpoints.MapControllerRoute("Print pricings page", "admin/print-pricings",
                    defaults: new { controller = "ProductPricing", action = "PrintPage" });
                
                endpoints.MapControllerRoute("Customer Print pricings", "print-pricing",
                    defaults: new { controller = "Pricing", action = "CustomerPricePrintVersionPage" });
                
                endpoints.MapControllerRoute("Print version", "print",
                    defaults: new { controller = "Pricing", action = "PricePrintVersionPage" });
                
                endpoints.MapControllerRoute("Inquiry Page", "admin/inquiry",
                    defaults: new { controller = "InternalPricing", action = "InternalPriceListingPage" });
                endpoints.MapControllerRoute("Inquiry Print version", "print",
                    defaults: new { controller = "Inquiry", action = "PricePrintVersionPage" });
                
                endpoints.MapControllerRoute("Users Listing", "admin/users",
                    defaults: new { controller = "User", action = "UserListing" });

                endpoints.MapRazorPages();
            });

        }
    }
}
