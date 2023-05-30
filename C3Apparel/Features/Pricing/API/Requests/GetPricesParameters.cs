

using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Pricing.API.Requests;

public class GetPricesParameters
{
    [JsonProperty("brandID")]
    public int BrandID { get; set; }
    [JsonProperty("pageNumber")]
    public int PageNumber { get; set; }
    [JsonProperty("itemsPerPage")]
    public int ItemsPerPage { get; set; }
}