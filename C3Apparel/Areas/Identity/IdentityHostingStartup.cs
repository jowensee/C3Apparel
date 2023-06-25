using System;
using C3Apparel.Areas.Identity.Data;
using C3Apparel.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(C3Apparel.Areas.Identity.IdentityHostingStartup))]
namespace C3Apparel.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<C3ApparelContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("C3ApparelContextConnection")));

                services.AddDefaultIdentity<C3ApparelUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<C3ApparelContext>();
            });
        }
    }
}
