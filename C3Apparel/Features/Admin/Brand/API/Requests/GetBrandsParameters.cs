

using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Pricing.API.Requests;

public class GetBrandsParameters
{
    [JsonProperty("pageNumber")]
    public int PageNumber { get; set; }
    [JsonProperty("itemsPerPage")]
    public int ItemsPerPage { get; set; }
    
    [JsonProperty("filters")]
    public Filters Filters { get; set; }
}

public class Filters
{
    [JsonProperty("filterName")]
    public string FilterName { get; set; }
    [JsonProperty("filterFocus")]
    public string FilterFocus { get; set; }
    [JsonProperty("filterCurrency")]
    public string FilterCurrency { get; set; }
}