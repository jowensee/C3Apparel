using Newtonsoft.Json;

namespace BlankSiteCore.Features.Base.API;

public class IDParameter
{
    [JsonProperty("id")]
    public int Id { get; set; }
}