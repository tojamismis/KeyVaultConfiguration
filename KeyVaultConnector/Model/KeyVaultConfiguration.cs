using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConnector.Model
{
    public class KeyVaultConfigurationSettings
    {
        public string AuthURL { get; }
        public string VaultURL { get; }
        public string ConnectingAccountIdentity { get; }
        public string ConnectingAccountCredential { get; }
        public Authentication.AuthenticationType CredentialType { get; }
        public string SecretPrefix { get; }

        public KeyVaultConfigurationSettings(string authUrl, string vaultUrl, string identity, string credential, Authentication.AuthenticationType credentialType) 
        {
            AuthURL = authUrl;
            VaultURL = vaultUrl;
            ConnectingAccountIdentity = identity;
            ConnectingAccountCredential = credential;
            CredentialType = credentialType;
            SecretPrefix = string.Empty;
        }

        public KeyVaultConfigurationSettings(string authUrl, string vaultUrl, string identity, string credential, Authentication.AuthenticationType credentialType, string secretPrefix)
        {
            AuthURL = authUrl;
            VaultURL = vaultUrl;
            ConnectingAccountIdentity = identity;
            ConnectingAccountCredential = credential;
            CredentialType = credentialType;
            SecretPrefix = secretPrefix;
        }
    }
}
