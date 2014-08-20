using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace WSCall
{
    /// <summary>
    /// Descripción breve de WebService
    /// </summary>
    public class WebService
    {
        public string Url { get; set; }
        public string MethodName { get; set; }
        public string Xmlns { get; set; }
        public Dictionary<string, string> Params = new Dictionary<string, string>();
        public XDocument ResultXML;
        public string ResultString;

        public NetworkCredential Credentials { get; set; }
        public WebService()
        {

        }

        public WebService(string url, string methodName)
        {
            Url = url;
            MethodName = methodName;
            Xmlns = "http://tempuri.org/";
        }

        public WebService(string url, string methodName, string xmlns)
        {
            Url = url;
            MethodName = methodName;
            Xmlns = xmlns;
        }

        public void AddCredentials (string user, string pass)
        {
            Credentials = new System.Net.NetworkCredential(user, pass);
        }
        
        /// <summary>
        /// Invokes service
        /// </summary>
        public void Invoke()
        {
            Invoke(true);
        }

        /// <summary>
        /// Invokes service
        /// </summary>
        /// <param name="encode">Added parameters will encode? (default: true)</param>
        public void Invoke(bool encode)
        {
            string soapStr =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                <{0} xmlns=""{2}"">
                  {1}
                </{0}>
              </soap:Body>
            </soap:Envelope>";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.Headers.Add("SOAPAction", String.Format("\"{0}" + MethodName + "\"",Xmlns));
            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.Method = "POST";

            if (Credentials != null)
                req.Credentials = Credentials;

            using (Stream stm = req.GetRequestStream())
            {
                string postValues = "";
                foreach (var param in Params)
                {
                    if (encode)
                        postValues += string.Format("<{0}>{1}</{0}>", HttpUtility.UrlEncode(param.Key), HttpUtility.UrlEncode(param.Value));
                    else
                        postValues += string.Format("<{0}>{1}</{0}>", param.Key, param.Value);
                }

                soapStr = string.Format(soapStr, MethodName, postValues, Xmlns);
                using (StreamWriter stmw = new StreamWriter(stm))
                {
                    stmw.Write(soapStr);
                }
            }

            using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                string result = responseReader.ReadToEnd();
                ResultXML = XDocument.Parse(result);
                ResultString = result;
            }
        }
    }
}