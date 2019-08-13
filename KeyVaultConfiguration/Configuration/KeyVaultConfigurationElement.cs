using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConfiguration
{
    public sealed class KeyVaultConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsRequired = false)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
        }

        [ConfigurationProperty("vaultUrl", DefaultValue = "", IsRequired = false)]
        public string VaultUrl
        {
            get
            {
                return (string)this["vaultUrl"];
            }
        }

        [ConfigurationProperty("authenticationUrl", DefaultValue = "", IsRequired = false)]
        public string AuthenticationUrl
        {
            get
            {
                return (string)this["authenticationUrl"];
            }
        }

        [ConfigurationProperty("identity", DefaultValue = "", IsRequired = false)]
        public string Identity
        {
            get
            {
                return (string)this["identity"];
            }
        }

        [ConfigurationProperty("credential", DefaultValue = "", IsRequired = false)]
        public string Credential
        {
            get
            {
                return (string)this["credential"];
            }
        }

        [ConfigurationProperty("credentialType", DefaultValue = "", IsRequired = false)]
        public string CredentialType
        {
            get
            {
                return (string)this["credentialType"];
            }
        }

        [ConfigurationProperty("secretPrefix", DefaultValue = "", IsRequired = false)]
        public string SecretPrefix
        {
            get
            {
                return (string)this["secretPrefix"];
            }
        }
    }
}
