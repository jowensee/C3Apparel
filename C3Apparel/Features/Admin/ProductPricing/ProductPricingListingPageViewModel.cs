using System.Collections.Generic;
using System.Linq;
using C3Apparel.Frontend.Data.Common;

namespace C3Apparel.Features.Admin.ProductPricing;

public class ProductPricingListingPageViewModel
{
    public IEnumerable<ListItem> Brands { get; set; } = Enumerable.Empty<ListItem>();
}