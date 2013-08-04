using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Xml.Xsl;
using System.Xml.Linq;
using System.IO;

namespace GMailNotifier.Engine
{
    internal class svrNotifier
    {
        public static event EventHandler OnUpdate;

        public static void CheckUpdate()
        {
            Inbox ibx = GetInbox();

            if (ibx == null)
                return;

            if (Program.inbox == null)
            {
                Program.inbox = ibx;
                if (OnUpdate != null)
                    OnUpdate(null, null);
            }
            else if (ibx.modified != Program.inbox.modified)
            {
                foreach (Entry entry in ibx.entries)
                {
                    if (Program.inbox.entries.Contains(entry))
                    {
                        entry.Notify = false;
                    }
                }
                Program.inbox = ibx;
                if (OnUpdate != null)
                    OnUpdate(null, null);
            }
            else
            {
                Program.inbox = ibx;
            }                        
        }

        static Inbox GetInbox()
        {
            string response = LoadData(Security.Unprotect(Storage.getSetting("UID")), Security.Unprotect(Storage.getSetting("CLV")));
            byte[] byteArray = Encoding.ASCII.GetBytes(response);
            MemoryStream stream = new MemoryStream(byteArray);
            XDocument document = XDocument.Load(stream);
            XElement element = document.Root;

            return InboxConverter(element);

        }
        internal static string LoadData(string user, string pass)
        {
            WebClient objClient = new WebClient();
            objClient.Credentials = new NetworkCredential(user, pass);
            string response = Encoding.UTF8.GetString(objClient.DownloadData("https://mail.google.com/mail/feed/atom"));
            response = response.Replace("<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">", "<feed>");
            return response;
        }
        static Inbox InboxConverter(XElement element)
        {
            Inbox ibx = new Inbox();
            ibx.fullcount = Convert.ToInt32(element.Element("fullcount").Value);
            ibx.link = element.Element("link").Attribute("href").Value;
            ibx.title = element.Element("title").Value;
            ibx.tagline = element.Element("tagline").Value;
            ibx.modified = Convert.ToDateTime(element.Element("modified").Value);
            ibx.entries = EntryConverter(element.Elements("entry").ToList());
            return ibx;
        }
        static List<Entry> EntryConverter(List<XElement> elements)
        {
            List<Entry> entries = new List<Entry>();
            foreach (XElement element in elements)
            {
                Entry entry = new Entry();
                entry.id = element.Element("id").Value;
                entry.title = element.Element("title").Value;
                entry.summary = element.Element("summary").Value;
                entry.link = element.Element("link").Attribute("href").Value;
                entry.modified = Convert.ToDateTime(element.Element("modified").Value);
                entry.issued = Convert.ToDateTime(element.Element("issued").Value);
                entry.author = AuthorConverter(element.Element("author"));
                entry.contributor = AuthorConverter(element.Element("contributor"));
                entry.Notify = true;
                entries.Add(entry);
            }
            return entries;
        }
        static author AuthorConverter(XElement element)
        {
            if (element == null) return null;
            author author = new author();
            author.name = element.Element("name").Value;
            author.email = element.Element("email").Value;
            return author;
        }

    }
}
