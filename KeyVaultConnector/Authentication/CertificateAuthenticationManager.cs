using WhiteStrike.KeyVaultConnector.Model;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConnector.Authentication
{
    class CertificateAuthenticationManager
    {
        KeyVaultConfigurationSettings _config = null;

        internal CertificateAuthenticationManager(KeyVaultConfigurationSettings configuration)
        {
            _config = configuration;
        }

        public async Task<KeyVaultClient> Authenticate()
        {
            return new KeyVaultClient(AuthenticationCallback);
        }

        //Yes, this is duplicate and superflous, but it is a required signature from the azure callback
        public async Task<string> AuthenticationCallback(string loginUrl, string vaultUrl, string scope)
        {
            var authContext = new AuthenticationContext(loginUrl);
            ClientAssertionCertificate assertionCertificate = new ClientAssertionCertificate(_config.ConnectingAccountIdentity, await GetCertificate());
            AuthenticationResult result = await authContext.AcquireTokenAsync(vaultUrl, assertionCertificate);
            if(result == null)
            {
                throw new ApplicationException("Login to KeyVault Denied");
            }
            return result.AccessToken;
        }

        private async Task<X509Certificate2> GetCertificate()
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindByThumbprint, _config.ConnectingAccountCredential, false);
                return collection[0];
            }            
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                store.Close();
            }
        }
    }
}
