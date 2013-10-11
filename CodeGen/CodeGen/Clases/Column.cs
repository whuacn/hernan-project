using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGen.Clases
{
    [Serializable]
    public class Column
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string Property { get; set; }
        public bool Selected { get; set; }
        
    }
}
