using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteStrike.KeyVaultConnector.Authentication
{
    public enum AuthenticationType
    {
        Certificate = 0,
        Secret = 1,
        ManagedIdentity = 2
    }
}
