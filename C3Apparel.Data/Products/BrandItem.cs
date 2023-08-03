using C3Apparel.Data.Pricing;

namespace C3Apparel.Data.Products
{
    public class BrandItem
    {
        public int BrandID { get; }
        public string BrandName { get; }
        public string BrandCodeName { get; }
        public string BrandCurrency { get; }
        public string DisclaimerTextAU { get; }
        public string DisclaimerTextNZ { get; }

        public string BrandRegionCode
        {
            get
            {
                switch (BrandCurrency)
                {
                    case CurrencyConstants.USD:
                        return Region.CODE_US;
                    case CurrencyConstants.EURO:
                        return Region.CODE_EUROPE;
                }

                return string.Empty;
            }
        }

        public BrandItem(int brandId, string brandName, string brandCodeName, string brandCurrency, string disclaimerTextAu, string disclaimerTextNz)
        {
            BrandID = brandId;
            BrandName = brandName;
            BrandCurrency = brandCurrency;
            DisclaimerTextAU = disclaimerTextAu;
            DisclaimerTextNZ = disclaimerTextNz;
            BrandCodeName = brandCodeName;
        }
    }
}