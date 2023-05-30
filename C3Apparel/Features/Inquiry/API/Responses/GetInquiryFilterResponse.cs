using System.Collections.Generic;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Pricing.API.Responses;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class WeightBasedSettingsResponse
{
    public decimal WeightInKg { get; set; }
    public decimal MarginInDecimal { get; set; }
        
    public decimal AUFreightPerKg { get; set; }
    public decimal NZFreightPerKg { get; set; }

    public decimal AUFreightSurcharge { get; set; }
    public decimal NZFreightSurcharge { get; set; }
}

public class GetInquiryFilterResponse
{
    [JsonProperty("rateAuEuro")]
    public decimal RateAuEuro { get; set; }
    [JsonProperty("rateAuUsd")]
    public decimal RateAuUsd { get; set; }
    [JsonProperty("rateNzEuro")]
    public decimal RateNzEuro { get; set; }
    [JsonProperty("rateNzUsd")]
    public decimal RateNzUsd { get; set; }
    
    [JsonProperty("auImportDuty")]
    public decimal AUImportDuty { get; set; }
    
    [JsonProperty("nzImportDuty")]
    public decimal NZImportDuty { get; set; }
    
    [JsonProperty("euroFreightSettings")]
    public List<WeightBasedSettingsResponse> EuroFreightSettings  { get; set; }
    
    [JsonProperty("usFreightSettings")]
    public List<WeightBasedSettingsResponse> USFreightSettings  { get; set; }
    
}

