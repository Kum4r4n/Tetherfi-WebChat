using Identity.Application.Interfaces;

namespace Identity.Infrastructure.Configuration
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly TokenSetting _tokenSetting;

        public ConfigurationProvider(TokenSetting tokenSetting)
        {

            _tokenSetting = tokenSetting;
        }

        public string GetSecret()
        {
            return _tokenSetting.Secret;
        }

        public string GetIssuer()
        {
            return _tokenSetting.Issuer;
        }

        public string Audience()
        {
            return _tokenSetting.Audience;
        }

    }
}
