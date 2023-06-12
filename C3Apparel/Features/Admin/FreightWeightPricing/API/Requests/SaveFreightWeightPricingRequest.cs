using System.Collections.Generic;
using C3Apparel.Web.Features.Pricing.API.Responses;
using Newtonsoft.Json;

namespace C3Apparel.Features.Admin.FreightWeighPricing.API.Requests;

public class SaveFreightWeightPricingRequest
{
    [JsonProperty("euroFreightSettings")]
    public List<WeightBasedSettingsResponse> EuroFreightSettings  { get; set; }
    
    [JsonProperty("usFreightSettings")]
    public List<WeightBasedSettingsResponse> USFreightSettings  { get; set; }
}