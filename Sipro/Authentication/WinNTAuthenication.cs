using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace sipro.autenticacion
{
    public class WinNTAuthenication : IAuthentication
    {

        private String _path;

        public WinNTAuthenication(String path)
        {
            _path = path;
        }
        public bool IsAuthenticated(string domain, string username, string pwd)
        {
            bool authenticated = false;
            try
            {
                using (PrincipalContext principalContext = new PrincipalContext(ContextType.Machine))
                {
                    authenticated = principalContext.ValidateCredentials(username, pwd);
                }
            }
            catch (PrincipalServerDownException ex)
            {
            }
            return authenticated;
        }
    }
}
