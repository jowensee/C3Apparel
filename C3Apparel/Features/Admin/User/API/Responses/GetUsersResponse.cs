using System.Collections.Generic;
using BlankSiteCore.Features.Base.API;
using Newtonsoft.Json;

namespace C3Apparel.Web.Features.User.API.Responses;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class UserAPIItem
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
}

public class GetUsersResponse : BaseListingResponse
{

    [JsonProperty("users")]
    public List<UserAPIItem> Users { get; set; }
    
    
}

