using Newtonsoft.Json;

namespace C3Apparel.Features.Admin.Brand.API;

public class BrandFullDetail
{
    [JsonProperty("brandId")]
    public int BrandId { get; set; }
    
    [JsonProperty("brand")]
    public string Brand { get; set; }
    [JsonProperty("codeName")]
    public string CodeName { get; set; }
    
    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("focus")]
    public string Focus { get; set; }
    
    [JsonProperty("website")]
    public string Website { get; set; }
    
    [JsonProperty("businessName")]
    public string BusinessName { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("enabled")]
    public bool Enabled { get; set; }

    [JsonProperty("publishDate")]
    public string PublishDate { get; set; }
    [JsonProperty("disclaimerTextAU")]
    public string DisclaimerTextAU { get; set; }
    [JsonProperty("disclaimerTextNZ")]
    public string DisclaimerTextNZ { get; set; }
}