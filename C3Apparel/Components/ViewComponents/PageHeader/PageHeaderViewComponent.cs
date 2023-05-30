using System.Threading.Tasks;
using C3Apparel.Frontend.Data.Membership;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Web.Component.ViewComponents.PageHeader
{
    public class PageHeaderViewComponent : ViewComponent
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        public PageHeaderViewComponent(ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = _currentUserProvider.GetCurrentUserInfo();
            
            return View("~/Components/ViewComponents/PageHeader/Default.cshtml", new HeaderViewModel(currentUser));
        }
    }
}