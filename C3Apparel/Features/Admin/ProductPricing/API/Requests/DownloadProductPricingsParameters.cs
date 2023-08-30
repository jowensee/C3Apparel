

using Newtonsoft.Json;

namespace C3Apparel.Web.Features.ProductPricing.API.Requests;

public class DownloadProductPricingsParameters
{
    [JsonProperty("filters")]
    public Filters Filters { get; set; }
}

