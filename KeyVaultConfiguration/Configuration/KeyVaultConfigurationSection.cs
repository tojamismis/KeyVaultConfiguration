using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConfiguration
{
    public class KeyVaultConfigurationSection : ConfigurationSection
    {
        private static KeyVaultConfigurationSection _manager;

        public static KeyVaultConfigurationSection Instance
        {
            get
            {
                if(_manager == null)
                {
                    try
                    {
                        _manager = ConfigurationManager.GetSection("keyVaultConfig") as KeyVaultConfigurationSection;
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
                return _manager;
            }
        }

        [ConfigurationProperty("vaults", IsRequired = true)]
        public KeyVaultCollection Vaults
        {
            get
            {
                return (KeyVaultCollection)base["vaults"];
            }
        }

    }
}
