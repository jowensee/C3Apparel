using System.Collections.Generic;
using C3Apparel.Data.Pricing;

namespace C3Apparel.Data.Products
{
    public interface IProductRepository
    {
        IEnumerable<BrandItem> GetBrandsWithPricing();
        IEnumerable<ProductItem> GetProducts(int brandID, SearchFilter filter);
        IEnumerable<ProductItem> GetProducts(int brandID, SearchFilter filter, int pageNumber, int itemsPerPage);
        int GetAllProductsCount(int brandID, SearchFilter filter);

    }
}