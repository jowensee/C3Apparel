using Newtonsoft.Json;

namespace C3Apparel.Features.Admin.ExchangeRates.API;

public class ExchangeRatesFullDetail
{
    [JsonProperty("exchangeRatesId")]
    public int ExchangeRatesId { get; set; }
    
}