using System.Collections.Generic;
using C3Apparel.Data.Modules.Classes;

namespace C3Apparel.Features.Admin.Brand;

public class BrandEditViewModel
{
    public BrandInfo EditItem { get; set; }
    public Dictionary<string, string> OptionsFocus { get; set; }
    public Dictionary<string, string> OptionsCurrency { get; set; }
}