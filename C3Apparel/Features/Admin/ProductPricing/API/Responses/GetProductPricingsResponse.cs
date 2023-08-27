using System.Collections.Generic;
using C3Apparel.Features.Base.API;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.ProductPricing.API.Responses;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class ProductPricingAPIItem
{
    public int Id { get; set; }
    public string SupplierName { get; set; }
    public string C3Style { get; set; }
    public string Collection { get; set; }
    public string SupplierStyle { get; set; }
    public string Description { get; set; }
    public string Coo { get; set; }
    public string ProductGroup { get; set; }
    public string Sizes { get; set; }
    public string ProductColours { get; set; }
    public string ColourDescription { get; set; }
    public string C3BuyPrice { get; set; }
    public string SkuWeight { get; set; }
    public string C3OverrideWeight { get; set; }
}

public class GetProductPricingsResponse : BaseListingResponse
{

    [JsonProperty("pricings")]
    public List<ProductPricingAPIItem> Pricings { get; set; }
    
    
}

