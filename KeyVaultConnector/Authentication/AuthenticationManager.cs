using WhiteStrike.KeyVaultConnector.Model;
using Microsoft.Azure.KeyVault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConnector.Authentication
{
    internal static class AuthenticationManager
    {
        public static Task<KeyVaultClient> Authenticate(KeyVaultConfigurationSettings configuration)
        {
            if (configuration.CredentialType == AuthenticationType.Certificate)
            {
                return (new CertificateAuthenticationManager(configuration)).Authenticate();
            }
            else if (configuration.CredentialType == AuthenticationType.Secret)
            {
                return (new ClientSecretAuthenticator(configuration)).Authenticate();
            }
            else
            {
                return (new ManageServiceIdentityAuthenticator(configuration)).Authenticate();
            }
        }
    }
}
