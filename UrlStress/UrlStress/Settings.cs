using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlStress
{

    public class Settings
    {
        public static int CountThreads = 0;
        public static int CountRequest = 0;
        public static bool isforever = false;
        public static bool autenticate = false;
        public static string authUser = "";
        public static string authPass = "";
        public static bool proxy = false;
        public static string proxyHost = "";
        public static string proxyUser = "";
        public static string proxyPass = "";
        public static int proxyPort = 0;
    }
}
