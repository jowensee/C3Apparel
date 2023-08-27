using Newtonsoft.Json;

namespace C3Apparel.Features.Base.API;

public class IDParameter
{
    [JsonProperty("id")]
    public int Id { get; set; }
}