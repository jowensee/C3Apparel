using System.Collections.Generic;
using C3Apparel.Data.Modules.Filters;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IPriceListPriceInfoProvider
    {
        void Insert(PriceListPriceInfo price);
        IEnumerable<PriceListPriceInfo> GetAllPrices(int versionId, string currency, int brandId, int pageNumber = 0, int itemsPerPage = 0);
        int GetAllPricesCount(int versionId, string currency, int brandId);
        void DeleteAll(int versionId, int brandId, string currency);
        IEnumerable<PriceListPriceInfo> SearchPriceList(int versionId, string currency, SearchPriceListFilter filter, int pageNumber = 0, int itemsPerPage = 0);
        int SearchPriceListCount(int versionId, string currency, SearchPriceListFilter filter);
    }
}