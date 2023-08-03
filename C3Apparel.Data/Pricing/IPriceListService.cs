namespace C3Apparel.Data.Pricing
{
    public interface IPriceListService
    {
        (string, int)  SavePriceListToPriceListTable(int versionId, string currency, int brandId);
    }
}