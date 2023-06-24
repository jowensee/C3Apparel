

using Newtonsoft.Json;

namespace C3Apparel.Web.Features.ProductPricing.API.Requests;

public class GetProductPricingsParameters
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
    [JsonProperty("filterSupplier")]
    public int FilterSupplier { get; set; }
    [JsonProperty("filterC3Style")]
    public string FilterC3Style { get; set; }
    [JsonProperty("filterCollection")]
    public string FilterCollection { get; set; }
    [JsonProperty("filterSupplierStyle")]
    public string FilterSupplierStyle { get; set; }
    [JsonProperty("filterDescription")]
    public string FilterDescription { get; set; }
    [JsonProperty("filterCOO")]
    public string FilterCOO { get; set; }
    [JsonProperty("filterProductGroup")]
    public string FilterProductGroup { get; set; }
    [JsonProperty("filterSizes")]
    public string FilterSizes { get; set; }
    [JsonProperty("filterColour")]
    public string FilterColour { get; set; }
}

/*
 {"filters":{"filterBrandId":"1","filterC3Style":"","filterCollection":"","filterSupplierStyle":"","filterDescription":"","filterCoo":"","filterProductGroup":"","filterSizes":"","filterColour":""},"pageNumber":1,"itemsPerPage":5}
 */