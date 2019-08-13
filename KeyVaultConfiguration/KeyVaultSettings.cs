using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteStrike.KeyVaultConnector.Model;
using WhiteStrike.KeyVaultConnector.Authentication;
using WhiteStrike.KeyVaultConnector.Manager;

namespace WhiteStrike.KeyVaultConfiguration
{
    internal class KeyVaultSettings
    {
        private const string __certificate = "Certificate";
        private const string __managedIdentity = "ManagedIdentity";
        private const string __Secret = "Secret";
        private KeyVaultConfigurationElement _section;

        internal KeyVaultSettings(KeyVaultConfigurationElement section)
        {
            _section = section;
        }

        private SecretManager _kvSecretManager;

        internal SecretManager kvSecretManager
        {
            get
            {
                if (_kvSecretManager == null)
                {
                    _kvSecretManager = new KeyVaultConnector.Manager.SecretManager(kvConfiguration);
                }
                return _kvSecretManager;
            }
        }

        private KeyVaultConfigurationSettings _config;

        internal KeyVaultConfigurationSettings kvConfiguration
        {
            get
            {
                if (_config == null)
                {
                    _config = new KeyVaultConnector.Model.KeyVaultConfigurationSettings(_section.AuthenticationUrl, _section.VaultUrl, _section.Identity, _section.Credential, kvAuthType, _section.SecretPrefix);
                }
                return _config;
            }
        }

        private AuthenticationType kvAuthType
        {
            get
            {
                string credType = _section.CredentialType;
                AuthenticationType authType = AuthenticationType.Certificate;
                if (credType == __certificate)
                {
                    authType = KeyVaultConnector.Authentication.AuthenticationType.Certificate;
                }
                else if (credType == __managedIdentity)
                {
                    authType = KeyVaultConnector.Authentication.AuthenticationType.ManagedIdentity;
                }
                else if (credType == __Secret)
                {
                    authType = KeyVaultConnector.Authentication.AuthenticationType.Secret;
                }
                else
                {
                    throw new ApplicationException($"Invalid CredentialType Specified.  Must be one of the following:  {__certificate}; {__managedIdentity} or {__Secret}");
                }
                return authType;
            }
        }

        public async Task<string> GetSecret(string key)
        {
            return await kvSecretManager.GetSecret(key);
        }
    }
}
