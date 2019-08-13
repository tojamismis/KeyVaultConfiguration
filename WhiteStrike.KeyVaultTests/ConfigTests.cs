using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStrike.KeyVaultConfiguration;

namespace WhiteStrike.KeyVaultTests
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void AssertConfig_NotNull()
        {
            KeyVaultConfigurationSection section = KeyVaultConfigurationSection.Instance;
            Assert.IsNotNull(section, "Configuration Section cannot be null");
        }

        [TestMethod]
        public void AssertVault_Count()
        {
            KeyVaultCollection collection = KeyVaultConfigurationSection.Instance.Vaults;
            Assert.AreEqual(3, collection.Count, "KeyVault configuration Count should return 3 results");
        }

        [TestMethod]
        public void AssertVault_GetByName()
        {
            KeyVaultConfigurationElement element = KeyVaultConfigurationSection.Instance.Vaults["TestVault"];
            Assert.IsNotNull(element, "KeyVault Configuration Element TestVault should not be null");

        }

        [TestMethod]
        public void AssertVault_ElementConfiguration()
        {
            KeyVaultConfigurationElement element = KeyVaultConfigurationSection.Instance.Vaults["StaticValueTestVault"];
            Assert.AreEqual("Test", element.SecretPrefix, "Secret Prefix should be 'Test'");
            Assert.AreEqual("Certificate", element.CredentialType, "Configured Credential Type should be Certificate");
            Assert.AreEqual("https://whitestrike.vault.azure.com", element.VaultUrl, "Vault URL is not correct");
            Assert.AreEqual("https://login.microsoftonline.com/orbital/login", element.AuthenticationUrl, "Authentication URL is not correct");
            Assert.AreEqual("f8651091-96c8-4819-af0d-5ac55f1b5666", element.Identity, "Configured Vault Identity is not correct");
            Assert.AreEqual("AD0F54B8CE90F8D1FD3146D299459791AFC99B91", element.Credential, "Configured Vault Credential is not correct");
        }

        [TestMethod]
        public void AssertVault_SettingsClassFromConfiguration()
        {
            KeyVaultConfigurationElement element = KeyVaultConfigurationSection.Instance.Vaults["TestVault"];
            KeyVaultSettings settings = new KeyVaultSettings(element);
            WhiteStrike.KeyVaultConnector.Model.KeyVaultConfigurationSettings config = settings.kvConfiguration;
            Assert.AreEqual(element.Credential, config.ConnectingAccountCredential, "Credential is not consistent after translation to Connector settings");
            Assert.AreEqual(element.AuthenticationUrl, config.AuthURL, "AuthenticationUrl is not consistent after translation to Connector settings");
            Assert.AreEqual(element.Identity, config.ConnectingAccountIdentity, "Identity is not consistent after translation to Connector settings");
            Assert.AreEqual(element.VaultUrl, config.VaultURL, "VaultUrl is not consistent after translation to Connector settings");
            Assert.AreEqual(element.SecretPrefix, config.SecretPrefix, "SecretPrefix is not consistent after translation to Connector settings");
            Assert.AreEqual(KeyVaultConnector.Authentication.AuthenticationType.Certificate, config.CredentialType, "CredentialType is not consistent after translation to Connector settings");

        }
    }
}
