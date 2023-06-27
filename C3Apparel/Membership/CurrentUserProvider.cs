

using System.Linq;
using System.Threading.Tasks;
using C3Apparel.Areas.Identity.Data;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Pricing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace C3Apparel.Web.Membership
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        
        private readonly UserManager<C3ApparelUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CurrentUserProvider(UserManager<C3ApparelUser> userManager, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }
        public async Task<C3ApparelUser> Get()
        {
            return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            
        }

        public async Task<AccountUser> GetCurrentUserInfo()
        {
            //TODO
            //return new AccountUser("AUClient", "AUClient", "AUClient", "AUClient", AccountConstants.ROLE_AU, "au", false);
            var user = await Get();

            if (user == null)
            {
                return new AccountUser("public");

            }
            
            var role = string.Empty;

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any(a => a == AccountConstants.ROLE_ADMIN))
            {
                role = AccountConstants.ROLE_ADMIN;
            }
            else
            {
                if (roles.Any(a=>a == AccountConstants.ROLE_AU))
                {
                    role = AccountConstants.ROLE_AU;
                }else if (roles.Any(a=>a == AccountConstants.ROLE_NZ))
                {
                    role = AccountConstants.ROLE_NZ;
                }
                
            }

            return new AccountUser(user.UserName, string.Empty, string.Empty, user.Email, role, string.Empty, role == AccountConstants.ROLE_ADMIN);

        }
    }
}