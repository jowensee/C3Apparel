

using Newtonsoft.Json;

namespace C3Apparel.Web.Features.ExchangeRates.API.Requests;

public class GetExchangeRatesParameters
{
    [JsonProperty("pageNumber")]
    public int PageNumber { get; set; }
    [JsonProperty("itemsPerPage")]
    public int ItemsPerPage { get; set; }
}

