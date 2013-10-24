using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sipro.autenticacion
{
    public class SiproAuthenication : IAuthentication       
    {
        private String _path;

        public SiproAuthenication(String path)
        {
            _path = path;
        }

        public bool IsAuthenticated(string domain, string username, string pwd)
        {
            throw new NotImplementedException();
        }
    }
}
