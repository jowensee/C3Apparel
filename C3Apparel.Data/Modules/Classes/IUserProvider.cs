using System.Collections.Generic;
using C3Apparel.Data.Modules.Filters;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IUserProvider
    {
        IEnumerable<UserAccount> GetAllUsers(UserFilter filter, int pageNumber = 0, int itemsPerPage = 0);
        int GetAllUsersCount(UserFilter filter);
        void Delete(string userId);
    }
}