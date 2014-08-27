using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Microsoft.Tools.TestClient
{
    public class Repository<T>
    {
        static string path = "";

        public static T Deserialize(string file)
        {
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(T));
                StreamReader reader = new StreamReader(file);
                T obj = (T)x.Deserialize(reader);
                reader.Close();
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al obtener " + file + ". " + ex.Message);
            }
        }
        public static void Serialize(T obj, string file)
        {
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(T));
                StreamWriter writer = new StreamWriter(file);
                x.Serialize(writer, obj);
                writer.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al guardar " + file + ". " + ex.Message);
            }
        }
    }
}
