using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStrike.KeyVaultConfiguration;

namespace WhiteStrike.KeyVaultTests
{
    [TestClass]
    public class KeyVaultConfigTests
    {
        //Please note!  You must set up your own key vault and alter the TestVault and TestVault2 tests for these to pass testing
        [TestMethod]
        public void TestKeyVaultSettings()
        {
            string hello = KeyVaultConfigurationManager.AppSettings("Test").Result;
            string test2 = KeyVaultConfigurationManager.AppSettings("TestString2").Result;
            string secondString = KeyVaultConfigurationManager.AppSettings("SecondString").Result;
            ConnectionStringSettings settings = KeyVaultConfigurationManager.ConnectionString("testConnection").Result;

            Assert.AreEqual(hello, "Hello World", "Hello string does not match expected value from KeyVault");
            Assert.AreEqual(test2, "Another String Here", "Test 2 string does not match expected value from KeyVault");
            Assert.AreEqual(secondString, "This is the value of my second string", "second string does not match expected value from KeyVault");
            Assert.AreEqual(settings.ConnectionString, "database=thisone;server=myserver;user id=testuser;pwd=this set password;", "connection string does not match expected setting from keyvault");
        }

        [TestMethod]
        public void TestKeyVaultSettingsStepThroughForCaching()
        {
            string hello = KeyVaultConfigurationManager.AppSettings("Test").Result;
            string test2 = KeyVaultConfigurationManager.AppSettings("TestString2").Result;
            string secondString = KeyVaultConfigurationManager.AppSettings("SecondString").Result;
            ConnectionStringSettings settings = KeyVaultConfigurationManager.ConnectionString("testConnection").Result;

            string test2again = KeyVaultConfigurationManager.AppSettings("TestString2").Result;
            string secondStringagain = KeyVaultConfigurationManager.AppSettings("SecondString").Result;
            ConnectionStringSettings settingsagain = KeyVaultConfigurationManager.ConnectionString("testConnection").Result;

            Assert.AreEqual(hello, "Hello World", "Hello string does not match expected value from KeyVault");
            Assert.AreEqual(test2, "Another String Here", "Test 2 string does not match expected value from KeyVault");
            Assert.AreEqual(secondString, "This is the value of my second string", "second string does not match expected value from KeyVault");
            Assert.AreEqual(settings.ConnectionString, "database=thisone;server=myserver;user id=testuser;pwd=this set password;", "connection string does not match expected setting from keyvault");
        }

        [TestMethod]
        public void TestKeyVaultSettingsStepThroughForRefresh()
        {
            KeyVaultConfigurationManager.RefreshFrequency = 30;
            KeyVaultConfigurationManager.RefreshTimerFrequency = 45;

            string hello = KeyVaultConfigurationManager.AppSettings("Test").Result;
            string test2 = KeyVaultConfigurationManager.AppSettings("TestString2").Result;
            string secondString = KeyVaultConfigurationManager.AppSettings("SecondString").Result;
            ConnectionStringSettings settings = KeyVaultConfigurationManager.ConnectionString("testConnection").Result;

            System.Threading.Thread.Sleep(90000);

            string test2again = KeyVaultConfigurationManager.AppSettings("TestString2").Result;
            string secondStringagain = KeyVaultConfigurationManager.AppSettings("SecondString").Result;
            ConnectionStringSettings settingsagain = KeyVaultConfigurationManager.ConnectionString("testConnection").Result;

            Assert.AreEqual(hello, "Hello World", "Hello string does not match expected value from KeyVault");
            Assert.AreEqual(test2, "Another String Here", "Test 2 string does not match expected value from KeyVault");
            Assert.AreEqual(secondString, "This is the value of my second string", "second string does not match expected value from KeyVault");
            Assert.AreEqual(settings.ConnectionString, "database=thisone;server=myserver;user id=testuser;pwd=this set password;", "connection string does not match expected setting from keyvault");
        }
    }
}
