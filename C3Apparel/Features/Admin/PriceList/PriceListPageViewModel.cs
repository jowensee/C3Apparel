using System.Collections.Generic;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Frontend.Data.Common;

namespace C3Apparel.Features.Admin.PriceList;

public class PricingListPageViewModel
{
    public IEnumerable<ListItem> Brands { get; set; }
}