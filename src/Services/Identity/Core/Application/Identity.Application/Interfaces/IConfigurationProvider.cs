namespace Identity.Application.Interfaces
{
    public interface IConfigurationProvider
    {
        string GetSecret();
        string GetIssuer();
        string Audience();
    }
}
