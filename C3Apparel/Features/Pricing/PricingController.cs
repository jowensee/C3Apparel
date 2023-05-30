using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using C3Apparel.Data.Common;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Frontend.Data.Common;
using C3Apparel.Frontend.Data.Membership;
using C3Apparel.PDF;
using C3Apparel.Web.Authentication;
using C3Apparel.Web.Features.Pricing;
using C3Apparel.Web.Features.Pricing.API.Requests;
using C3Apparel.Web.Features.Pricing.API.Responses;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Web.Features.Pricing
{
    [TypeFilter(typeof(C3AuthorizationFilter))]
    public class PricingController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductPricingService _productPricingService;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IProductSettingsRepository _productSettingsRepository;
        private readonly AllPriceWeightBasedSettings _weightbasedSettings;
        private readonly IBrandRepository _brandRepository;
        
        public PricingController(IProductRepository productRepository, IProductPricingService productPricingService, ICurrentUserProvider currentUserProvider, 
            IProductSettingsRepository productSettingsRepository, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _productPricingService = productPricingService;
            _currentUserProvider = currentUserProvider;
            _productSettingsRepository = productSettingsRepository;
            _brandRepository = brandRepository;
            _weightbasedSettings = _productSettingsRepository.GetAllWeightBasedPriceSettings();
        }
        
       
        public async Task<ActionResult> PriceListingPage(int brandId = 0)
        {

            var vm = new PriceListingPageViewModel();

            vm.Brands = _productRepository.GetBrandsWithPricing().Select(a=> new ListItem(a.BrandName, a.BrandID.ToString(), brandId == a.BrandID));

            var targetCurrency = _currentUserProvider.GetCurrentUserInfo().Currency;

            if (targetCurrency == CurrencyConstants.AUD)
            {
                vm.CountryName = "Australia";
            }  else if (targetCurrency == CurrencyConstants.NZD)
            {
                vm.CountryName = "New Zealand";
            }
            
            
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
            }
            return View("~/Features/Pricing/PriceListingPage.cshtml",vm);
        }
      
        private bool PriceColumnHasFreightSurcharge(string regionCode, string priceKeyName, string targetCurrency)
        {
            priceKeyName =  WeightbasedSettings.GetRegionPriceKeyName(regionCode, priceKeyName);
            var settings = _weightbasedSettings?.AllPriceWeightbasedSettings;
            return settings.ContainsKey(priceKeyName) && settings[priceKeyName]
                .FreightSurcharge(targetCurrency) > 0;
        }

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
        
        [Route("print-pricing")]
        public async Task<ActionResult> CustomerPricePrintVersionPage(int brandId, string currency = "")
        {
            if (string.IsNullOrEmpty(currency))
            {

                currency = _currentUserProvider.GetCurrentUserInfo().Currency;
                
            }
            
            var brand = _brandRepository.GetBrand(brandId);
            if (brandId == 0 || currency == string.Empty || brand == null)
            {
                return View("~/Features/Pricing/PricePrintVersionPage.cshtml",new PriceListingPageViewModel
                {
                    ErrorMessage = "Invalid parameters"
                });
            }
            
            var pdfGenerator = new PDFGenerator();
            
            //TODO presentation url
            var bytes = pdfGenerator.GeneratePDF($"http://localhost/print?brandid={brandId}&currency={currency}");

            return new FileContentResult(bytes, "application/octet-stream")
            {
                FileDownloadName = $"PriceList{brand.BrandName}_{currency}.pdf"
            };
        }
        
        [Route("getprices")]
        [HttpPost]
        public async Task<ActionResult> GetPrices([FromBody]GetPricesParameters requests)
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
            IEnumerable<ProductItem> products;
            ResultItem result;

            var brandPricing = _brandRepository.GetBrandPricingInfo(requests.BrandID, _currentUserProvider.GetCurrentUserInfo().Currency);

            if (!brandPricing.IsValid)
            {
                return BadRequest();
            }
            
            (products, result) = _productPricingService.GetProductsWithConvertedPrice(brandPricing, _currentUserProvider.GetCurrentUserInfo().Currency, requests.PageNumber, requests.ItemsPerPage);

            if (result.HasError)
            {
                return BadRequest();
            }

            var totalCount = _productRepository.GetAllProductsCount(requests.BrandID, null);

            response.TotalPage = GetTotalPage(totalCount, requests.ItemsPerPage);
            response.Pricings = products.Select(p => new PricingAPIItem
            {
                Brand = p.BrandName,
                ProductCode = p.ProductCode,
                Description = p.ProductName,
                Sizes = p.ProductSizes,
                Colours = p.ProductColours,
                UnitPrice1 = p.FormatPrice(p.Price1),
                Moq1 = p.MinimumOrderQty1,
                FreightSurcharge1 = p.FormatPrice(p.FreightSurcharge1),
                UnitPrice2 = p.FormatPrice(p.Price2),
                Moq2 = p.MinimumOrderQty2,
                FreightSurcharge2 = p.FormatPrice(p.FreightSurcharge1),
                UnitPrice3 = p.FormatPrice(p.Price3),
                Moq3 = p.MinimumOrderQty3,
                FreightSurcharge3 = p.FormatPrice(p.FreightSurcharge1),
                UnitPrice4 = p.FormatPrice(p.Price4),
                Moq4 = p.MinimumOrderQty4,
                FreightSurcharge4 = p.FormatPrice(p.FreightSurcharge1),

            }).ToList();
            
            return Ok(response);
        }

        public async Task<ActionResult> PrintPDF(PrintPDFRequest request)
        {
            return File(new MemoryStream(), null);
        }
    }
}