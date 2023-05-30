using CMS.DataEngine;

namespace C3Apparel.Data.Kentico.Modules.Classes
{
    /// <summary>
    /// Class providing <see cref="ProductInfo"/> management.
    /// </summary>
    [ProviderInterface(typeof(IProductInfoProvider))]
    public partial class ProductInfoProvider : AbstractInfoProvider<ProductInfo, ProductInfoProvider>, IProductInfoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductInfoProvider"/> class.
        /// </summary>
        public ProductInfoProvider()
            : base(ProductInfo.TYPEINFO)
        {
        }
    }
}