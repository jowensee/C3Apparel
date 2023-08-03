using System.Globalization;
using CsvHelper.Configuration;

namespace C3Apparel.Data.Products
{
    public class ProductItemCSVMap: ClassMap<ProductItem>
    {
        public ProductItemCSVMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.ProductColourDesc).Ignore();
            Map(m => m.ProductCoo).Ignore();
            Map(m => m.ProductSupplierStyle).Ignore();
            Map(m => m.ProductGroup).Ignore();
            Map(m => m.C3BuyPrice).Ignore();
            Map(m => m.C3OverrideWeight).Ignore();
            Map(m => m.SKUWeight).Ignore();
            Map(m => m.FreightSurcharge2).Ignore();
            Map(m => m.FreightSurcharge3).Ignore();
            Map(m => m.FreightSurcharge4).Ignore();
            Map(m => m.Price1).TypeConverterOption.Format("0.00");
            Map(m => m.Price2).TypeConverterOption.Format("0.00");
            Map(m => m.Price3).TypeConverterOption.Format("0.00");
            Map(m => m.Price4).TypeConverterOption.Format("0.00");
        }
    }

}