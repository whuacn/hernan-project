using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHSession
{
    public interface IVariantDictionary : IEnumerable
    {
        int Count { get; }
        dynamic this[object VarKey] { get; set; }
        dynamic get_Key(object VarKey);
        IEnumerator GetEnumerator();
        void let_Item(object VarKey, object pvar);
        void Remove(object VarKey);
        void RemoveAll();
    }
}
