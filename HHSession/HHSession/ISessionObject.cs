using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSession
{
    public interface ISessionObject
    {

        int CodePage { get; set; }
        IVariantDictionary Contents { get; }
        int LCID { get; set; }
        string SessionID { get; }
        IVariantDictionary StaticObjects { get; }
        int Timeout { get; set; }
        dynamic this[string bstrValue] { get; set; }
        void Abandon();
        void let_Value(string bstrValue, object pvar);
    }
}
