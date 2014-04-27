using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class Contexto
    {
        static List<Machine> machines = new List<Machine>();
        public static List<Machine> Machines
        {
            get { return machines; }
            set { machines = value; }
        }
    }
}
