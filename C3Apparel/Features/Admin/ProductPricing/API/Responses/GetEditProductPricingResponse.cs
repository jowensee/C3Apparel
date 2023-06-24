using System.Collections.Generic;
using BlankSiteCore.Features.Base.API;
using C3Apparel.Features.Admin.ProductPricing.API;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.ProductPricing.API.Responses;


public class GetEditProductPricingResponse : BaseListingResponse
{

    public ProductPricingFullDetail ProductPricing { get; set; }
}

