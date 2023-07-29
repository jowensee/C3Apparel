namespace C3Apparel.Data.Modules.Classes
{
    /*
     *	
	[PriceVersionID] [int] NOT NULL,
	[PriceCurrency] [nvarchar](200) NOT NULL,
	[PriceBrandID] [int] NOT NULL,
	[PriceBrandName] [nvarchar](200) NULL,
	[PriceCollection] [nvarchar](500) NULL,
	[PriceC3Style] [nvarchar](500) NOT NULL,
	[PriceSupplierStyle] [nvarchar](200) NULL,
	[PriceDescription] [nvarchar](500) NOT NULL,
	[PriceCoo] [nvarchar](200) NULL,
	[PriceGroup] [nvarchar](200) NOT NULL,
	[PriceSizes] [nvarchar](200) NOT NULL,
	[PriceColours] [nvarchar](200) NOT NULL,
	[PriceColourDesc] [nvarchar](max) NULL,
	[PriceCol1FreightSurcharge] [decimal](19, 4) NULL,
	[PriceCol1UnitPrice] [decimal](19, 3) NOT NULL,
	[PriceCol1MOQUnit] [int] NULL,
	[PriceCol2FreightSurcharge] [decimal](19, 4) NULL,
	[PriceCol2UnitPrice] [decimal](19, 3) NOT NULL,
	[PriceCol2MOQUnit] [int] NULL,
	[PriceCol3FreightSurcharge] [decimal](19, 4) NULL,
	[PriceCol3UnitPrice] [decimal](19, 3) NOT NULL,
	[PriceCol3MOQUnit] [int] NULL,
	[PriceCol4FreightSurcharge] [decimal](19, 4) NULL,
	[PriceCol4UnitPrice] [decimal](19, 3) NOT NULL,
	[PriceCol4MOQUnit] [int] NULL
	)
GO

     * 
     */
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

	    public string FormatPrice(decimal price)
	    {
		    return $"${price:#,##0.00}";

	    }
    }
}