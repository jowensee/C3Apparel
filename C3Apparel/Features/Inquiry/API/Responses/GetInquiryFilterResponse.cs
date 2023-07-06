using System.Collections.Generic;
using C3Apparel.Data.Pricing;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Pricing.API.Responses;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class WeightBasedSettingsResponse
{
    public string CodeName { get; set; }
    public decimal WeightInKg { get; set; }
    public decimal MarginInDecimal { get; set; }
    public decimal AUFreightPerKg { get; set; }
    public decimal NZFreightPerKg { get; set; }

    public decimal AUFreightSurcharge { get; set; }
    public decimal NZFreightSurcharge { get; set; }
    public string ColumnHeader1 { get; set; }
    public string ColumnHeader2 { get; set; }

    public static WeightBasedSettingsResponse Create(PriceWeightbasedSettings a)
    {
        return new WeightBasedSettingsResponse
        {
            CodeName = a.KeyName,
            WeightInKg = a.WeightInKg,
            MarginInDecimal = a.MarginInDecimal,
            AUFreightPerKg = a.AUFreightPerKg,
            NZFreightPerKg = a.NZFreightPerKg,
            AUFreightSurcharge = a.FreightSurcharge(CurrencyConstants.AUD),
            NZFreightSurcharge = a.FreightSurcharge(CurrencyConstants.NZD),
            ColumnHeader1 = a.ColumnHeader1,
            ColumnHeader2 = a.ColumnHeader2
        };
    }
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

