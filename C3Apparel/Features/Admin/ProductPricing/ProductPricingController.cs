using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using C3Apparel.Features.Base.API;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Data.Pricing;
using C3Apparel.Features.Admin.ProductPricing.API;
using C3Apparel.Features.Admin.ProductPricing.CSV;
using C3Apparel.Frontend.Data.Common;
using C3Apparel.Frontend.Data.Settings;
using C3Apparel.Infrastructure;
using C3Apparel.Web.Authentication;
using C3Apparel.Web.Features.ProductPricing.API.Requests;
using C3Apparel.Web.Features.ProductPricing.API.Responses;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace C3Apparel.Features.Admin.ProductPricing
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class ProductPricingController : Controller
    {
        private readonly IBrandInfoProvider _brandInfoProvider;
        private readonly IProductPricingInfoProvider _productPricingInfoProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPriceListService _priceListService;
        private readonly ISessionService _sessionService;
        public ProductPricingController(IBrandInfoProvider brandInfoProvider,
            IProductPricingInfoProvider productPricingInfoProvider, IHttpContextAccessor httpContextAccessor, IPriceListService priceListService, ISessionService sessionService)
        {
            _brandInfoProvider = brandInfoProvider;
            _productPricingInfoProvider = productPricingInfoProvider;
            _httpContextAccessor = httpContextAccessor;
            _priceListService = priceListService;
            _sessionService = sessionService;
        }


        public async Task<ActionResult> ProductPricingListing()
        {

            var parameter = _sessionService.GetString(SessionService.KeyProductPricingParameter);

            var request = JsonConvert.DeserializeObject<GetProductPricingsParameters>(parameter);
            var vm = new ProductPricingListingPageViewModel();
            vm.PageFilters = request;

            vm.Brands = _brandInfoProvider.GetAllBrands(null, 1, 1000)
                .Select(a => new ListItem(a.BrandDisplayName, a.BrandID.ToString(), false)).ToList();

            if (request is { Filters.FilterSupplier: > 0 })
            {
                vm.Brands.Where(a=>a.Value == request.Filters.FilterSupplier.ToString()).ToList().ForEach(a=>a.IsSelected = true);
            }
            return View("~/Features/Admin/ProductPricing/ProductPricingListingPage.cshtml", vm);

            
        }

        public async Task<ActionResult> UploadPage()
        {

            var vm = new UploadPricingsPageViewModel();

            vm.Brands = _brandInfoProvider.GetAllBrands(null, 1, 1000)
                .Select(a => new ListItem(a.BrandDisplayName, a.BrandID.ToString(), false));
            return View("~/Features/Admin/ProductPricing/UploadPricingsPage.cshtml", vm);
        }

        [HttpPost]
        [Route("admin/upload-file")]
        public async Task<ActionResult> UploadPricingsFile([FromForm] UploadPricingsParameters form)
        {
            var result = new CommandAPIResult();

            var files = _httpContextAccessor.HttpContext.Request.Form.Files;

            if (files.IsNullOrEmpty())
            {
                result.Message = "No file specified";
                return Ok(result);
            }

            var uploader =
                new CSV.CSVUploader(files[0], _brandInfoProvider,
                    _productPricingInfoProvider);

            var pricings = uploader.RetrievePricingsFromCSV();

            bool success;
            string message;
            (success, message) = uploader.SavePricings(form.BrandId, pricings, form.DeleteAll);

            return Ok(new CommandAPIResult
            {
                Success = success,
                Message = message
            });
        }

        public async Task<ActionResult> PrintPage()
        {

            var vm = new PrintPricingsPageViewModel();

            vm.Brands = _brandInfoProvider.GetAllBrands(null, 1, 1000)
                .Select(a => new ListItem(a.BrandDisplayName, a.BrandID.ToString(), false));
            return View("~/Features/Admin/ProductPricing/PrintPricingsPage.cshtml", vm);
        }
        
        public async Task<ActionResult> ProductPricingEdit(int id = 0)
        {

            var vm = new ProductPricingEditViewModel();

            if (id > 0)
            {
                vm.ID = id;
            }

            vm.Brands = _brandInfoProvider.GetAllBrands(null, 1, 200)
                .ToDictionary(a => a.BrandID.ToString(), b => b.BrandDisplayName);
            return View("~/Features/Admin/ProductPricing/ProductPricingEditPage.cshtml", vm);
        }



        [Route("get-product-pricings")]
        [HttpPost]
        public async Task<ActionResult> GetProductPricings([FromBody] GetProductPricingsParameters requests)
        {
            int GetTotalPage(int totalItems, int itemsPerPage)
            {
                if (totalItems % itemsPerPage == 0)
                {
                    return totalItems / itemsPerPage;
                }

                return (int)Math.Floor((double)totalItems / itemsPerPage) + 1;
            }

            _sessionService.SetValue(SessionService.KeyProductPricingParameter, JsonConvert.SerializeObject(requests));            
            var response = new GetProductPricingsResponse();
            var filter = new ProductPricingFilter
            {
                Description = requests.Filters.FilterDescription,
                //Collection = requests.Filters.FilterCollection,
                C3Style = requests.Filters.FilterC3Style,
                Colour = requests.Filters.FilterColour,
                COO = requests.Filters.FilterCOO,
                ProductGroup = requests.Filters.FilterProductGroup,
                Sizes = requests.Filters.FilterSizes,
                Supplier = requests.Filters.FilterSupplier,
                SupplierStyle = requests.Filters.FilterSupplierStyle,

            };

            var brands = _brandInfoProvider.GetAllBrands();
            IEnumerable<ProductPricingInfo> productPricings =
                _productPricingInfoProvider.GetAllProductPricings(filter, requests.PageNumber,
                    AdminSettings.DEFAULT_PAGE_SIZE);

            var totalCount = _productPricingInfoProvider.GetAllProductPricingsCount(filter);

            response.TotalPage = GetTotalPage(totalCount, requests.ItemsPerPage);
            response.Pricings = productPricings.Select(p => new ProductPricingAPIItem()
            {
                Id = p.ProductPricingID,
                SupplierName = brands.FirstOrDefault(a => a.BrandID == p.ProductPricingSupplierID)?.BrandDisplayName,
                Collection = p.ProductPricingCollection,
                ProductColours = p.ProductPricingColours,
                ColourDescription = p.ProductPricingColourDesc,
                C3Style = p.ProductPricingC3Style,
                C3BuyPrice = p.ProductPricingC3BuyPrice.ToString("0.00"),
                SkuWeight = p.ProductPricingSKUWeight.ToString("0.000kg"),
                Coo = p.ProductPricingCoo,
                Description = p.ProductPricingDescription,
                ProductGroup = p.ProductPricingGroup,
                SupplierStyle = p.ProductPricingSupplierStyle,
                Sizes = p.ProductPricingSizes,
                C3OverrideWeight = p.ProductPricingC3OverrideWeight.ToString("0.000kg"),


            }).ToList();

            return Ok(response);
        }

        [Route("delete-product-pricings")]
        [HttpPost]
        public async Task<ActionResult> DeleteProductPricing([FromBody] IDParameter requests)
        {
            try
            {
                _productPricingInfoProvider.Delete(requests.Id);
                return Ok(new CommandAPIResult
                {
                    Success = true,

                });
            }
            catch (Exception ex)
            {
                return Ok(new CommandAPIResult
                {
                    Message = ex.Message
                });
            }
        }

        //[EnableCors("FEPolicy")]
        [Route("get-product-pricing")]
        [HttpPost]
        public async Task<ActionResult> GetProductPricingsForEdit([FromBody] IDParameter requests)
        {
            try
            {
                var pricing = _productPricingInfoProvider.GetProductPricing(requests.Id);

                if (pricing == null)
                {
                    return Ok();
                }

                return Ok(new GetEditProductPricingResponse()
                {
                    ProductPricing = new ProductPricingFullDetail
                    {
                        Id = pricing.ProductPricingID,
                        BrandId = pricing.ProductPricingSupplierID,
                        C3BuyPrice = pricing.ProductPricingC3BuyPrice,
                        C3OverrideWeight = pricing.ProductPricingC3OverrideWeight,
                        SkuWeight = pricing.ProductPricingSKUWeight,
                        Collection = pricing.ProductPricingCollection,
                        C3Style = pricing.ProductPricingC3Style,
                        Colour = pricing.ProductPricingColours,
                        ColourDescription = pricing.ProductPricingColourDesc,
                        Coo = pricing.ProductPricingCoo,
                        Sizes = pricing.ProductPricingSizes,
                        AllSizes = pricing.ProductPricingAllSizes,
                        SupplierStyle = pricing.ProductPricingSupplierStyle,
                        Status = pricing.ProductPricingStatus,
                        Description = pricing.ProductPricingDescription,
                        ProductGroup = pricing.ProductPricingGroup,
                        ProductSubCategory = pricing.ProductPricingSubCategory

                    }

                });
            }
            catch (Exception ex)
            {
                return Ok(new CommandAPIResult
                {
                    Message = ex.Message
                });
            }
        }

        [Route("save-product-pricing")]
        [HttpPost]
        public async Task<ActionResult> SaveProductPricings([FromBody] ProductPricingFullDetail request)
        {
            try
            {

                var productPricing = new ProductPricingInfo
                {
                    ProductPricingCollection = request.Collection,
                    ProductPricingC3Style = request.C3Style,
                    ProductPricingC3BuyPrice = request.C3BuyPrice,
                    ProductPricingColours = request.Colour,
                    ProductPricingColourDesc = request.ColourDescription,
                    ProductPricingCoo = request.Coo,
                    ProductPricingSubCategory = request.ProductSubCategory,
                    ProductPricingC3OverrideWeight = request.C3OverrideWeight,
                    ProductPricingDescription = request.Description,
                    ProductPricingGroup = request.ProductGroup,
                    ProductPricingSizes = request.Sizes,
                    ProductPricingAllSizes = request.AllSizes,
                    ProductPricingSupplierStyle = request.SupplierStyle,
                    ProductPricingStatus = request.Status,
                    ProductPricingSupplierID = request.BrandId,
                    ProductPricingSKUWeight = request.SkuWeight,
                    ProductPricingID = request.Id
                };

                if (productPricing.ProductPricingID == 0)
                {
                    _productPricingInfoProvider.InsertProductPricing(productPricing);
                }
                else
                {
                    _productPricingInfoProvider.UpdateProductPricing(productPricing);
                }

                return Ok(new CommandAPIResult
                {
                    Success = true,
                    RedirectUrl = "/admin/product-pricings"
                });
            }
            catch (Exception ex)
            {
                return Ok(new CommandAPIResult
                {
                    Message = ex.Message
                });
            }
        }

        [TypeFilter(typeof(AuthentictedAuthorizationFilter))]
        [HttpPost]
        [Route("productmaintenancedownloadcsv")]
        public async Task<ActionResult> DownloadCSV([FromBody] DownloadProductPricingsParameters requests)
        {

            var filter = new ProductPricingFilter
            {
                Description = requests.Filters.FilterDescription,
                Collection = requests.Filters.FilterCollection,
                C3Style = requests.Filters.FilterC3Style,
                Colour = requests.Filters.FilterColour,
                COO = requests.Filters.FilterCOO,
                ProductGroup = requests.Filters.FilterProductGroup,
                Sizes = requests.Filters.FilterSizes,
                Supplier = requests.Filters.FilterSupplier,
                SupplierStyle = requests.Filters.FilterSupplierStyle,

            };

            var brands = _brandInfoProvider.GetAllBrands();
            IEnumerable<ProductPricingInfo> productPricings =
                _productPricingInfoProvider.GetAllProductPricings(filter, 0,0);

            var brand = brands.FirstOrDefault(a => a.BrandID == requests.Filters.FilterSupplier);
            
            var csvItems = productPricings.Select(a => new CSVProductItem
            {
                Supplier = brand?.BrandDisplayName,
                Collection = a.ProductPricingCollection,
                Colours = a.ProductPricingColours,
                SupplierStyle = a.ProductPricingSupplierStyle,
                Sizes = a.ProductPricingSizes,
                ColourDescription = a.ProductPricingColourDesc,
                ProductGroups = a.ProductPricingGroup,
                Description = a.ProductPricingDescription,
                COO = a.ProductPricingCoo,
                C3Style = a.ProductPricingC3Style,
                C3BuyPrice = a.ProductPricingC3BuyPrice,
                Currency = brand?.BrandCurrency,
                SKUWeight = a.ProductPricingSKUWeight,
                C3OverrideWeight = a.ProductPricingC3OverrideWeight,
                Status = a.ProductPricingStatus
            });
            
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csvWriter.WriteRecords(csvItems);
                        streamWriter.Flush();
                        
                        var bytes = memoryStream.ToArray();
                        return new FileStreamResult(new MemoryStream(bytes), "text/csv")
                        {
                            FileDownloadName = "productMaintenanceSearch.csv"
                        };
                    }
                }
            }
            
        }
    }
}