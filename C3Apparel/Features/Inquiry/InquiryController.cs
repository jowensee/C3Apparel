using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using C3Apparel.Data.Common;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Data.Utilities;
using C3Apparel.PDF;
using C3Apparel.Web.Features.Content.API.Requests;
using C3Apparel.Web.Features.Pricing;
using C3Apparel.Web.Features.Pricing.API.Responses;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace C3Apparel.Web.Features.Content
{
    [Route("inquiry")]
    public class InquiryController : Controller
    {
        
        private readonly IProductRepository _productRepository;
        private readonly IProductSettingsRepository _productSettingsRepository;
        private readonly IExchangeRateRetriever _exchangeRateRetriever;
        private readonly IProductPricingService _productPricingService;
        private readonly IBrandRepository _brandRepository;
        private readonly AllPriceWeightBasedSettings _weightbasedSettings;
        private readonly IInquirySettingsInfoProvider _inquirySettingsInfoProvider;
        
        public InquiryController(IProductSettingsRepository productSettingsRepository, IExchangeRateRetriever exchangeRateRetriever, 
            IProductPricingService productPricingService, IProductRepository productRepository, IBrandRepository brandRepository, 
            IInquirySettingsInfoProvider inquirySettingsInfoProvider)
        {
            _productSettingsRepository = productSettingsRepository;
            _exchangeRateRetriever = exchangeRateRetriever;
            _productPricingService = productPricingService;
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _inquirySettingsInfoProvider = inquirySettingsInfoProvider;
            _weightbasedSettings = _productSettingsRepository.GetAllWeightBasedPriceSettings();
        }

        private decimal GetRate(List<ExchangeRateItem> allRates, string sourceCurrency, string targetCurrency)
        {
            return allRates
                .FirstOrDefault(a => a.SourceCurrency == sourceCurrency && a.TargetCurrency == targetCurrency)?.Rate ?? 0;

        }
        [Route("getfilters")]
        public async Task<ActionResult> GetInitialFilter()
        {
            var response = new GetInquiryFilterResponse();
            var globalSettings = _productSettingsRepository.GetPriceGlobalSettings();
            response.AUImportDuty = globalSettings.AUImportDutyInDecimal;
            response.NZImportDuty = globalSettings.NZImportDutyInDecimal;
            var exchangeRates = _exchangeRateRetriever.GetAllExchangeRates(new []{ CurrencyConstants.EURO, CurrencyConstants.USD }).ToList();

            response.RateAuEuro = GetRate(exchangeRates, CurrencyConstants.EURO, CurrencyConstants.AUD);
            response.RateAuUsd = GetRate(exchangeRates, CurrencyConstants.USD, CurrencyConstants.AUD);
            response.RateNzEuro = GetRate(exchangeRates, CurrencyConstants.EURO, CurrencyConstants.NZD);
            response.RateNzUsd = GetRate(exchangeRates, CurrencyConstants.USD, CurrencyConstants.NZD);
            var allWeightBasedPriceSettings = _productSettingsRepository.GetAllWeightBasedPriceSettings();

            response.EuroFreightSettings = allWeightBasedPriceSettings.AllPriceWeightbasedSettings
                .Where(a => a.Key.Contains(Region.CODE_EUROPE))
                .Select(a => new WeightBasedSettingsResponse
                {
                    WeightInKg = a.Value.WeightInKg,
                    MarginInDecimal=a.Value.MarginInDecimal,
                    AUFreightPerKg=a.Value.AUFreightPerKg,
                    NZFreightPerKg=a.Value.NZFreightPerKg,
                    AUFreightSurcharge=a.Value.FreightSurcharge(CurrencyConstants.AUD),
                    NZFreightSurcharge=a.Value.FreightSurcharge(CurrencyConstants.NZD),
                }).ToList();
            
            response.USFreightSettings = allWeightBasedPriceSettings.AllPriceWeightbasedSettings
                .Where(a => a.Key.Contains(Region.CODE_US))
                .Select(a => new WeightBasedSettingsResponse
                {
                    WeightInKg = a.Value.WeightInKg,
                    MarginInDecimal=a.Value.MarginInDecimal,
                    AUFreightPerKg=a.Value.AUFreightPerKg,
                    NZFreightPerKg=a.Value.NZFreightPerKg,
                    AUFreightSurcharge=a.Value.FreightSurcharge(CurrencyConstants.AUD),
                    NZFreightSurcharge=a.Value.FreightSurcharge(CurrencyConstants.NZD),
                }).ToList();
            
            return Ok(response);

        }
        
        [Route("downloadcsv")]
        public async Task<ActionResult> DownloadCSV(string id)
        {

            IEnumerable<ProductItem> products;
            ResultItem result;

            var vm = new PriceListingPageViewModel();

            Guid guid;
            Guid.TryParse(id, out guid);
            var settings = _inquirySettingsInfoProvider.GetSettingsByGuid(guid);
            var settingsString =  settings?.InquirySettingsJsonString;

            if (string.IsNullOrWhiteSpace(settingsString))
            {
                vm.ErrorMessage = "Settings not found";
                return View("~/Features/Pricing/PricePrintVersionPage.cshtml",vm);
            }

            var requests = JsonConvert.DeserializeObject<GetInquiryPricingRequest>(settingsString);
            
            var brandPricing =
                _brandRepository.GetBrandPricingInfo(requests.BrandID.ToInteger(), requests.TargetCurrency);
            
            if (!brandPricing.IsValid)
            {
                vm.ErrorMessage = "Invalid settings";
                return View("~/Features/Pricing/PricePrintVersionPage.cshtml",vm);
            }
            brandPricing.ImportDuty = requests.PricingSettings.GetImportDuty(requests.TargetCurrency);
            brandPricing.ExchangeRate = new ExchangeRateItem(brandPricing.Brand.BrandRegionCode,
                requests.TargetCurrency,
                requests.PricingSettings.GetExchangeRateValue(brandPricing.Brand.BrandRegionCode,
                    requests.TargetCurrency));

            var freightSettings = MapperHelper.Map(requests.PricingSettings, brandPricing.Brand.BrandRegionCode);

            if (freightSettings == null || freightSettings.Count < 4)
            {
                vm.ErrorMessage = "Incomplete Freight settings";
                return View("~/Features/Pricing/PricePrintVersionPage.cshtml",vm);
            }
            
            var searchFilter = new SearchFilter
            {
                Collection = requests.Collection
            };
            (products, result) = _productPricingService.GetProductsWithConvertedPrice(freightSettings, brandPricing, searchFilter, requests.TargetCurrency);

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

            
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csvWriter.Context.RegisterClassMap<ProductItemCSVMap>();
                        csvWriter.WriteRecords(products);
                        streamWriter.Flush();
                        
                        var bytes = memoryStream.ToArray();
                        return new FileStreamResult(new MemoryStream(bytes), "text/csv")
                        {
                            FileDownloadName = "productpricing.csv"
                        };
                    }
                }
            }
            
        }
        
        
        [Route("print")]
        public async Task<ActionResult> PricePrintVersionPage(string id)
        {

            IEnumerable<ProductItem> products;
            ResultItem result;

            Guid guid;
            Guid.TryParse(id, out guid);
            var vm = new PriceListingPageViewModel();
            var settingsString = _inquirySettingsInfoProvider.GetSettingsByGuid(guid)?.InquirySettingsJsonString;
            if (string.IsNullOrWhiteSpace(settingsString))
            {
                vm.ErrorMessage = "Settings not found";
                return View("~/Features/Pricing/PricePrintVersionPage.cshtml",vm);
            }

            var requests = JsonConvert.DeserializeObject<GetInquiryPricingRequest>(settingsString);
            
            var brandPricing =
                _brandRepository.GetBrandPricingInfo(requests.BrandID.ToInteger(), requests.TargetCurrency);
            
            if (!brandPricing.IsValid)
            {
                vm.ErrorMessage = "Invalid settings";
                return View("~/Features/Pricing/PricePrintVersionPage.cshtml",vm);
            }
            brandPricing.ImportDuty = requests.PricingSettings.GetImportDuty(requests.TargetCurrency);
            brandPricing.ExchangeRate = new ExchangeRateItem(brandPricing.Brand.BrandRegionCode,
                requests.TargetCurrency,
                requests.PricingSettings.GetExchangeRateValue(brandPricing.Brand.BrandRegionCode,
                    requests.TargetCurrency));

            var freightSettings = MapperHelper.Map(requests.PricingSettings, brandPricing.Brand.BrandRegionCode);

            if (freightSettings == null || freightSettings.Count < 4)
            {
                vm.ErrorMessage = "Incomplete Freight settings";
                return View("~/Features/Pricing/PricePrintVersionPage.cshtml",vm);
            }
            
            var searchFilter = new SearchFilter
            {
                Collection = requests.Collection
            };
            (products, result) = _productPricingService.GetProductsWithConvertedPrice(freightSettings, brandPricing, searchFilter, requests.TargetCurrency);

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
                
            return View("~/Features/Pricing/PricePrintVersionPage.cshtml",vm);
        }
        
        [Route("print-pricing")]
        public async Task<ActionResult> InquiryPricePrintVersionPage(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return View("~/Features/Pricing/PricePrintVersionPage.cshtml",new PriceListingPageViewModel
                {
                    ErrorMessage = "Invalid parameters"
                });
            }
            
            var pdfGenerator = new PDFGenerator();
            //TODO presentation url
            var bytes = pdfGenerator.GeneratePDF($"http://localhost/inquiry/print?id={id}");

            return new FileContentResult(bytes, "application/octet-stream")
            {
                FileDownloadName = $"InquiryPriceList_{id}.pdf"
            };
        }
        
        [Route("getprices")]
        [HttpPost]
        public async Task<ActionResult> GetPrices([FromBody]GetInquiryPricingRequest requests)
        {
            int GetTotalPage(int totalItems, int itemsPerPage)
            {
                if (totalItems % itemsPerPage == 0)
                {
                    return totalItems / itemsPerPage;
                }

                return (int) Math.Floor((double) totalItems / itemsPerPage) + 1;
            }

            if (requests == null)
            {
                return BadRequest();
            }
            var response = new GetPricingsResponse();
            IEnumerable<ProductItem> products;
            ResultItem result;


            var brandPricing =
                _brandRepository.GetBrandPricingInfo(requests.BrandID.ToInteger(), requests.TargetCurrency);
            
            if (!brandPricing.IsValid)
            {
                return BadRequest();
            }

            brandPricing.ImportDuty = requests.PricingSettings.GetImportDuty(requests.TargetCurrency);
            brandPricing.ExchangeRate = new ExchangeRateItem(brandPricing.Brand.BrandRegionCode,
                requests.TargetCurrency,
                requests.PricingSettings.GetExchangeRateValue(brandPricing.Brand.BrandRegionCode,
                    requests.TargetCurrency));

            var freightSettings = MapperHelper.Map(requests.PricingSettings, brandPricing.Brand.BrandRegionCode);

            if (freightSettings == null || freightSettings.Count < 4)
            {
                response.ErrorMessage = "Incomplete Freight settings";
                return Ok(response);
            }
            
            var searchFilter = new SearchFilter
            {
                Collection = requests.Collection
            };
            (products, result) = _productPricingService.GetProductsWithConvertedPrice(freightSettings, brandPricing, searchFilter, requests.TargetCurrency, requests.PageNumber, requests.ItemsPerPage);

            if (result.HasError)
            {
                return BadRequest();
            }

            var totalCount = _productRepository.GetAllProductsCount(requests.BrandID.ToInteger(), searchFilter);

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

            if (requests.SaveButtonClicked)
            {
                var settings = new InquirySettingsInfo();
                settings.InquirySettingsGuid = Guid.NewGuid();
                settings.InquirySettingsModifiedWhen = DateTime.Now;
                settings.InquirySettingsName = $"Settings-{DateTime.Now:ddMMyyyyHHmmss}";
                settings.InquirySettingsJsonString = JsonConvert.SerializeObject(requests);
                
                //TODO insert
                //settings.Insert();

                response.SettingsGuid = settings.InquirySettingsGuid.ToString();
            }
            
            return Ok(response);
        }
    }
}