using System;
using System.Collections.Generic;
using System.Data;
using C3Apparel.Data.Modules.Filters;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IBrandInfoProvider
    {
        IEnumerable<BrandInfo> GetAllBrands(bool includeDisabled = false);
        BrandInfo GetBrand(int brandId);
        
        IEnumerable<BrandInfo> GetBrandsWithPricing(bool enabledOnly);
        IEnumerable<BrandInfo> GetAllBrands(BrandFilter filter, int pageNumber = 0, int itemsPerPage = 0);
        int GetAllBrandsCount(BrandFilter filter);
        void Delete(int id);
        void InsertBrand(BrandInfo brand);
        void UpdateBrand(BrandInfo brand);
        void SaveLastPublishedDate(int brandId, DateTime dt);
    }
}