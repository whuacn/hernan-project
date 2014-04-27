using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entidades;

namespace Servicios
{
    public class GestorMachine
    {
        public static List<Machine> GetAll()
        {
            try
            {
                return Servicios.Repository<List<Machine>>.Deserialize("Machines.data");
            }
            catch (Exception)
            {
                return new List<Machine>();
            }
            
        }

        public static void SaveAll()
        {
            Servicios.Repository<List<Machine>>.Serialize(Contexto.Machines, "Machines.data");
        }

        public static void Add(Machine machine)
        {
            Contexto.Machines.Add(machine);
            SaveAll();
        }

        public static void Delete(Machine machine)
        {
            Contexto.Machines.Remove(machine);
            SaveAll();
        }

    }
}
