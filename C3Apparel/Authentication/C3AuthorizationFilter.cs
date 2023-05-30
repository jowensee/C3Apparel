using System;
using C3Apparel.Frontend.Data.Membership;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace C3Apparel.Web.Authentication;

public class C3AuthorizationFilter : Attribute, IAuthorizationFilter
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IHttpContextAccessor _httpContextRetriever;

    public C3AuthorizationFilter(ICurrentUserProvider currentUserProvider, IHttpContextAccessor httpContextRetriever)
    {
        _currentUserProvider = currentUserProvider;
        _httpContextRetriever = httpContextRetriever;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {

        var urlReferrer = string.Empty;

        if (_httpContextRetriever.HttpContext.Request.Headers.ContainsKey("Referer"))
        {
            urlReferrer = _httpContextRetriever.HttpContext.Request.Headers["Referer"];
        }
        

        if (string.IsNullOrEmpty(urlReferrer) )
        {
            return;
        }
        var currentUser = _currentUserProvider.GetCurrentUserInfo();
        
        if (currentUser.IsPublicUser)
        {
            context.Result =
                new RedirectResult("/Login");
        }
        else if (string.IsNullOrEmpty(currentUser.CountryRole))
        {
            context.Result = new RedirectResult("/page-not-found");
        }
    }
}