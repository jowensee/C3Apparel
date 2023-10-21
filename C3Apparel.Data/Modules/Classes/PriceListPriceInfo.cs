namespace C3Apparel.Data.Modules.Classes
{
    public class PriceListPriceInfo
    {
	    public int PriceVersionID { get; set; }
	    public string PriceCurrency { get; set; }
	    public int PriceBrandID { get; set; }
	    public string PriceBrandName { get; set; }
	    public string PriceCollection { get; set; }
	    public string PriceC3Style { get; set; }
	    public string PriceDescription { get; set; }
	    public string PriceSizes { get; set; }
	    public string PriceColours { get; set; }
	    public decimal PriceCol1FreightSurcharge { get; set; }
	    public decimal PriceCol1UnitPrice { get; set; }
	    public int PriceCol1MOQUnit { get; set; }
	    public decimal PriceCol2FreightSurcharge { get; set; }
	    public decimal PriceCol2UnitPrice { get; set; }
	    public int PriceCol2MOQUnit { get; set; }
	    public decimal PriceCol3FreightSurcharge { get; set; }
	    public decimal PriceCol3UnitPrice { get; set; }
	    public int PriceCol3MOQUnit { get; set; }
	    public decimal PriceCol4FreightSurcharge { get; set; }
	    public decimal PriceCol4UnitPrice { get; set; }
	    public int PriceCol4MOQUnit { get; set; }
	    public string PriceSupplierStyle { get; set; }
	    public string PriceCoo { get; set; }
	    public string PriceGroup { get; set; }
	    public string PriceColourDesc { get; set; }
		public string PriceSubCategory { get; set; }
		public string PriceAllSizes { get; set; }

		public string FormatPrice(decimal price)
	    {
		    return $"${price:#,##0.00}";

	    }
    }
}