using Newtonsoft.Json;

namespace C3Apparel.Features.Admin.ImportDuty.API.Requests;

public class SaveImportDutyRequest
{
    [JsonProperty("importDutyAU")]
    public decimal ImportDutyAU { get; set; }
    
    [JsonProperty("importDutyNZ")]
    public decimal ImportDutyNZ { get; set; }
}