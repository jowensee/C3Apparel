using CsvHelper.Configuration.Attributes;

namespace C3Apparel.Features.Admin.ProductPricing.CSV;

public class CSVPProductInquiryItem 
{
    public string Brand { get; set; }
    
    [Name("C-3 Style")]
    public string Style { get; set; }
    public string Collection { get; set; }
    public string Description { get; set; }
    
    [Name("Product Group")]
    public string ProductGroup { get; set; }
    
    public string Sizes { get; set; }
    public string Colours { get; set; }
    [Name("Color Description")]
    public string ColorDescription { get; set; }
    
    [Name("Freight Surcharge")]
    public decimal FreightSurcharge1 { get; set; }
    
    [Name("Unit Price 1")]
    public decimal Price1 { get; set; }
    [Name("MOQ Unit 1")]
    public int MinimumOrderQty1 { get; set; }
    [Name("Unit Price 2")]
    public decimal Price2 { get; set; }
    [Name("MOQ Unit 2")]
    public int MinimumOrderQty2 { get; set; }
    [Name("Unit Price 3")]
    public decimal Price3 { get; set; }
    [Name("MOQ Unit 3")]
    public int MinimumOrderQty3 { get; set; }
    [Name("Unit Price 4")]
    public decimal Price4 { get; set; }
    [Name("MOQ Unit 4")]
    public int MinimumOrderQty4 { get; set; }
    
    
    
   
}