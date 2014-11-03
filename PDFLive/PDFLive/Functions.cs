using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace PDFLive
{
    class Functions
    {
        internal static bool isUTF8(byte[] cont)
        {
            if (Encoding.UTF8.GetByteCount(Encoding.UTF8.GetString(cont)) == cont.Length)
                return true;

            return false;
        }

        internal static byte[] SourceDataBin(string URL)
        {
            if (URL == null)
                return null;

            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            return MyWebClient.DownloadData(URL);
        }

        internal static byte[] SourceDataBin(string URL, string user, string psw)
        {
            if (URL == null)
                return null;

            NetworkCredential myCredentials = new NetworkCredential(user, psw);
            WebClient MyWebClient = new WebClient();
            MyWebClient.Credentials = myCredentials;
            return MyWebClient.DownloadData(URL);
        }
        internal static float MilimetroToPoint(Single num)
        {
            //CONVIERTE MILIMETROS A POINTS
            return (float)(((num / 10) / 2.54) * 72);
        }

        internal static void Errorhandle(String mensaje, Exception e)
        {
            Console.WriteLine("Message:    {0}\n", e.Message);
            Console.WriteLine("Source:     {0}\n", e.Source);
            Console.WriteLine("TargetSite: {0}\n", e.TargetSite.Name);
            Console.WriteLine("StackTrace: {0}\n", e.StackTrace);

            throw new Exception(mensaje, e);

        }
    }
}
