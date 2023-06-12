using System.Collections.Generic;
using BlankSiteCore.Features.Base.API;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.ExchangeRates.API.Responses;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class ExchangeRateAPIItem
{
    [JsonProperty("exchangeRateId")]
    public int ExchangeRateId { get; set; }
    
}

public class GetExchangeRatesResponse : BaseListingResponse
{

    [JsonProperty("exchangeRates")]
    public List<ExchangeRateAPIItem> ExchangeRates { get; set; }
    
    
}

