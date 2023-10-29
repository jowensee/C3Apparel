using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Pricing.API.Responses;

public class DownloadCSVResponse
{
    
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }
}