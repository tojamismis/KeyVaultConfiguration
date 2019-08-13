using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConfiguration
{
    internal class KeyVaultCache
    {
        private const string __delimiter = "::";
        private ConcurrentDictionary<string, string> _kvItems = new ConcurrentDictionary<string, string>();
        private ConcurrentDictionary<string, DateTime> _expirations = new ConcurrentDictionary<string, DateTime>();
        private System.Timers.Timer _timer = new System.Timers.Timer(300);
        /// <summary>
        /// Number of seconds until the cached secret should be refreshed
        /// </summary>
        internal int RefreshFrequency = 600;
        /// <summary>
        /// Number of seconds between KeyVault refresh
        /// </summary>
        internal double RefreshTimerFrequency
        {
            get
            {
                return _timer.Interval / 1000;
            }
            set
            {
                _timer.Interval = value * 1000;
            }
        }

        internal KeyVaultCache(double RefreshTimerFrequency, int RefreshFrequency)
        {
            this.RefreshFrequency = RefreshFrequency;
            this.RefreshTimerFrequency = RefreshTimerFrequency;
            this.Initialize();
        }

        internal async Task<string> GetSettingFromKeyVault(string key)
        {
            if (!_kvItems.ContainsKey(key))
            {
                await RefreshSetting(key);
            }
            return _kvItems[key];
        }

        internal bool ContainsKey(string key)
        {
            return _kvItems.ContainsKey(key);
        }

        private void RefreshItems()
        {
            foreach (string key in _kvItems.Keys)
            {
                if (!_expirations.ContainsKey(key) || _expirations[key] < DateTime.Now)
                {
                    Task.Run(() => TryRefreshSetting(key));
                }
            }
        }

        private void TryRefreshSetting(string key)
        {
            try
            {
                RefreshSetting(key).Start();
            }
            catch
            { }
        }

        private async Task RefreshSetting(string key)
        {
            //eliminate the before and after characters the ${ and the }
            string newKey = key.Substring(2, key.Length - 3);
            //split the string by the delimeter
            string[] kvParameters = Regex.Split(newKey, __delimiter, RegexOptions.None);
            //get the vault name from the element
            string kvName = kvParameters[1];
            //get the configuration section for this vault
            KeyVaultConfigurationElement section = KeyVaultConfigurationSection.Instance.Vaults[kvName];
            //create and instance of the settings class that translates between the config and the kv connector dll
            KeyVaultSettings settings = new KeyVaultSettings(section);
            //Use the settings to update this item in the cache
            _kvItems.AddOrUpdate(key, await settings.GetSecret(kvParameters[2]), (thiskey, thisvalue) => thisvalue);
            //And update the expiration time for this item
            _expirations.AddOrUpdate(key, DateTime.Now.AddSeconds(RefreshFrequency), (thiskey, thisvalue) => thisvalue);
        }


        private void Initialize()
        {
            _timer.Elapsed += CheckRefresh;
            _timer.Start();
        }

        //Async refresh of KeyVault items in the background.  Uses a concurrent dictionary so values will continue to be retrievable while we refresh
        private void CheckRefresh(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() => RefreshItems());
        }
    }
}
