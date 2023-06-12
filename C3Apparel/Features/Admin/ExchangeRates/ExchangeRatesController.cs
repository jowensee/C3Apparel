using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BlankSiteCore.Features.Base.API;
using C3Apparel.Data.Common;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Features.Admin.Brand;
using C3Apparel.Features.Admin.Brand.API;
using C3Apparel.Features.Admin.ExchangeRates.API;
using C3Apparel.Frontend.Data.Settings;
using C3Apparel.Web.Features.ExchangeRates.API.Requests;
using C3Apparel.Web.Features.ExchangeRates.API.Responses;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Features.Admin.ExchangeRates
{
    //[TypeFilter(typeof(AdminAuthorizationFilter))]
    public class ExchangeRatesController : Controller
    {
        private readonly IExchangeRateInfoProvider _exchangeRateInfoProvider;
        public ExchangeRatesController(IExchangeRateInfoProvider exchangeRateInfoProvider)
        {
            _exchangeRateInfoProvider = exchangeRateInfoProvider;
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
                vm.ID = brandId;   
            }
            vm.OptionsFocus = C3Definitions.BrandFocuses.ToDictionary(a => a, a => a);
            vm.OptionsCurrency = C3Definitions.Currencies.ToDictionary(a => a, a => a);
            return View("~/Features/Admin/Brand/BrandEditPage.cshtml",vm);
        }
        
        [Route("getExchangeRates")]
        [HttpPost]
        public async Task<ActionResult> GetExchangeRates([FromBody]GetExchangeRatesParameters requests)
        {
            int GetTotalPage(int totalItems, int itemsPerPage)
            {
                if (totalItems % itemsPerPage == 0)
                {
                    return totalItems / itemsPerPage;
                }

                return (int) Math.Floor((double) totalItems / itemsPerPage) + 1;
            }
            var response = new GetExchangeRatesResponse();
           
            IEnumerable<ExchangeRateInfo> exchangeRates = _exchangeRateInfoProvider.GetAllExchangeRates(requests.PageNumber, AdminSettings.DEFAULT_PAGE_SIZE);

            var totalCount = _exchangeRateInfoProvider.GetAllExchangeRatesCount();

            response.TotalPage = GetTotalPage(totalCount, requests.ItemsPerPage);
            response.ExchangeRates = exchangeRates.Select(p => new ExchangeRateAPIItem()
            {
                

            }).ToList();
            
            return Ok(response);
        }

        [Route("delete-exchange-rate")]
        [HttpPost]
        public async Task<ActionResult> DeleteExchangeRates([FromBody] IDParameter requests)
        {
            try
            {
                _exchangeRateInfoProvider.Delete(requests.Id);
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
        [Route("get-exchange-rate")]
        [HttpPost]
        public async Task<ActionResult> GetExchangeRateForEdit([FromBody] IDParameter requests)
        {
            try
            {
                var exchangerRate = _exchangeRateInfoProvider.GetExchangerRate(requests.Id);

                if (exchangerRate == null)
                {
                    return Ok();
                }
                return Ok(new GetEditExchangeRateResponse()
                {
                    
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
        
        [Route("save-exchange-rate")]
        [HttpPost]
        public async Task<ActionResult> SaveExchangeRate([FromBody] ExchangeRatesFullDetail requests)
        {
            try
            {
                
                var exchangeRate = new ExchangeRateInfo()
                {
                };

                if (exchangeRate.ExchangeRateID == 0)
                {
                    _exchangeRateInfoProvider.Insert(exchangeRate);
                }
                else
                {
                    _exchangeRateInfoProvider.Update(exchangeRate);
                }

                return Ok(new CommandAPIResult
                {
                   Success = true,
                   RedirectUrl = "/admin/exchange-rates"
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