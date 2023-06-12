using System.Collections.Generic;
using BlankSiteCore.Features.Base.API;
using C3Apparel.Features.Admin.FreightWeighPricing.API;
using C3Apparel.Web.Features.Pricing.API.Responses;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.AdminImportDuty.API.Responses;


public class GetEditFreightWeighPricingResponse 
{
    [JsonProperty("euroFreightSettings")]
    public List<WeightBasedSettingsResponse> EuroFreightSettings  { get; set; }
    
    [JsonProperty("usFreightSettings")]
    public List<WeightBasedSettingsResponse> USFreightSettings  { get; set; }
}

