using Newtonsoft.Json;

namespace C3Apparel.Features.Base.API;

public class StringIDParameter
{
    [JsonProperty("id")]
    public string Id { get; set; }
}