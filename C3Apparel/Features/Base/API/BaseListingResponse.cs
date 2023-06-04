using Newtonsoft.Json;

namespace BlankSiteCore.Features.Base.API;

public abstract class BaseListingResponse
{
    [JsonProperty("totalPage")]
    public int TotalPage { get; set; }
    
    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }
}