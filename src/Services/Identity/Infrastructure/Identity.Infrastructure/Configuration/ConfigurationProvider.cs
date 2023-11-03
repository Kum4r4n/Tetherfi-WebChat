using Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
