using System.Collections.Generic;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Pricing.API.Responses;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class PricingAPIItem
{
    [JsonProperty("brand")]
    public string Brand { get; set; }

    [JsonProperty("productCode")]
    public string ProductCode { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("sizes")]
    public string Sizes { get; set; }

    [JsonProperty("colours")]
    public string Colours { get; set; }
    

    [JsonProperty("unitPrice1")]
    public string UnitPrice1 { get; set; }

    [JsonProperty("moq1")]
    public int Moq1 { get; set; }
    
    [JsonProperty("freightSurcharge1")]
    public string FreightSurcharge1 { get; set; }

    [JsonProperty("unitPrice2")]
    public string UnitPrice2 { get; set; }

    [JsonProperty("moq2")]
    public int Moq2 { get; set; }
    
    [JsonProperty("freightSurcharge2")]
    public string FreightSurcharge2 { get; set; }

    [JsonProperty("unitPrice3")]
    public string UnitPrice3 { get; set; }

    [JsonProperty("moq3")]
    public int Moq3 { get; set; }
    [JsonProperty("freightSurcharge3")]
    
    public string FreightSurcharge3 { get; set; }

    [JsonProperty("unitPrice4")]
    public string UnitPrice4 { get; set; }

    [JsonProperty("moq4")]
    public int Moq4 { get; set; }
    
    [JsonProperty("freightSurcharge4")]
    public string FreightSurcharge4 { get; set; }
}

public class GetPricingsResponse
{
    [JsonProperty("totalPage")]
    public int TotalPage { get; set; }

    [JsonProperty("pricings")]
    public List<PricingAPIItem> Pricings { get; set; }
    
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }
    
    [JsonProperty("settingsGuid")]
    public string SettingsGuid { get; set; }
}

