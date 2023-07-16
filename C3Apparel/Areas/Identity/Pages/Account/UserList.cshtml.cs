// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using C3Apparel.Areas.Identity.Data;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Web.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace C3Apparel.Areas.Identity.Pages.Account
{
    [TypeFilter(typeof(AdminAuthorizationFilter))]
    public class UserListModel : PageModel
    {
        private readonly UserManager<C3ApparelUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IUserProvider _userProvider;
        
        public UserListModel(
            UserManager<C3ApparelUser> userManager,
            ILogger<RegisterModel> logger, IUserProvider userProvider)
        {
            _userManager = userManager;
            _logger = logger;
            _userProvider = userProvider;
        }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        
        public string Message { get; set; }


        public IEnumerable<UserAccount> Users { get; set; }
        
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Users = _userProvider.GetAllUsers(null);
        }
    }
}
