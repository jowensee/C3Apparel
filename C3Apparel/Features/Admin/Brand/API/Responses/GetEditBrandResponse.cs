using System.Collections.Generic;
using C3Apparel.Features.Base.API;
using C3Apparel.Features.Admin.Brand.API;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.Brand.API.Responses;


public class GetEditBrandResponse : BaseListingResponse
{

    public BrandFullDetail Brand { get; set; }
}

