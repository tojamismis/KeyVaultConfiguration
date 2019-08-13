using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConfiguration
{
    public static class KeyVaultConfigurationManager
    {
        private const string __vaultPrefix = "${KV::";
        private const string __start = "${";
        private const string __end = "}";
        private static KeyVaultCache _cache;
        private static ConcurrentDictionary<string, ConnectionStringSettings> _connectionStrings = new ConcurrentDictionary<string, ConnectionStringSettings>();
        private static int _refreshFrequency = 600;
        /// <summary>
        /// Number of seconds until the cached secret should be refreshed
        /// </summary>
        public static int RefreshFrequency
        {
            get
            {
                return _refreshFrequency;
            }
            set
            {
                _refreshFrequency = value;
                Cache.RefreshFrequency = _refreshFrequency;
            }
        }
        /// <summary>
        /// Number of seconds between KeyVault refresh
        /// </summary>
        public static double RefreshTimerFrequency
        {
            get
            {
                return Cache.RefreshTimerFrequency;
            }
            set
            {
                Cache.RefreshTimerFrequency = value;
            }
        }

        /// <summary>
        /// Resolves a string to determine whether it contains KeyVault markup.  If so, it returns the KeyVault secret or the string
        /// </summary>
        /// <param name="key">The input string to check for KeyVault syntax</param>
        /// <returns>string</returns>
        public static async Task<string> ResolveSetting(string key)
        {
            string returnValue = key;
            if (key.Contains(__vaultPrefix))
            {
                returnValue = await ResolveString(key);
            }
            return returnValue;
        }

        /// <summary>
        /// Gets and application setting and resolves it from KeyVault if it contains KeyVault markup
        /// </summary>
        /// <param name="key">The application setting key</param>
        /// <returns>string</returns>
        public static async Task<string> AppSettings(string key)
        {
            string returnValue = ConfigurationManager.AppSettings[key];
            if (returnValue != null && returnValue.Contains(__vaultPrefix))
            {
                return await ResolveString(returnValue);
            }
            else
            {
                return returnValue;
            }
        }

        /// <summary>
        /// Gets a Connection string setting object.  Resolves the connection string from KeyVault if it contains KeyVault markup
        /// </summary>
        /// <param name="key">The Connection String Name</param>
        /// <returns>ConnectionStringSettings</returns>
        public static async Task<ConnectionStringSettings> ConnectionString(string key)
        {
            //if we have not already added the connection string to the cache here, we create a new clone of the settings
            //the connectionStrings in a config file are read only, so we cannot update a connection string with a value
            //from KeyVault unless we clone it
            if (!_connectionStrings.ContainsKey(key))
            {
                ConnectionStringSettings oldConn = ConfigurationManager.ConnectionStrings[key];
                if (oldConn != null)
                {
                    ConnectionStringSettings newConn = new ConnectionStringSettings(oldConn.Name, oldConn.ConnectionString, oldConn.ProviderName);
                    _connectionStrings.AddOrUpdate(key, newConn, (thisKey, thisValue) => thisValue);
                }
            }

            //Get the connection string, replace it with the cached value from KeyVault and output the result
            ConnectionStringSettings conn;
            if (_connectionStrings.ContainsKey(key))
            {
                conn = _connectionStrings[key];
                if (conn.ConnectionString.Contains(__vaultPrefix))
                {
                    conn.ConnectionString = await ResolveString(conn.ConnectionString);
                }
                return conn;
            }
            return null;
        }

        private static async Task<string> ResolveString(string item)
        {
            while (item.Contains(__vaultPrefix))
            {
                string thematch = Regex.Match(item, @"\${[^${}]+}").Value;
                item = item.Replace(thematch, await Cache.GetSettingFromKeyVault(thematch));
            }
            return item;
        }

        private static KeyVaultCache Cache
        {
            get
            {
                if(_cache == null)
                {
                    _cache = new KeyVaultCache(RefreshTimerFrequency, RefreshFrequency);
                }
                return _cache;
            }
        }
    }
}
