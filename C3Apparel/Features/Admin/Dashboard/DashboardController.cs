using System.Threading.Tasks;
using C3Apparel.Web.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Features.Admin.Dashboard
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class DashboardController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var vm = new DashboardViewModel();

            return View("~/Features/Admin/Dashboard/Dashboard.cshtml",vm);
        }
        

    }
}