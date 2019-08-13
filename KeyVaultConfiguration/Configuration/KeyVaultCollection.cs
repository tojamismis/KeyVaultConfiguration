using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConfiguration
{
    /// <summary>
    /// A collection of configured KeyVaults
    /// </summary>
    [ConfigurationCollection(typeof(KeyVaultConfigurationElement), AddItemName = "vaultConfig")]
    public sealed class KeyVaultCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new KeyVaultConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as KeyVaultConfigurationElement).Name;
        }

        public new KeyVaultConfigurationElement this[string key]
        {
            get
            {
                return (BaseGet(key) as KeyVaultConfigurationElement);
            }
        }
    }
}
