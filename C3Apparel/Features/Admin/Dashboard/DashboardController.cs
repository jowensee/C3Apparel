using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BlankSiteCore.Features.Base.API;
using C3Apparel.Data.Common;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Features.Admin.Brand;
using C3Apparel.Features.Admin.Brand.API;
using C3Apparel.Frontend.Data.Settings;
using C3Apparel.Web.Features.Pricing.API.Requests;
using C3Apparel.Web.Features.Pricing.API.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Features.Admin.Dashboard
{
    //[TypeFilter(typeof(AdminAuthorizationFilter))]
    public class DashboardController : Controller
    {
        public DashboardController(IBrandInfoProvider brandInfoProvider)
        {
        }
        
       
        public async Task<ActionResult> Index()
        {

            var vm = new DashboardViewModel();

            return View("~/Features/Admin/Dashboard/Dashboard.cshtml",vm);
        }
        

    }
}