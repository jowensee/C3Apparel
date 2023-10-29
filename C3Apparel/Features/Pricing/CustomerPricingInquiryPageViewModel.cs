using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Products;
using C3Apparel.Frontend.Data.Common;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Pricing;

public class CustomerPricingInquiryPageViewModel
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
    public string Currency { get; set; }
    public string CountryCode { get; set; }
    public bool UserIsAdministrator { get; set; }

    public string JsonBrandsFilterOptions
    {
        get
        {
            if (Brands.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var options = Brands.Select(b => new
            {
                value = b.Value.ToInt(),
                label = b.Text
            });

            return JsonConvert.SerializeObject(options);
        }
    }

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