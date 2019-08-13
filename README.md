# KeyVaultConfiguration
A Key Vault configuration provider for .NET Framework projects.

Code is provided as is with no warranty or license.

The project is designed to provide KeyVault support for IaaS server projects in .NET 4 framework projects that are pre 4.7.1 and cannot use Microsoft's provider, or in the case that development teams want to pick and choose which values they want from KeyVault.

This project allows a team to use keyvault based on simple changes to code that will use a "KeyVaultConfigurationProvider" to get AppSettings and ConnectionStrings.

The provider will then evaluate the value for a special "KeyVault" syntax to determine whether to get the value from KeyVault or from the configuration file.  This allows development teams to work and run the project without having to connect to KeyVault, but then KeyVault can be used in test, integration and production environments with a simple configuration file change.

The provider runs as a static cache that can refresh on a timer interval.  Should the provider be unable to connect to KeyVault during a refresh, the application will continue to run using the existing cached values.


Sample configuration file (from the unit test)

  <keyVaultConfig>
    <vaults>
      <vaultConfig name="TestVault" vaultUrl="https://[yourvault].vault.azure.net" authenticationUrl="https://login.microsoftonline.com/[yourtenantidguid]/login" identity="[identityguidforappuser]" credential="[certthumbprint]" credentialType="Certificate" secretPrefix="Test" />
      <vaultConfig name="TestVault2" vaultUrl="https://[yoursecondvault].vault.azure.net" authenticationUrl="https://login.microsoftonline.com/[yourtenantidguid]/login" identity="[identityguidforappuser]" credential="[clientsecret]" credentialType="Secret" secretPrefix="Test" />
      <vaultConfig name="StaticValueTestVault" vaultUrl="https://whitestrike.vault.azure.com" authenticationUrl="https://login.microsoftonline.com/orbital/login" identity="f8651091-96c8-4819-af0d-5ac55f1b5666" credential="AD0F54B8CE90F8D1FD3146D299459791AFC99B91" credentialType="Certificate" secretPrefix="Test" />
    </vaults>
  </keyVaultConfig>
  
    <appSettings>
    <!-- Pulls from vault named "TestVault" in the configuration above and looks for a value named "TestHello" -->
    <add key="Test" value="${KV::TestVault::Hello}" />
    <!-- Pulls from vault named "TestVault2" in the configuration above and looks for a value named "TestThisString" -->
    <add key="TestString2" value="${KV::TestVault2::ThisString}" />
    <!-- Pulls from vault named "TestVault" in the configuration above and looks for a value named "TestSecondString" -->
    <add key="SecondString" value="${KV::TestVault::SecondString}" />
  </appSettings>
  <connectionStrings>
    <add name="testConnection" providerName="Microsoft.Sql.Server" connectionString="${KV::TestVault::ConnectionString}" />
  </connectionStrings>
  
  And then the code is altered from "ConfigurationManager.AppSettings[value]" to "KeyVaultConfigurationManager.AppSettings(value)"
  
  Like so.  
  
  string hello = KeyVaultConfigurationManager.AppSettings("Test").Result;
  string test2 = KeyVaultConfigurationManager.AppSettings("TestString2").Result;
  string secondString = KeyVaultConfigurationManager.AppSettings("SecondString").Result;
  ConnectionStringSettings settings = KeyVaultConfigurationManager.ConnectionString("testConnection").Result;
