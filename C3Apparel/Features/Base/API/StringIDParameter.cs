using Newtonsoft.Json;

namespace BlankSiteCore.Features.Base.API;

public class StringIDParameter
{
    [JsonProperty("id")]
    public string Id { get; set; }
}