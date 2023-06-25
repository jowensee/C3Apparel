using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Products;
using C3Apparel.Frontend.Data.Common;

namespace C3Apparel.Web.Features.Admin.ProductPricing;

public class InternalPriceListingPageViewModel
{
    public IEnumerable<ListItem> Brands { get; set; } = Enumerable.Empty<ListItem>();
    public IEnumerable<ProductItem> Products { get; set; } = Enumerable.Empty<ProductItem>();
    public string ErrorMessage { get; set; }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);


}