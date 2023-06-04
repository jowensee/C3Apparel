using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BlankSiteCore.Features.Base.API;
using C3Apparel.Data.Common;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Features.Admin.Brand.API;
using C3Apparel.Frontend.Data.Settings;
using C3Apparel.Web.Features.Pricing.API.Requests;
using C3Apparel.Web.Features.Pricing.API.Responses;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Features.Admin.Brand
{
    //[TypeFilter(typeof(AdminAuthorizationFilter))]
    public class BrandController : Controller
    {
        private readonly IBrandInfoProvider _brandInfoProvider;
        public BrandController(IBrandInfoProvider brandInfoProvider)
        {
            _brandInfoProvider = brandInfoProvider;
        }
        
       
        public async Task<ActionResult> BrandListing(int brandId = 0)
        {

            var vm = new BrandListingPageViewModel();

            return View("~/Features/Admin/Brand/BrandListingPage.cshtml",vm);
        }
        
        public async Task<ActionResult> BrandEdit(int brandId = 0)
        {

            var vm = new BrandEditViewModel();

            if (brandId > 0)
            {
                vm.EditItem = _brandInfoProvider.GetBrand(brandId);   
            }
            vm.OptionsFocus = C3Definitions.BrandFocuses.ToDictionary(a => a, a => a);
            vm.OptionsCurrency = C3Definitions.Currencies.ToDictionary(a => a, a => a);
            return View("~/Features/Admin/Brand/BrandEditPage.cshtml",vm);
        }
        
        [Route("getbrands")]
        [HttpPost]
        public async Task<ActionResult> GetBrands([FromBody]GetBrandsParameters requests)
        {
            int GetTotalPage(int totalItems, int itemsPerPage)
            {
                if (totalItems % itemsPerPage == 0)
                {
                    return totalItems / itemsPerPage;
                }

                return (int) Math.Floor((double) totalItems / itemsPerPage) + 1;
            }
            var response = new GetBrandsResponse();
            var filter = new BrandFilter
            {
                Name = requests.Filters.FilterName,
                Currency = requests.Filters.FilterCurrency,
                Focus = requests.Filters.FilterFocus
            };
            IEnumerable<BrandInfo> brands = _brandInfoProvider.GetAllBrands(filter, requests.PageNumber, AdminSettings.DEFAULT_PAGE_SIZE);

            var totalCount = _brandInfoProvider.GetAllBrandsCount(filter);

            response.TotalPage = GetTotalPage(totalCount, requests.ItemsPerPage);
            response.Brands = brands.Select(p => new BrandAPIItem()
            {
                Brand = p.BrandDisplayName,
                Currency = p.BrandCurrency,
                Focus = p.BrandFocus,
                BrandId = p.BrandID,
                Enabled = p.BrandEnabled,
                PublishDate = p.BrandPriceListPublishedDate.ToString("d/M/yyyy")

            }).ToList();
            
            return Ok(response);
        }

        [Route("delete-brand")]
        [HttpPost]
        public async Task<ActionResult> DeleteBrand([FromBody] IDParameter requests)
        {
            try
            {
                _brandInfoProvider.Delete(requests.Id);
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
        
        [Route("get-brand")]
        [HttpPost]
        public async Task<ActionResult> GetBrandForEdit([FromBody] IDParameter requests)
        {
            try
            {
                var brand = _brandInfoProvider.GetBrand(requests.Id);

                if (brand == null)
                {
                    return Ok();
                }
                return Ok(new GetEditBrandResponse()
                {
                    Brand = new BrandFullDetail{ 
                        Brand = brand.BrandDisplayName,
                        BrandId = brand.BrandID,
                        Enabled = brand.BrandEnabled,
                        Currency = brand.BrandCurrency,
                        Focus = brand.BrandFocus,
                        DisclaimerTextAU = brand.BrandPricingDisclaimerTextAU,
                        DisclaimerTextNZ = brand.BrandPricingDisclaimerTextNZ,
                        Website = brand.Website,
                        PublishDate = brand.BrandPriceListPublishedDate.ToString("dd/MM/yyyy")
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
        
        [Route("save-brand")]
        [HttpPost]
        public async Task<ActionResult> SaveBrand([FromBody] BrandFullDetail requests)
        {
            try
            {
                DateTime publishDate;
                DateTime.TryParseExact(requests.PublishDate, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out publishDate);

                var brand = new BrandInfo
                {
                    BrandID = requests.BrandId,
                    BrandDisplayName = requests.Brand,
                    BrandEnabled = requests.Enabled,
                    BrandFocus = requests.Focus,
                    BrandCurrency = requests.Currency,
                    BrandBusinessName = requests.BusinessName,
                    BrandHomepage = requests.Website,
                    BrandPricingDisclaimerTextAU = requests.DisclaimerTextAU,
                    BrandPricingDisclaimerTextNZ = requests.DisclaimerTextNZ,
                    BrandPriceListPublishedDate = publishDate
                };

                if (brand.BrandID == 0)
                {
                    _brandInfoProvider.InsertBrand(brand);
                }
                else
                {
                    _brandInfoProvider.UpdateBrand(brand);
                }

                return Ok(new CommandAPIResult
                {
                   Success = true
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

    }
}