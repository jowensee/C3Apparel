using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IPriceListPriceInfoProvider
    {
        void Insert(PriceListPriceInfo price);
        IEnumerable<PriceListPriceInfo> GetAllPrices(int versionId, string currency, int brandId, int pageNumber = 0, int itemsPerPage = 0);
        int GetAllPricesCount(int versionId, string currency, int brandId);
        void DeleteAll(int versionId, int brandId, string currency);
    }
}