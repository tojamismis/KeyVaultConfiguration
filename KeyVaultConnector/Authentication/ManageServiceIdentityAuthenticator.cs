using WhiteStrike.KeyVaultConnector.Model;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConnector.Authentication
{
    class ManageServiceIdentityAuthenticator
    {
        KeyVaultConfigurationSettings _config = null;

        internal ManageServiceIdentityAuthenticator(KeyVaultConfigurationSettings configuration)
        {
            _config = configuration;
        }

        public async Task<KeyVaultClient> Authenticate()
        {
            AzureServiceTokenProvider provider = new AzureServiceTokenProvider();
            return new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(provider.KeyVaultTokenCallback));
        }
    }
}
