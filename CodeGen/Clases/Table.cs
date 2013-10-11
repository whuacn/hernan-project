using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace CodeGen.Clases
{
    [Serializable]
    public class Table
    {
        public string TableName { get; set; }
        public string ClassName { get; set; }
        public string GestorName { get; set; }
        public List<Column> Columns { get; set; }
        public string NameSpaceName { get; set; }
        public bool Selected { get; set; }

        public Table(string name)
        {
            this.TableName = name;
            this.ClassName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(name.ToLower());
            this.GestorName = "Gestor" + this.ClassName;
            this.Selected = false;
        }

    }
}
