using WhiteStrike.KeyVaultConnector.Model;
using Microsoft.Azure.KeyVault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConnector.Manager
{
    public class CertificateManager
    {
        private KeyVaultConfigurationSettings _config;
        private KeyVaultClient _client;
        public CertificateManager(KeyVaultConfigurationSettings configuration)
        {
            _config = configuration;
            Task.Run(CreateClient);
        }

        private async Task CreateClient()
        {
            _client = await Authentication.AuthenticationManager.Authenticate(_config);
        }

        private KeyVaultClient client
        {
            get
            {
                if (_client == null)
                {
                    _client = Authentication.AuthenticationManager.Authenticate(_config).Result;
                }
                return _client;
            }
        }
    }
}
