using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entidades;

namespace Servicios
{
    public class GestorMachine
    {
        public static List<Machine> Get()
        {
            return Servicios.Repository<List<Machine>>.Deserialize("Machines.data");
        }

        public static void Save(List<Machine> machinelist)
        {
            Servicios.Repository<List<Machine>>.Serialize(machinelist, "Machines.data");
        }

    }
}
