using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Pricing;

namespace C3Apparel.Web.Membership;

public class AccountUser
{
    public string Username { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public IList<string> Roles { get; }
    public string FullName { get; }
    public bool IsAdministrator => Roles.Any(a => a == AccountConstants.ROLE_ADMIN);
    
    public bool IsCustomer => Roles.Any(a => a == AccountConstants.ROLE_CUSTOMER);
    public bool IsPublicUser => Username == "public";


    public AccountUser(string username, string firstName, string lastName, string email, IList<string> roles, string fullName)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        FullName = fullName;
        Roles = roles;
    }

    public AccountUser(string username)
    {
        Username = username;
    }
}