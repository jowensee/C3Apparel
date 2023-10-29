

using System.Collections.Generic;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Pricing.API.Requests;

public class SearchPriceListFilterParameter
{
    [JsonProperty("brands")]
    public List<int> Brands { get; set; }
    
    [JsonProperty("collection")]
    public string Collection { get; set; }
    
    [JsonProperty("c3Style")]
    public string C3Style { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [JsonProperty("productGroup")]
    public string ProductGroup { get; set; }
    
    [JsonProperty("sizes")]
    public string Sizes { get; set; }
    
    [JsonProperty("Colours")]
    public string Colours { get; set; }
    
    [JsonProperty("colourDescriptions")]
    public string ColourDescriptions { get; set; }

}

public class SearchPriceListParameters
{
    [JsonProperty("currency")]
    public string Currency { get; set; }
    
    [JsonProperty("filters")]
    public SearchPriceListFilterParameter Filters { get; set; }
    
    [JsonProperty("pageNumber")]
    public int PageNumber { get; set; }
    
    [JsonProperty("itemsPerPage")]
    public int ItemsPerPage { get; set; }
}