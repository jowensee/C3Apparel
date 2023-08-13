using System.Collections.Generic;
using BlankSiteCore.Features.Base.API;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Pricing.API.Responses;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class ProductInquiryAPIItem : PricingAPIItem
{
    [JsonProperty("productGroup")]
    public string ProductGroup { get; set; }

    [JsonProperty("colorDescription")]
    public string ColorDescription { get; set; }
}

public class GetProductInquiryResponse : BaseListingResponse
{

    [JsonProperty("pricings")]
    public List<ProductInquiryAPIItem> Pricings { get; set; }
    
    
    [JsonProperty("settingsGuid")]
    public string SettingsGuid { get; set; }
}

