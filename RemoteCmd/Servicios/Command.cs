using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Servicios
{
    public static class Command
    {
        public static String[] BlockPort
        {
            get {

                String[] commands = { 
                                    "netsh ipsec static add policy name=ProxyDesa assign=yes",
                                    "netsh ipsec static add filterlist name=IPProxy",
                                    "netsh ipsec static add filter filterlist=IPProxy srcaddr=Me dstaddr=any protocol=TCP srcport=0 dstport=1433",
                                    "netsh ipsec static add filteraction name=blockIPProxy action=block",
                                    "netsh ipsec static add rule name=blockIPProxyrule policy=ProxyDesa filterlist=IPProxy filteraction=blockIPProxy"
                                };
                return commands;
            }
        }

        public static String[] UnBlockPort
        {
            get
            {

                String[] commands = { 
                                        "netsh ipsec static set policy name=ProxyDesa assign=no",
                                        "netsh ipsec static delete rule name=blockIPProxyrule policy=ProxyDesa",
                                        "netsh ipsec static delete filteraction name=blockIPProxy",
                                        "netsh ipsec static delete filterlist name=IPProxy",                                        
                                        "netsh ipsec static delete policy name=ProxyDesa"
                                    };
                return commands;
            }
        }
    }
}
