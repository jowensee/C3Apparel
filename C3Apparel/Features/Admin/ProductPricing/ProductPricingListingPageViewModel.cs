using System.Collections.Generic;
using System.Linq;
using System.Net;
using C3Apparel.Frontend.Data.Common;
using C3Apparel.Web.Features.ProductPricing.API.Requests;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace C3Apparel.Features.Admin.ProductPricing;

public class ProductPricingListingPageViewModel
{
    public List<ListItem> Brands { get; set; } = new List<ListItem>();
    public GetProductPricingsParameters PageFilters { get; set; }

    public string PageFiltersJson
    {
        get
        {
            if (PageFilters == null)
            {
                return string.Empty;

            }

            return WebUtility.HtmlEncode(JsonConvert.SerializeObject(PageFilters));
        }
    }
}