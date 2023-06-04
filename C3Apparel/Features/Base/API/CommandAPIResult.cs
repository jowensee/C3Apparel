using Newtonsoft.Json;

namespace BlankSiteCore.Features.Base.API;

public class CommandAPIResult
{
    [JsonProperty("success")]
    public bool Success { get; set; }
    [JsonProperty("redirectUrl")]
    public string RedirectUrl { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
}