using System;
using C3Apparel.Web.Membership;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace C3Apparel.Web.Authentication;

public class AuthentictedAuthorizationFilter : Attribute, IAuthorizationFilter
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IHttpContextAccessor _httpContextRetriever;

    public AuthentictedAuthorizationFilter(ICurrentUserProvider currentUserProvider, IHttpContextAccessor httpContextRetriever)
    {
        _currentUserProvider = currentUserProvider;
        _httpContextRetriever = httpContextRetriever;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {

        var currentUser = _currentUserProvider.GetCurrentUserInfo().Result;
        
        if (currentUser.IsPublicUser)
        {
            context.Result =
                new RedirectResult("/Login");
        }
    }
}