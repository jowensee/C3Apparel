using System;

namespace C3Apparel.Data.Modules.Classes
{
    public class BrandInfo
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }
        public string BrandDisplayName { get; set; }
        public string BrandCurrency { get; set; }
        public string BrandHomepage { get; set; }
        public string BrandDescription { get; set; }
        public string BrandPricingDisclaimerTextAU { get; set; }
        public string BrandPricingDisclaimerTextNZ { get; set; }
        public bool BrandEnabled { get; set; }
        public string BrandFocus { get; set; }
        public string BrandBusinessName { get; set; }
        public DateTime BrandPriceListPublishedDate { get; set; }
    }
}