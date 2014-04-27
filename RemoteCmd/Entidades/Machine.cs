using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    [Serializable()]
    public class Machine
    {
        public String IP { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
    }

}
