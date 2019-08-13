using WhiteStrike.KeyVaultConnector.Model;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConnector.Authentication
{
    internal class ClientSecretAuthenticator
    {
        KeyVaultConfigurationSettings _config = null;

        internal ClientSecretAuthenticator(KeyVaultConfigurationSettings configuration)
        {
            _config = configuration;
        }

        public async Task<KeyVaultClient> Authenticate()
        {
            return new KeyVaultClient(AuthenticationCallback);
        }

        public async Task<string> AuthenticationCallback(string loginUrl, string vaultUrl, string scope)
        {
            var authContext = new AuthenticationContext(loginUrl);
            ClientCredential credential = new ClientCredential(_config.ConnectingAccountIdentity, _config.ConnectingAccountCredential);
            AuthenticationResult result = await authContext.AcquireTokenAsync(vaultUrl, credential);
            if (result == null)
            {
                throw new ApplicationException("Login to KeyVault Denied");
            }
            return result.AccessToken;
        }
    }
}
