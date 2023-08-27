using System.Collections.Generic;
using C3Apparel.Features.Base.API;
using Newtonsoft.Json;

namespace C3Apparel.Features.Admin.PriceList.API.Responses;

public class BrandPriceListAPIItem
{
    [JsonProperty("brandId")]
    public int BrandId { get; set; }
    
    [JsonProperty("brand")]
    public string Brand { get; set; }

   [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("c3PublishDate")]
    public string C3PublishDate { get; set; }
    
    [JsonProperty("lastPublishDateTime")]
    public string LastPublishDateTime { get; set; }
    
    public string PDFAUPriceUrl { get; set; }
    public string PDFNZPriceUrl { get; set; }
    public string CSVAUPriceUrl { get; set; }
    public string CSVNZPriceUrl { get; set; }
}

public class GetBrandsPriceListResponse : BaseListingResponse
{
    [JsonProperty("brands")]
    public List<BrandPriceListAPIItem> Brands { get; set; }
}