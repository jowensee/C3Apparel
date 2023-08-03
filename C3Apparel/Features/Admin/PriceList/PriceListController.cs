using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlankSiteCore.Features.Base.API;
using BlankSiteCore.Features.PriceList;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Data.Pricing;
using C3Apparel.Features.Admin.PriceList.API.Responses;
using C3Apparel.Frontend.Data.Common;
using C3Apparel.Frontend.Data.Settings;
using C3Apparel.Web.Authentication;
using C3Apparel.Web.Features.Brand.API.Responses;
using C3Apparel.Web.Features.PriceList.API.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace C3Apparel.Features.Admin.PriceList
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class PriceListController : Controller
    {
        private readonly IBrandInfoProvider _brandInfoProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPriceListService _priceListService;
        private readonly IPriceListFileService _priceListFileService;
        public PriceListController(IBrandInfoProvider brandInfoProvider, IHttpContextAccessor httpContextAccessor, IPriceListService priceListService, IPriceListFileService priceListFileService)
        {
            _brandInfoProvider = brandInfoProvider;
            _httpContextAccessor = httpContextAccessor;
            _priceListService = priceListService;
            _priceListFileService = priceListFileService;
        }



        public async Task<ActionResult> PriceList()
        {

            var vm = new PricingListPageViewModel();

            vm.Brands = _brandInfoProvider.GetAllBrands(null, 1, 1000)
                .Select(a => new ListItem(a.BrandDisplayName, a.BrandID.ToString(), false));
            return View("~/Features/Admin/PriceList/PriceListPage.cshtml", vm);
        }

        [HttpPost]
        [Route("save-price-list")]
        public IActionResult SaveToPriceListTable([FromBody] SavePriceToDBTableParameters request)
        {
            var message = string.Empty;
            var priceCount = 0;
            var response = new CommandAPIResult();
            (message, priceCount) = _priceListService.SavePriceListToPriceListTable(1, CurrencyConstants.AUD, request.BrandId);

            if (!message.IsNullOrEmpty())
            {
                response.Message = message;
            }
            else
            {
                (message, priceCount) = _priceListService.SavePriceListToPriceListTable(1, CurrencyConstants.NZD, request.BrandId);
                if (!message.IsNullOrEmpty())
                {
                    response.Message = message;
                }
            }

            if (priceCount == 0)
            {
                response.Message = "Price List is empty";
            }
            else
            {
                response.Success = true;
                var brand = _brandInfoProvider.GetBrand(request.BrandId);
            
                _priceListFileService.GeneratePDFFile(request.BrandId, brand.BrandName, CurrencyConstants.AUD);
                _priceListFileService.GeneratePDFFile(request.BrandId, brand.BrandName, CurrencyConstants.NZD);  
                _priceListFileService.GenerateCSVFile(request.BrandId, brand.BrandName, CurrencyConstants.AUD);   
                _priceListFileService.GenerateCSVFile(request.BrandId, brand.BrandName, CurrencyConstants.NZD);   
            }
            return Ok(response);
        }
        
        [Route("get-brands-pricelist")]
        [HttpPost]
        public async Task<ActionResult> GetBrandsPriceList([FromBody] GetBrandPriceListsParameters request)
        {
            int GetTotalPage(int totalItems, int itemsPerPage)
            {
                if (totalItems % itemsPerPage == 0)
                {
                    return totalItems / itemsPerPage;
                }

                return (int) Math.Floor((double) totalItems / itemsPerPage) + 1;
            }
            var response = new GetBrandsPriceListResponse();

            IEnumerable<BrandInfo> brands = _brandInfoProvider.GetAllBrands(null, request.PageNumber, request.ItemsPerPage);

            var totalCount = _brandInfoProvider.GetAllBrandsCount(null);
            
            response.TotalPage = GetTotalPage(totalCount, request.ItemsPerPage);
            response.Brands = brands.Select(p => new BrandPriceListAPIItem()
            {
                Brand = p.BrandDisplayName,
                BrandId = p.BrandID,
                Enabled = p.BrandEnabled,
                PublishDate = p.BrandPriceListPublishedDate == DateTime.MinValue ? string.Empty : p.BrandPriceListPublishedDate.ToString("d/M/yyyy"),
                PDFAUPriceUrl = _priceListFileService.GetPriceListFile(p.BrandName, CurrencyConstants.AUD, PriceListConstants.FILE_TYPE_PDF),
                PDFNZPriceUrl = _priceListFileService.GetPriceListFile(p.BrandName, CurrencyConstants.NZD, PriceListConstants.FILE_TYPE_PDF),
                CSVAUPriceUrl = _priceListFileService.GetPriceListFile(p.BrandName, CurrencyConstants.AUD, PriceListConstants.FILE_TYPE_CSV),
                CSVNZPriceUrl = _priceListFileService.GetPriceListFile(p.BrandName, CurrencyConstants.NZD, PriceListConstants.FILE_TYPE_CSV),

            }).ToList();
            
            return Ok(response);
        }
        
    }
}