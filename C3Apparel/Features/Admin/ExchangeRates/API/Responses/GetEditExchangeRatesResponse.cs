using System.Collections.Generic;
using BlankSiteCore.Features.Base.API;
using C3Apparel.Features.Admin.ExchangeRates.API;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.ExchangeRates.API.Responses;


public class GetEditExchangeRateResponse : BaseListingResponse
{

    public ExchangeRatesFullDetail ExchangeRate { get; set; }
}

