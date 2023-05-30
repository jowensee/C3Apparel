using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Frontend.Data.Common;

namespace C3Apparel.Web.Features.Pricing;

public class PriceListingPageViewModel
{
    public IEnumerable<ListItem> Brands { get; set; } = Enumerable.Empty<ListItem>();
    public IEnumerable<ProductItem> Products { get; set; } = Enumerable.Empty<ProductItem>();
    public string ErrorMessage { get; set; }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public bool PriceCol1HasFreightSurcharge { get; set; }
    public bool PriceCol2HasFreightSurcharge { get; set; }
    public bool PriceCol3HasFreightSurcharge { get; set; }
    public bool PriceCol4HasFreightSurcharge { get; set; }
    public string CountryName { get; set; }
    
    public Dictionary<string, PriceWeightbasedSettings> PriceWeightBasedSettings { get; set; }

    public string DisclaimerText { get; set; }
    public string CurrentBrandName { get; set; }
    public string CurrentBrandRegionCode { get; set; }
    public string GetColumnHeader(int headerIndex, string priceKey)
    {
        if (PriceWeightBasedSettings == null || !PriceWeightBasedSettings.ContainsKey(priceKey))
        {
            return string.Empty;
        }

        switch (headerIndex)
        {
            case 1:
                return PriceWeightBasedSettings[priceKey].ColumnHeader1;
            case 2:
                return PriceWeightBasedSettings[priceKey].ColumnHeader2;
            default:
                return string.Empty;
        }
    }
}