using CsvHelper.Configuration.Attributes;

namespace C3Apparel.Data.Products
{
    public class ProductItem
    {
        public int BrandID { get; set; }
        public decimal C3BuyPrice { get; set; }
        public decimal SKUWeight { get; set; }
        public decimal C3OverrideWeight { get; set; }
        [Name("Brand")]
        public string BrandName { get; set; }
        [Name("Style")]
        public string ProductCode { get; set; }
        
        [Name("Collection")]
        public string Collection { get; set; }
        [Name("Description")]
        public string ProductName { get; set; }
        [Name("Sizes")]
        public string ProductSizes { get; set; }
        public string ProductColours { get; set; }
        public decimal FreightSurcharge1 { get; set; }
        [Name("Unit Price 1")]
        public decimal Price1 { get; set; }
        [Name("MOQ Unit 1")]
        public int MinimumOrderQty1 { get; set; }
        public decimal FreightSurcharge2 { get; set; }
        
        [Name("Unit Price 2")]
        public decimal Price2 { get; set; }
        [Name("MOQ Unit 2")]
        public int MinimumOrderQty2 { get; set; }
        public decimal FreightSurcharge3 { get; set; }
        
        [Name("Unit Price 3")]
        public decimal Price3 { get; set; }
        
        [Name("MOQ Unit 3")]
        public int MinimumOrderQty3 { get; set; }
        public decimal FreightSurcharge4 { get; set; }
        
        [Name("Unit Price 4")]
        public decimal Price4 { get; set; }
        [Name("MOQ Unit 4")]
        public int MinimumOrderQty4 { get; set; }

        public string FormatPrice(decimal price)
        {
            return $"${price:#,##0.00}";

        }
    }
}