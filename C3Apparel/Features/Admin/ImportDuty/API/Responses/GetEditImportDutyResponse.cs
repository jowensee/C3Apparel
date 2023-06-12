using System.Collections.Generic;
using BlankSiteCore.Features.Base.API;
using C3Apparel.Features.Admin.ImportDuty.API;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.AdminImportDuty.API.Responses;


public class GetEditImportDutyResponse 
{

    [JsonProperty("importDutyAU")]
    public decimal ImportDutyAU { get; set; }
    
    [JsonProperty("importDutyNZ")]
    public decimal ImportDutyNZ { get; set; }
}

