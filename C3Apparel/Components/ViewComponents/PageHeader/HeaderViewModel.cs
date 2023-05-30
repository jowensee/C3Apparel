
using C3Apparel.Frontend.Data.Membership;

namespace C3Apparel.Web.Component.ViewComponents.PageHeader
{
    public class HeaderViewModel
    {
        public AccountUser CurrentUser { get; }

        public HeaderViewModel(AccountUser currentUser)
        {
            CurrentUser = currentUser;
        }
    }
}