namespace C3Apparel.Data.Pricing
{
    public interface IPriceListService
    {
        string SavePriceListToPriceListTable(int versionId, string currency, int brandId);
    }
}