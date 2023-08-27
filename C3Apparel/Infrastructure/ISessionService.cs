namespace C3Apparel.Infrastructure;

public interface ISessionService
{
    public string GetString(string sessionKey);
    public void SetValue(string sessionKey, string value);
}