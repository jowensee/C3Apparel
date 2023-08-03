

using Newtonsoft.Json;

namespace C3Apparel.Web.Features.PriceList.API.Requests;

public class GetBrandPriceListsParameters
{
    [JsonProperty("pageNumber")]
    public int PageNumber { get; set; }
    [JsonProperty("itemsPerPage")]
    public int ItemsPerPage { get; set; }
}
