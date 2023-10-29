using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Filters
{
    public class SearchPriceListFilter
    {
        public List<int> Brands { get; set; }
        public string C3Style { get; set; }
        public string Collection { get; set; }
        public string Description { get; set; }
        public string ProductGroup { get; set; }
        public string Sizes { get; set; }
        public string Colour { get; set; }
        public string ColourDescription { get; set; }
    }
}