using C3Apparel.Data.Pricing;

namespace C3Apparel.Frontend.Data.Membership;

public class AccountUser
{
    public string Username { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string CountryRole { get; }
    public string FullName { get; }
    public bool IsGlobalAdministrator { get; }
    public bool IsPublicUser => Username == "public";

    public string Currency
    {
        get
        {
            switch (CountryRole)
            {
                case AccountConstants.ROLE_AU:
                    return CurrencyConstants.AUD;
                case AccountConstants.ROLE_NZ:
                    return CurrencyConstants.NZD;
            }

            return string.Empty;
        }
    }

    public AccountUser(string username, string firstName, string lastName, string email, string countryRole, string fullName, bool isGlobalAdministrator)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        CountryRole = countryRole;
        FullName = fullName;
        IsGlobalAdministrator = isGlobalAdministrator;
    }
}