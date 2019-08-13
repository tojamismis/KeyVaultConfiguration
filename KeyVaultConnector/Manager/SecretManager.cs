using WhiteStrike.KeyVaultConnector.Model;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Rest.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConnector.Manager
{
    public class SecretManager
    {
        private KeyVaultConfigurationSettings _config;
        private KeyVaultClient _client;
        public SecretManager(KeyVaultConfigurationSettings configuration)
        {
            _config = configuration;
        }

        private KeyVaultClient client
        {
            get
            {
                if(_client == null)
                {
                    _client = Authentication.AuthenticationManager.Authenticate(_config).Result;
                }
                return _client;
            }
        }

        public async Task<string> GetSecret(string identifier)
        {
            try
            {
                //For reasons beyond my comprehension, the call does not work when it is async.  Crappy work Microsoft... 
                SecretBundle secret = client.GetSecretAsync(_config.VaultURL, _config.SecretPrefix + identifier).Result;
                if (secret == null)
                {
                    return "secret is empty";
                }
                else
                {
                    return secret.Value;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Dictionary<string, string>> GetSecrets()
        {
            IPage<SecretItem> secrets = await _client.GetSecretsAsync(_config.VaultURL);
            Dictionary<string, string> returnDictionary = new Dictionary<string, string>();
            foreach(var item in secrets)
            {
                returnDictionary.Add(item.Id, await GetSecret(item.Id));
            }
            return returnDictionary;
        }
    }
}
