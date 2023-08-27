using C3Apparel.Features.Base.API;

namespace C3Apparel.Web.Features.ExchangeRates.API.Responses;


public class GetEditExchangeRateResponse : BaseListingResponse
{

    public ExchangeRateAPIItem Rate { get; set; }
}

