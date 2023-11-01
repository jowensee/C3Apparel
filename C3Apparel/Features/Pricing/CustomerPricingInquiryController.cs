using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using C3Apparel.Features.PriceList;
using C3Apparel.Data.Common;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Helpers;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Features.Admin.ProductPricing.CSV;
using C3Apparel.Frontend.Data.Common;
using C3Apparel.PDF;
using C3Apparel.Web.Authentication;
using C3Apparel.Web.Features.Pricing.API.Requests;
using C3Apparel.Web.Features.Pricing.API.Responses;
using C3Apparel.Web.Membership;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Web.Features.Pricing
{
    
    public class CustomerPricingInquiryController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IPriceListPriceInfoProvider _priceListPriceInfoProvider;
        private readonly IProductPriceConversionService _productPriceConversionService;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IProductSettingsRepository _productSettingsRepository;
        private readonly AllPriceWeightBasedSettings _weightbasedSettings;
        private readonly IBrandRepository _brandRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPriceListFileService _priceListFileService;
        public CustomerPricingInquiryController(IProductRepository productRepository, IProductPriceConversionService productPriceConversionService, ICurrentUserProvider currentUserProvider, 
            IProductSettingsRepository productSettingsRepository, IBrandRepository brandRepository, IHttpContextAccessor httpContextAccessor, IPriceListPriceInfoProvider priceListPriceInfoProvider, IPriceListFileService priceListFileService)
        {
            _productRepository = productRepository;
            _productPriceConversionService = productPriceConversionService;
            _currentUserProvider = currentUserProvider;
            _productSettingsRepository = productSettingsRepository;
            _brandRepository = brandRepository;
            _httpContextAccessor = httpContextAccessor;
            _priceListPriceInfoProvider = priceListPriceInfoProvider;
            _priceListFileService = priceListFileService;
            _weightbasedSettings = _productSettingsRepository.GetAllWeightBasedPriceSettings();
        }
        
        
        [TypeFilter(typeof(AuthentictedAuthorizationFilter))]
        public async Task<ActionResult> CustomerPricingInquiryPage(string countryCode)
        {

            var vm = new CustomerPricingInquiryPageViewModel();

            var user = await _currentUserProvider.GetCurrentUserInfo();

            vm.UserIsAdministrator = user.IsAdministrator;
            vm.Brands = _productRepository.GetBrandsWithPricing(true).Select(a=> new ListItem(a.BrandName, a.BrandID.ToString(), false));

            var targetCurrency = CountryHelper.GetCountryCurrencyCode(countryCode);

            vm.Currency = targetCurrency;
            vm.CountryCode = countryCode;
            vm.CountryName = CountryHelper.GetCountryName(countryCode);

            vm.PriceWeightBasedSettings = _weightbasedSettings?.AllPriceWeightbasedSettings;
  
            vm.PriceCol1HasFreightSurcharge = true;

            return View("~/Features/Pricing/CustomerPricingInquiryPage.cshtml",vm);
        }
      
     
        private SearchPriceListFilter MapFilter(SearchPriceListParameters requests)
        {
            if (requests?.Filters == null)
            {
                return null;
            }

            return new SearchPriceListFilter
            {
                Brands = requests.Filters.Brands,
                //Collection = requests.Filters.Collection,
                C3Style = requests.Filters.C3Style,
                Description = requests.Filters.Description,
                ProductGroup = requests.Filters.ProductGroup,
                Sizes = requests.Filters.Sizes,
                Colour = requests.Filters.Colours,
                ColourDescription = requests.Filters.ColourDescriptions
                    
            };
        }
        
        [TypeFilter(typeof(AuthentictedAuthorizationFilter))]
        [Route("search-price-list")]
        [HttpPost]
        public async Task<ActionResult> SearchPriceList([FromBody]SearchPriceListParameters requests)
        {
            
            
            int GetTotalPage(int totalItems, int itemsPerPage)
            {
                if (totalItems % itemsPerPage == 0)
                {
                    return totalItems / itemsPerPage;
                }

                return (int) Math.Floor((double) totalItems / itemsPerPage) + 1;
            }
            var response = new GetProductInquiryResponse();
            ResultItem result;

            var currency = requests.Currency;

            var filter = MapFilter(requests);


            var products = _priceListPriceInfoProvider.SearchPriceList(1, currency, filter, requests.PageNumber,
                requests.ItemsPerPage);
            
            var totalCount = _priceListPriceInfoProvider.SearchPriceListCount(1, currency, filter);

            response.TotalResult = totalCount;
            response.TotalPage = GetTotalPage(totalCount, requests.ItemsPerPage);
            response.Pricings = products.Select(p => new ProductInquiryAPIItem
            {
                Brand = p.PriceBrandName,
                ProductCode = p.PriceC3Style,
                Collection = p.PriceCollection,
                Description = p.PriceDescription,
                ProductGroup = p.PriceGroup,
                ColorDescription = p.PriceColourDesc,
                Sizes = p.PriceSizes,
                Colours = p.PriceColours,
                UnitPrice1 = p.FormatPrice(p.PriceCol1UnitPrice),
                Moq1 = p.PriceCol1MOQUnit,
                FreightSurcharge1 = p.FormatPrice(p.PriceCol1FreightSurcharge),
                UnitPrice2 = p.FormatPrice(p.PriceCol2UnitPrice),
                Moq2 = p.PriceCol2MOQUnit,
                FreightSurcharge2 = p.FormatPrice(p.PriceCol2FreightSurcharge),
                UnitPrice3 = p.FormatPrice(p.PriceCol3UnitPrice),
                Moq3 = p.PriceCol3MOQUnit,
                FreightSurcharge3 = p.FormatPrice(p.PriceCol3FreightSurcharge),
                UnitPrice4 = p.FormatPrice(p.PriceCol4UnitPrice),
                Moq4 = p.PriceCol4MOQUnit,
                FreightSurcharge4 = p.FormatPrice(p.PriceCol4FreightSurcharge),

            }).ToList();
            
            return Ok(response);
        }

        
        [TypeFilter(typeof(AuthentictedAuthorizationFilter))]
        [HttpPost]
        [Route("inquirydownloadcsv")]
        public async Task<ActionResult> DownloadCSV([FromBody] SearchPriceListParameters requests)
        {

            var currency = requests.Currency;

            var filter = MapFilter(requests);

            var totalCount = _priceListPriceInfoProvider.SearchPriceListCount(1, currency, filter);

            if (totalCount > 500)
            {
                
                return Json(new DownloadCSVResponse
                {
                    ErrorMessage = $"Your search results is more than the allowed number of rows of 500. Please update your filter to trim your results."
                });
            }
            
            var products = _priceListPriceInfoProvider.SearchPriceList(1, currency, filter);
            
            
            //TODO create csvmodel for pricelist item
            var csvItems = products.Select(a => new CSVPProductInquiryItem
            {
                Brand = a.PriceBrandName,
                Collection = a.PriceCollection,
                Colours = a.PriceColours,
                Style = a.PriceC3Style,
                Sizes = a.PriceSizes,
                ColorDescription = a.PriceColourDesc,
                ProductGroup = a.PriceGroup,
                Description = a.PriceDescription,
                FreightSurcharge1 = a.PriceCol1FreightSurcharge,
                MinimumOrderQty1 = a.PriceCol1MOQUnit,
                Price1 = a.PriceCol1UnitPrice,
                MinimumOrderQty2 = a.PriceCol2MOQUnit,
                Price2 = a.PriceCol2UnitPrice,
                MinimumOrderQty3 = a.PriceCol3MOQUnit,
                Price3 = a.PriceCol3UnitPrice,
                MinimumOrderQty4 = a.PriceCol4MOQUnit,
                Price4 = a.PriceCol4UnitPrice
            });
            
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        //csvWriter.Context.RegisterClassMap<ProductItemCSVMap>();
                        csvWriter.WriteRecords(csvItems);
                        streamWriter.Flush();
                        
                        var bytes = memoryStream.ToArray();
                        return new FileStreamResult(new MemoryStream(bytes), "text/csv")
                        {
                            FileDownloadName = "pricelistSearch.csv"
                        };
                    }
                }
            }
            
        }
    }
}