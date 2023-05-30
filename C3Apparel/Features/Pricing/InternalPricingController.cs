using System.Linq;
using System.Threading.Tasks;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Frontend.Data.Common;
using C3Apparel.Web.Authentication;
using C3Apparel.Web.Features.Pricing;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Web.Features.Pricing
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class InternalPricingController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSettingsRepository _productSettingsRepository;
        
        public InternalPricingController(IProductRepository productRepository, 
            IProductSettingsRepository productSettingsRepository)
        {
            _productRepository = productRepository;
            _productSettingsRepository = productSettingsRepository;
        }
        
       
        //[EnableCors("CMSPolicy")]
        public async Task<ActionResult> InternalPriceListingPage(int brandId = 0)
        {

            var vm = new InternalPriceListingPageViewModel();

            vm.Brands = _productRepository.GetBrandsWithPricing().Select(a=> new ListItem(a.BrandName, a.BrandID.ToString(), brandId == a.BrandID));
            return View("~/Features/Pricing/InternalPriceListingPage.cshtml",vm);
        }

    }
}