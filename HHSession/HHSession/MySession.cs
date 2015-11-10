using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HHSession
{
    [ComVisible(true)]
    public class MySession : ISessionObject
    {
        IVariantDictionary _contents = new VariantDictionary();
        public int CodePage
        {
            get
            {
                return HttpContext.Current.Session.CodePage;
            }
            set
            {
                HttpContext.Current.Session.CodePage = value;
            }
        }

        public IVariantDictionary Contents
        {
            get { return _contents; }
        }

        public int LCID
        {
            get
            {
                return HttpContext.Current.Session.LCID;
            }
            set
            {
                HttpContext.Current.Session.LCID = value;
            }
        }

        public string SessionID
        {
            get { return HttpContext.Current.Session.SessionID; }
        }

        public IVariantDictionary StaticObjects
        {
            get { throw new NotImplementedException(); }
        }

        public int Timeout
        {
            get
            {
                return HttpContext.Current.Session.Timeout;
            }
            set
            {
                HttpContext.Current.Session.Timeout = value;
            }
        }

        public dynamic this[string bstrValue]
        {
            get
            {
                return _contents[bstrValue];
            }
            set
            {
                _contents[bstrValue] = value;
            }
        }

        public void Abandon()
        {
            HttpContext.Current.Session.Abandon();
        }

        public void let_Value(string bstrValue, object pvar)
        {
            _contents[bstrValue] = pvar;
        }
    }
}
