using System.Collections.Generic;

namespace C3Apparel.Data.Products
{
    public interface IBrandRepository
    {
        IEnumerable<BrandItem> GetAllBrands();
        BrandItem GetBrand(int brandId);
        BrandPricing GetBrandPricingInfo(int brandId, string targetCurrency);
    }
}