using System.Text;
using Microsoft.AspNetCore.Http;

namespace C3Apparel.Infrastructure;

public class SessionService : ISessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public SessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public const string KeyProductPricingParameter = "Key_ProductMaintenanceFilter";

    public string GetString(string sessionKey)
    {

        var bytes = _httpContextAccessor.HttpContext.Session.Get(sessionKey);
        return bytes == null ? string.Empty : Encoding.Default.GetString(bytes);
    }

    public void SetValue(string sessionKey, string value)
    {
        var bytes = Encoding.ASCII.GetBytes(value);

        if (bytes.Length == 0)
        {
            _httpContextAccessor.HttpContext.Session.Remove(sessionKey);
    
        }
        _httpContextAccessor.HttpContext.Session.Set(sessionKey, bytes);

    }
}