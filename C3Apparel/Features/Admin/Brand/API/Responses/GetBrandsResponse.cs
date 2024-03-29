using System.Collections.Generic;
using C3Apparel.Features.Base.API;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Brand.API.Responses;

public class BrandAPIItem
{
    [JsonProperty("brandId")]
    public int BrandId { get; set; }
    
    [JsonProperty("brand")]
    public string Brand { get; set; }

    [JsonProperty("focus")]
    public string Focus { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("publishDate")]
    public string PublishDate { get; set; }
}

public class GetBrandsResponse : BaseListingResponse
{

    [JsonProperty("brands")]
    public List<BrandAPIItem> Brands { get; set; }
    
    
}

