using System.Collections.Generic;
using C3Apparel.Data.Modules.Classes;

namespace C3Apparel.Features.Admin.ProductPricing;

public class ProductPricingEditViewModel
{
    public int ID { get; set; }
    public Dictionary<string, string> Brands { get; set; }

}