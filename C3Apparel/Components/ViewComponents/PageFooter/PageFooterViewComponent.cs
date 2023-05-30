using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Web.Component.ViewComponents.PageFooter
{
    public class PageFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Components/ViewComponents/PageFooter/Default.cshtml", new PageFooterViewModel());
        }
    }
}