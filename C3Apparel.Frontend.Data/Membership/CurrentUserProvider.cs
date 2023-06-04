

using C3Apparel.Data.Pricing;

namespace C3Apparel.Frontend.Data.Membership
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        public CurrentUserInfo Get()
        {
            //TODO
            return null;// MembershipContext.AuthenticatedUser;
        }

        public AccountUser GetCurrentUserInfo()
        {
            //TODO
            return new AccountUser("AUClient", "AUClient", "AUClient", "AUClient", AccountConstants.ROLE_AU, "au", false);
            /*var user = Get();

            var role = string.Empty;
            if (user.IsInRole(AccountConstants.ROLE_AU, SiteContext.CurrentSiteName))
            {
                role = AccountConstants.ROLE_AU;
            }else if (user.IsInRole(AccountConstants.ROLE_NZ, SiteContext.CurrentSiteName))
            {
                role = AccountConstants.ROLE_NZ;
            }

            return new AccountUser(user.UserName, user.FirstName, user.LastName, user.Email, role, user.FullName, user.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin));
*/
        }
    }
}