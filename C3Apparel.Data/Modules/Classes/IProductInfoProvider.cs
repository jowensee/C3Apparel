using CMS.DataEngine;

namespace C3Apparel.Data.Kentico.Modules.Classes
{
    /// <summary>
    /// Declares members for <see cref="ProductInfo"/> management.
    /// </summary>
    public partial interface IProductInfoProvider : IInfoProvider<ProductInfo>, IInfoByIdProvider<ProductInfo>
    {
    }
}