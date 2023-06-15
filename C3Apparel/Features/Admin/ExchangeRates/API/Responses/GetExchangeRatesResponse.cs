using System.Collections.Generic;
using BlankSiteCore.Features.Base.API;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.ExchangeRates.API.Responses;

public class ExchangeRateAPIItem
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("sourceCurrency")]
    public string SourceCurrency { get; set; }
    [JsonProperty("nzdValue")]
    public decimal NzdValue { get; set; }
    [JsonProperty("audValue")]
    public decimal AudValue { get; set; }
    [JsonProperty("validFrom")]
    public string ValidFrom { get; set; }
    [JsonProperty("validTo")]
    public string ValidTo { get; set; }
}

public class GetExchangeRatesResponse : BaseListingResponse
{

    [JsonProperty("exchangeRates")]
    public List<ExchangeRateAPIItem> ExchangeRates { get; set; }
    
    
}

