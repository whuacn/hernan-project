using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HHSession
{
    public class VariantDictionary: IVariantDictionary
    {
        System.Web.SessionState.HttpSessionState m_Items = HttpContext.Current.Session;
        public int Count
        {
            get { return m_Items.Count; }
        }

        public dynamic this[object VarKey]
        {
            get
            {
                return m_Items[(string)VarKey];
            }
            set
            {
                m_Items[(string)VarKey] = value;
            }
        }

        public dynamic get_Key(object VarKey)
        {
            return m_Items[(string)VarKey];
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
           return m_Items.GetEnumerator();
        }

        public void let_Item(object VarKey, object pvar)
        {
            m_Items[(string)VarKey] = pvar;
        }

        public void Remove(object VarKey)
        {
            m_Items.Remove((string)VarKey);
        }

        public void RemoveAll()
        {
            m_Items.RemoveAll();
        }
    }
}
