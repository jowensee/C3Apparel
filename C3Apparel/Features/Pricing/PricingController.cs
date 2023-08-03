using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlankSiteCore.Features.PriceList;
using C3Apparel.Data.Common;
using C3Apparel.Data.Helpers;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Frontend.Data.Common;
using C3Apparel.PDF;
using C3Apparel.Web.Authentication;
using C3Apparel.Web.Features.Pricing.API.Requests;
using C3Apparel.Web.Features.Pricing.API.Responses;
using C3Apparel.Web.Membership;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Web.Features.Pricing
{
    
    public class PricingController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IPriceListPriceInfoProvider _priceListPriceInfoProvider;
        private readonly IProductPricingService _productPricingService;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IProductSettingsRepository _productSettingsRepository;
        private readonly AllPriceWeightBasedSettings _weightbasedSettings;
        private readonly IBrandRepository _brandRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPriceListFileService _priceListFileService;
        public PricingController(IProductRepository productRepository, IProductPricingService productPricingService, ICurrentUserProvider currentUserProvider, 
            IProductSettingsRepository productSettingsRepository, IBrandRepository brandRepository, IHttpContextAccessor httpContextAccessor, IPriceListPriceInfoProvider priceListPriceInfoProvider, IPriceListFileService priceListFileService)
        {
            _productRepository = productRepository;
            _productPricingService = productPricingService;
            _currentUserProvider = currentUserProvider;
            _productSettingsRepository = productSettingsRepository;
            _brandRepository = brandRepository;
            _httpContextAccessor = httpContextAccessor;
            _priceListPriceInfoProvider = priceListPriceInfoProvider;
            _priceListFileService = priceListFileService;
            _weightbasedSettings = _productSettingsRepository.GetAllWeightBasedPriceSettings();
        }
        
        [TypeFilter(typeof(C3AuthorizationFilter))]
        public async Task<ActionResult> PriceListingPage(string countryCode, int brandId = 0)
        {

            var vm = new PriceListingPageViewModel();

            vm.Brands = _productRepository.GetBrandsWithPricing().Select(a=> new ListItem(a.BrandName, a.BrandID.ToString(), brandId == a.BrandID));

            var targetCurrency = CountryHelper.GetCountryCurrencyCode(countryCode);

            vm.Currency = targetCurrency;
            vm.CountryName = CountryHelper.GetCountryName(countryCode);

            vm.PriceWeightBasedSettings = _weightbasedSettings?.AllPriceWeightbasedSettings;


            var brand = _brandRepository.GetBrand(brandId);

            if (brand != null)
            {
                
                vm.CurrentBrandRegionCode = brand.BrandRegionCode;
                vm.PriceCol1HasFreightSurcharge = PriceColumnHasFreightSurcharge(brand.BrandRegionCode, WeightbasedSettings.Price1KeyName, targetCurrency);
                vm.PriceCol2HasFreightSurcharge = PriceColumnHasFreightSurcharge(brand.BrandRegionCode, WeightbasedSettings.Price2KeyName, targetCurrency);
                vm.PriceCol3HasFreightSurcharge = PriceColumnHasFreightSurcharge(brand.BrandRegionCode,
                    WeightbasedSettings.Price3KeyName, targetCurrency);
                vm.PriceCol4HasFreightSurcharge = PriceColumnHasFreightSurcharge(brand.BrandRegionCode,
                    WeightbasedSettings.Price4KeyName, targetCurrency);

                if (targetCurrency == CurrencyConstants.AUD)
                {
                    vm.DisclaimerText = brand.DisclaimerTextAU;
                }
                else
                {
                    vm.DisclaimerText = brand.DisclaimerTextNZ;
                }

                vm.BrandPDFPriceListUrl = _priceListFileService.GetPriceListFile(brand.BrandCodeName, targetCurrency,
                    PriceListConstants.FILE_TYPE_PDF);
                vm.BrandCSVDataUrl = _priceListFileService.GetPriceListFile(brand.BrandCodeName, targetCurrency,
                    PriceListConstants.FILE_TYPE_CSV);
            }
            return View("~/Features/Pricing/PriceListingPage.cshtml",vm);
        }
        
        [TypeFilter(typeof(C3AuthorizationFilter))]
        public async Task<ActionResult> CustomerPricingInquiryPage(string countryCode)
        {

            var vm = new CustomerPricingInquiryPageViewModel();

            vm.Brands = _productRepository.GetBrandsWithPricing().Select(a=> new ListItem(a.BrandName, a.BrandID.ToString(), false));

            var targetCurrency = CountryHelper.GetCountryCurrencyCode(countryCode);

            vm.Currency = targetCurrency;
            vm.CountryName = CountryHelper.GetCountryName(countryCode);

            vm.PriceWeightBasedSettings = _weightbasedSettings?.AllPriceWeightbasedSettings;
  
            vm.PriceCol1HasFreightSurcharge = true;

            return View("~/Features/Pricing/CustomerPricingInquiryPage.cshtml",vm);
        }
      
        private bool PriceColumnHasFreightSurcharge(string regionCode, string priceKeyName, string targetCurrency)
        {
            priceKeyName =  WeightbasedSettings.GetRegionPriceKeyName(regionCode, priceKeyName);
            var settings = _weightbasedSettings?.AllPriceWeightbasedSettings;
            return settings.ContainsKey(priceKeyName) && settings[priceKeyName]
                .FreightSurcharge(targetCurrency) > 0;
        }
        [TypeFilter(typeof(PDFAuthorizationFilter))]
        [Route("print")]
        public async Task<ActionResult> PricePrintVersionPage(int brandId, string currency)
        {

            if (brandId == 0 || currency == string.Empty)
            {
                return View("~/Features/Pricing/PricePrintVersionPage.cshtml",new PriceListingPageViewModel
                {
                    ErrorMessage = "Invalid parameters"
                });
            }
            var vm = new PriceListingPageViewModel();

            vm.Brands = _productRepository.GetBrandsWithPricing().Select(a=> new ListItem(a.BrandName, a.BrandID.ToString(), brandId == a.BrandID));
           
            if (brandId > 0)
            {
                vm.CurrentBrandName = vm.Brands.FirstOrDefault(a => a.Value == brandId.ToString())?.Text;
               
                IEnumerable<ProductItem> products;
                ResultItem result;

                var brandPricing = _brandRepository.GetBrandPricingInfo(brandId, currency);
                if (!brandPricing.IsValid)
                {
                    return BadRequest();
                }
                (products, result) = _productPricingService.GetProductsWithConvertedPrice(brandPricing, currency);

                if (result.HasError)
                {
                    vm.ErrorMessage = result.Message;
                    return View("~/Features/Pricing/PricePrintVersionPage.cshtml",vm);
                }
                
                vm.Products = products;
                vm.PriceCol1HasFreightSurcharge = products.Any(p => p.FreightSurcharge1 > 0);
                vm.PriceCol2HasFreightSurcharge = products.Any(p => p.FreightSurcharge2 > 0);
                vm.PriceCol3HasFreightSurcharge = products.Any(p => p.FreightSurcharge3 > 0);
                vm.PriceCol4HasFreightSurcharge = products.Any(p => p.FreightSurcharge4 > 0);

                vm.PriceWeightBasedSettings = _weightbasedSettings?.AllPriceWeightbasedSettings;
                
                var brand = _brandRepository.GetBrand(brandId);
                if (brand != null)
                {
                    vm.CurrentBrandRegionCode = brand.BrandRegionCode;
                    if (currency == CurrencyConstants.AUD)
                    {
                        vm.DisclaimerText = brand.DisclaimerTextAU;
                    }
                    else
                    {
                        vm.DisclaimerText = brand.DisclaimerTextNZ;
                    }
                }
            }

            return View("~/Features/Pricing/PricePrintVersionPage.cshtml",vm);
        }
        
        [TypeFilter(typeof(C3AuthorizationFilter))]
        [Route("print-pricing")]
        public async Task<ActionResult> CustomerPricePrintVersionPage(int brandId, string currency)
        {

            var brand = _brandRepository.GetBrand(brandId);
            if (brandId == 0 || currency == string.Empty || brand == null)
            {
                return View("~/Features/Pricing/PricePrintVersionPage.cshtml",new PriceListingPageViewModel
                {
                    ErrorMessage = "Invalid parameters"
                });
            }
            
            var pdfGenerator = new PDFGenerator();

            var baseUrl = $"{(_httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://")}{_httpContextAccessor.HttpContext.Request.Host.Value}"; ;

            
            var bytes = pdfGenerator.GeneratePDF($"{baseUrl}/print?brandid={brandId}&currency={currency}");

            return new FileContentResult(bytes, "application/octet-stream")
            {
                FileDownloadName = $"PriceList{brand.BrandName}_{currency}.pdf"
            };
        }
        
        [TypeFilter(typeof(C3AuthorizationFilter))]
        [Route("search-price-list")]
        [HttpPost]
        public async Task<ActionResult> SearchPriceList([FromBody]SearchPriceListParameters requests)
        {
            SearchPriceListFilter MapFilter()
            {
                if (requests?.Filters == null)
                {
                    return null;
                }

                return new SearchPriceListFilter
                {
                    BrandId = requests.Filters.BrandId,
                    Collection = requests.Filters.Collection,
                    C3Style = requests.Filters.C3Style,
                    Description = requests.Filters.Description,
                    ProductGroup = requests.Filters.ProductGroup,
                    Sizes = requests.Filters.Sizes,
                    Colour = requests.Filters.Colours,
                    ColourDescription = requests.Filters.ColourDescriptions
                    
                };
            }
            
            int GetTotalPage(int totalItems, int itemsPerPage)
            {
                if (totalItems % itemsPerPage == 0)
                {
                    return totalItems / itemsPerPage;
                }

                return (int) Math.Floor((double) totalItems / itemsPerPage) + 1;
            }
            var response = new GetPricingsResponse();
            ResultItem result;

            var currency = requests.Currency;

            var filter = MapFilter();


            var products = _priceListPriceInfoProvider.SearchPriceList(1, currency, filter, requests.PageNumber,
                requests.ItemsPerPage);
            
            var totalCount = _priceListPriceInfoProvider.SearchPriceListCount(1, currency, filter);

            response.TotalPage = GetTotalPage(totalCount, requests.ItemsPerPage);
            response.Pricings = products.Select(p => new PricingAPIItem
            {
                Brand = p.PriceBrandName,
                ProductCode = p.PriceC3Style,
                Collection = p.PriceCollection,
                Description = p.PriceDescription,
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

        [TypeFilter(typeof(C3AuthorizationFilter))]
        [Route("getpricesfrompricelist")]
        [HttpPost]
        public async Task<ActionResult> GetPricesFromPriceList([FromBody]GetPricesParameters requests)
        {
            int GetTotalPage(int totalItems, int itemsPerPage)
            {
                if (totalItems % itemsPerPage == 0)
                {
                    return totalItems / itemsPerPage;
                }

                return (int) Math.Floor((double) totalItems / itemsPerPage) + 1;
            }
            var response = new GetPricingsResponse();

            var currency = requests.Currency;
            var brandPricing = _brandRepository.GetBrandPricingInfo(requests.BrandID, currency);

            if (!brandPricing.IsValid)
            {
                return BadRequest();
            }

            var products = _priceListPriceInfoProvider.GetAllPrices(1, currency, requests.BrandID, requests.PageNumber,
                requests.ItemsPerPage);
            
            var totalCount = _priceListPriceInfoProvider.GetAllPricesCount(1, currency, requests.BrandID);

            response.TotalPage = GetTotalPage(totalCount, requests.ItemsPerPage);
            response.Pricings = products.Select(p => new PricingAPIItem
            {
                Brand = p.PriceBrandName,
                ProductCode = p.PriceC3Style,
                Collection = p.PriceCollection,
                Description = p.PriceDescription,
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
    }
}