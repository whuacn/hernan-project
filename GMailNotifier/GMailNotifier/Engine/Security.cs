using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace GMailNotifier.Engine
{
    internal class Security
    {
        static readonly Encoding _encoding = Encoding.Unicode;

        internal static string Unprotect(string encryptedString)
        {
            if (encryptedString == null)
                return null;
            byte[] protectedData = Convert.FromBase64String(encryptedString);
            byte[] unprotectedData = ProtectedData.Unprotect(protectedData,
                null, DataProtectionScope.CurrentUser);

            return _encoding.GetString(unprotectedData);
        }

        internal static string Protect(string unprotectedString)
        {
            if (unprotectedString == null)
                return null;

            byte[] unprotectedData = _encoding.GetBytes(unprotectedString);
            byte[] protectedData = ProtectedData.Protect(unprotectedData,
                null, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(protectedData);
        }
    }
}
