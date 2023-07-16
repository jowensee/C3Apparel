

using Newtonsoft.Json;

namespace C3Apparel.Web.Features.User.API.Requests;

public class GetUserParameters
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
    [JsonProperty("filterUserName")]
    public string FilterUserName { get; set; }
    [JsonProperty("filterRole")]
    public string FilterRole { get; set; }
}