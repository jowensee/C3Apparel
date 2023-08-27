using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using C3Apparel.Features.Base.API;
using C3Apparel.Data.Common;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Data.Pricing;
using C3Apparel.Features.Admin.FreightWeighPricing.API;
using C3Apparel.Features.Admin.FreightWeighPricing.API.Requests;
using C3Apparel.Frontend.Data.Settings;
using C3Apparel.Web.Authentication;
using C3Apparel.Web.Features.AdminImportDuty.API.Responses;
using C3Apparel.Web.Features.Pricing.API.Requests;
using C3Apparel.Web.Features.Pricing.API.Responses;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace C3Apparel.Features.Admin.FreightWeighPricing
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class FreightWeightSettingsController : Controller
    {
        private readonly IPriceSettingsInfoProvider _priceSettingsInfoProvider;
        
        private readonly IProductSettingsRepository _productSettingsRepository;
        public FreightWeightSettingsController(IPriceSettingsInfoProvider priceSettingsInfoProvider, IProductSettingsRepository productSettingsRepository)
        {
            _priceSettingsInfoProvider = priceSettingsInfoProvider;
            _productSettingsRepository = productSettingsRepository;
        }
        
       
        public async Task<ActionResult> Index()
        {

            var vm = new FreightWeightPricingEditViewModel();

            return View("~/Features/Admin/FreightWeightPricing/FreightWeightPricingEditPage.cshtml",vm);
        }
        
        
        [Route("get-price-settings")]
        [HttpPost]
        public async Task<ActionResult> GetFreightWeightSettings()
        {
            var response = new GetEditFreightWeighPricingResponse();

            var weightbasedSettings = _productSettingsRepository.GetAllWeightBasedPriceSettings();
            if (weightbasedSettings != null)
            {
                response.EuroFreightSettings = weightbasedSettings.AllPriceWeightbasedSettings
                    .Where(a => a.Key.Contains(Region.CODE_EUROPE))
                    .Select(a=>WeightBasedSettingsResponse.Create(a.Value)).ToList();
            
                response.USFreightSettings = weightbasedSettings.AllPriceWeightbasedSettings
                    .Where(a => a.Key.Contains(Region.CODE_US))
                    .Select(a=>WeightBasedSettingsResponse.Create(a.Value)).ToList();
                return Ok(response);
            }
            
            return Ok(response);
        }


        
        [Route("save-freigh-weight-pricing")]
        [HttpPost]
        public async Task<ActionResult> SaveFreightWeightSettings([FromBody] SaveFreightWeightPricingRequest requests)
        {
            try
            {
                //MarginInDecimal variable is not reflection of the value type, it is in percentage when saved from Admin
                foreach (var setting in requests.EuroFreightSettings)
                {
                    
                    _priceSettingsInfoProvider.Update(PriceSettingsInfo.Create(
                        setting.CodeName, setting.WeightInKg, setting.MarginInDecimal,
                        setting.AUFreightPerKg, setting.NZFreightPerKg, setting.AUFreightSurcharge, setting.NZFreightSurcharge,
                        setting.ColumnHeader1, setting.ColumnHeader2));    
                }
                
                foreach (var setting in requests.USFreightSettings)
                {
                    _priceSettingsInfoProvider.Update(PriceSettingsInfo.Create(
                        setting.CodeName, setting.WeightInKg, setting.MarginInDecimal,
                        setting.AUFreightPerKg, setting.NZFreightPerKg, setting.AUFreightSurcharge, setting.NZFreightSurcharge,
                        setting.ColumnHeader1, setting.ColumnHeader2));    
                }
                

                return Ok(new CommandAPIResult
                {
                   Success = true,
                   Message = "Saved."
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