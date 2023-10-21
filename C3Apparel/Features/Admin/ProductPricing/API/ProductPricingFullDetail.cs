using Newtonsoft.Json;

namespace C3Apparel.Features.Admin.ProductPricing.API;

public class ProductPricingFullDetail
{
    public int Id { get; set; }
    public string Status { get; set; }
    public int BrandId { get; set; }
    public string C3Style { get; set; }
    public string Collection { get; set; }
    public string SupplierStyle { get; set; }
    public string Description { get; set; }
    public string Coo { get; set; }
    public string ProductGroup { get; set; }
    public string Sizes { get; set; }

    public string ProductSubCategory { get; set; }
    public string AllSizes { get; set; }
    public string Colour { get; set; }
    public string ColourDescription { get; set; }
    public decimal C3BuyPrice { get; set; }
    public decimal SkuWeight { get; set; }
    public decimal C3OverrideWeight { get; set; }

}