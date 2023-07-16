using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Data.Sql;
using C3Apparel.Data.Utilities;

namespace C3Apparel.Data.Modules.Classes
{
    public class UserProvider : BaseRepository, IUserProvider
    {

        public UserProvider(IConfigurationService configurationService) : base(configurationService)
        {
        }
        public IEnumerable<UserAccount> GetAllUsers(UserFilter filter, int pageNumber = 0, int itemsPerPage = 0)
        {
            
            var sFilterSql = new StringBuilder();
            if (filter != null )
            {

            }
            
            var sSql = $@"SELECT u.Id as UserID, u.UserName, u.Email, r.Id as RoleId, r.Name as RoleName FROM AspNetUsers u
                            JOIN AspNetUserRoles ur
                            ON u.Id = ur.UserId
                            JOIN AspNetRoles r
                            ON ur.RoleId = r.Id
                            Order By UserName";

            if (itemsPerPage > 0)
            {
                sSql += $" OFFSET {(pageNumber - 1) * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY";
            }
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return Enumerable.Empty<UserAccount>();
            }

            
            var results = new List<UserAccount>();

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var row = ds.Tables[0].Rows[i];
                
                results.Add(
                    new UserAccount
                    {
                        UserId  = row["UserId"].ToSafeString(),
                        UserName =  row["UserName"].ToSafeString(),
                        Email =  row["Email"].ToSafeString(),
                        RoleId =  row["RoleId"].ToSafeString(),
                        RoleName =  row["RoleName"].ToSafeString(),
                    });
            }

            return results;
        }

        public int GetAllUsersCount(UserFilter filter)
        {
            var sFilterSql = new StringBuilder();
            if (filter != null )
            {

            }
            
            var sSql = $@"SELECT count(*) FROM AspNetUsers u
                            JOIN AspNetUserRoles ur
                            ON u.Id = ur.UserId
                            JOIN AspNetRoles r
                            ON ur.RoleId = r.Id
                            WHERE 1 = 1 {sFilterSql}";

           
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return 0;
            }

            return ds.Tables[0].Rows[0][0].ToInt();

        }

        public void Delete(string userId)
        {
            var parameters = new Dictionary<string, object> { { "@Id", userId } };
            ExecuteCommand("DELETE FROM AspNetUserRoles WHERE UserId = @Id", parameters);
            ExecuteCommand("DELETE FROM AspNetUsers WHERE Id = @Id", parameters);
        }
    }
}