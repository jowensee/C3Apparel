
namespace C3Apparel.Frontend.Data.Membership
{
    public interface ICurrentUserProvider
    {
        CurrentUserInfo Get();
        AccountUser GetCurrentUserInfo();
    }
}