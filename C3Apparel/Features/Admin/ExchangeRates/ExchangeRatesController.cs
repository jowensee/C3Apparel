using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlankSiteCore.Features.Base.API;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Modules.Classes;
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
        
       
        public async Task<ActionResult> ExchangeRatesListing()
        {

            var vm = new ExchangeRatesListingPageViewModel();

            return View("~/Features/Admin/ExchangeRates/ExchangeRatesListingPage.cshtml",vm);
        }
        
        public async Task<ActionResult> ExchangeRatesEdit(int id = 0)
        {

            var vm = new ExchangeRatesEditViewModel();

            if (id > 0)
            {
                vm.ID = id;   
            }
            
            return View("~/Features/Admin/ExchangeRates/ExchangeRatesEditPage.cshtml",vm);
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
                Id = p.ExchangeRateID,
                SourceCurrency = p.ExchangeRateSourceCurrency,
                AudValue = p.ExchangeRateAUDValue,
                NzdValue = p.ExchangeRateNZDValue,
                ValidFrom = p.ExchangeRateValidFrom == DateTime.MinValue ? string.Empty : p.ExchangeRateValidFrom.ToString("dd/MM/yyyy"),
                ValidTo = p.ExchangeRateValidTo == DateTime.MinValue ? string.Empty : p.ExchangeRateValidTo.ToString("dd/MM/yyyy"),
                

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
                    Rate = new ExchangeRateAPIItem()
                    {
                        Id = exchangerRate.ExchangeRateID,
                        SourceCurrency = exchangerRate.ExchangeRateSourceCurrency,
                        AudValue = exchangerRate.ExchangeRateAUDValue,
                        NzdValue = exchangerRate.ExchangeRateNZDValue,
                        ValidFrom = exchangerRate.ExchangeRateValidFrom == DateTime.MinValue ? string.Empty : exchangerRate.ExchangeRateValidFrom.ToString("dd/MM/yyyy"),
                        ValidTo = exchangerRate.ExchangeRateValidTo == DateTime.MinValue ? string.Empty : exchangerRate.ExchangeRateValidTo.ToString("dd/MM/yyyy"),
                

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
        
        [Route("save-exchange-rate")]
        [HttpPost]
        public async Task<ActionResult> SaveExchangeRate([FromBody] ExchangeRateAPIItem requests)
        {
            try
            {
                
                var exchangeRate = new ExchangeRateInfo()
                {
                    ExchangeRateID = requests.Id,
                    ExchangeRateSourceCurrency = requests.SourceCurrency,
                    ExchangeRateAUDValue = requests.AudValue,
                    ExchangeRateNZDValue = requests.NzdValue,
                    ExchangeRateValidFrom = requests.ValidFrom.ToDateTime("dd/MM/yyyy"),
                    ExchangeRateValidTo = requests.ValidTo.ToDateTime("dd/MM/yyyy"),
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