using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sipro.autenticacion
{
    public interface IAuthentication
    {
        bool IsAuthenticated(String domain, String username, String pwd);

    }
}
