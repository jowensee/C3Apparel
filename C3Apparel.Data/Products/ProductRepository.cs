using System.Data;
using C3Apparel.Data.Modules.Classes;
using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Pricing;

namespace C3Apparel.Data.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly IPriceListPriceInfoProvider _priceListPriceInfoProvider;
        private readonly IProductPricingInfoProvider _productPricingInfoProvider;
        private readonly IBrandInfoProvider _brandInfoProvider;

        public ProductRepository(IBrandInfoProvider brandInfoProvider, 
            IProductPricingInfoProvider productPricingInfoProvider, IPriceListPriceInfoProvider priceListPriceInfoProvider)
        {
            _brandInfoProvider = brandInfoProvider;
            _productPricingInfoProvider = productPricingInfoProvider;
            _priceListPriceInfoProvider = priceListPriceInfoProvider;
        }

        private ProductItem CreateProductItem(DataRow dr)
        {
            return new ProductItem
            {
                BrandID = dr[nameof(BrandInfo.BrandID)].ToInt(),
                BrandName = dr[nameof(BrandInfo.BrandDisplayName)].ToString(),
                ProductCode = dr[nameof(ProductPricingInfo.ProductPricingC3Style)].ToString(),
                Collection = dr[nameof(ProductPricingInfo.ProductPricingCollection)].ToString(),
                ProductName = dr[nameof(ProductPricingInfo.ProductPricingDescription)].ToString(),
                ProductSizes = dr[nameof(ProductPricingInfo.ProductPricingSizes)].ToString(),
                ProductColours = dr[nameof(ProductPricingInfo.ProductPricingColours)].ToString(),
                ProductColourDesc = dr[nameof(ProductPricingInfo.ProductPricingColourDesc)].ToString(),
                ProductCoo = dr[nameof(ProductPricingInfo.ProductPricingCoo)].ToString(),
                ProductGroup = dr[nameof(ProductPricingInfo.ProductPricingGroup)].ToString(),
                ProductSupplierStyle = dr[nameof(ProductPricingInfo.ProductPricingSupplierStyle)].ToString(),
                C3BuyPrice = dr[nameof(ProductPricingInfo.ProductPricingC3BuyPrice)].ToDecimal(),
                SKUWeight = dr[nameof(ProductPricingInfo.ProductPricingSKUWeight)].ToDecimal(),
                C3OverrideWeight =  dr[nameof(ProductPricingInfo.ProductPricingC3OverrideWeight)].ToDecimal()
            };
        }

        public IEnumerable<BrandItem> GetBrandsWithPricing(bool enabledOnly)
        {
            return _brandInfoProvider.GetBrandsWithPricing(enabledOnly)
                .Select(b => new BrandItem(b.BrandID, b.BrandDisplayName, b.BrandName, b.BrandCurrency,
                    b.BrandPricingDisclaimerTextAU,
                    b.BrandPricingDisclaimerTextNZ));
        }

        public IEnumerable<ProductItem> GetProducts(int brandID, SearchFilter filter)
        {
            return _productPricingInfoProvider.GetProductPricing(brandID, filter)
                .Select(product => CreateProductItem(product));
        }

        public IEnumerable<ProductItem> GetProducts(int brandID, SearchFilter filter, int pageNumber, int itemsPerPage)
        {

            return _productPricingInfoProvider.GetProductPricing(brandID, filter, pageNumber, itemsPerPage)
                .Select(product => CreateProductItem(product));
        }

        public int GetAllProductsCount(int brandID, SearchFilter filter)
        {
            return _productPricingInfoProvider.GetProductPricingCount(brandID, filter);
        }
        
    }
}