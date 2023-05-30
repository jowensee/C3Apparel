using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Classes
{
    public class BrandInfoProvider : IBrandInfoProvider
    {
        public IEnumerable<BrandInfo> GetAllBrands()
        {
            //brandInfoProvider.Get().OrderBy(nameof(BrandInfo.BrandDisplayName))
            throw new System.NotImplementedException();
        }

        public BrandInfo GetBrand(int brandId)
        {
            //_brandInfoProvider.Get(brandId);
            throw new System.NotImplementedException();
        }

        public IEnumerable<BrandInfo> GetBrandsWithPricing()
        {
            /*
             *_brandInfoProvider.Get()
                .WhereTrue(nameof(BrandInfo.BrandEnabled)).WhereIn(nameof(BrandInfo.BrandID),
                    _productPricingInfoProvider.Get().Select(p => p.ProductPricingSupplierID).Distinct().ToList())
                .OrderBy(b => b.BrandName)
             * 
             */
            throw new System.NotImplementedException();
        }
    }
}