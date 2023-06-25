

using Newtonsoft.Json;

namespace C3Apparel.Web.Features.ProductPricing.API.Requests;

public class UploadPricingsParameters
{
    
    public int BrandId { get; set; }
    public bool DeleteAll { get; set; }
    
}
