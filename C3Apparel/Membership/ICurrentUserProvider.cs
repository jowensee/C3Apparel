
using System.Threading.Tasks;
using C3Apparel.Areas.Identity.Data;

namespace C3Apparel.Web.Membership
{
    public interface ICurrentUserProvider
    {
        Task<C3ApparelUser> Get();
        Task<AccountUser> GetCurrentUserInfo();
    }
}