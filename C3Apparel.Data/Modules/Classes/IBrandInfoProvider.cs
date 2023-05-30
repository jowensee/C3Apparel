using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IBrandInfoProvider
    {
        IEnumerable<BrandInfo> GetAllBrands();
        BrandInfo GetBrand(int brandId);
        
        IEnumerable<BrandInfo> GetBrandsWithPricing();
    }
}