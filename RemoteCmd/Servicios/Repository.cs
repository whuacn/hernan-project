using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Servicios
{
    public class Repository<T>
    {
        static string path = "Repo/";
        public static T Deserialize(string file)
        {
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(T));
                StreamReader reader = new StreamReader(path + file);
                T obj = (T)x.Deserialize(reader);
                reader.Close();
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al obtener " + file);
            }
        }

        public static void Serialize(T obj, string file)
        {
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(T));
                StreamWriter writer = new StreamWriter(path + file);
                x.Serialize(writer, obj);
                writer.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error al guardar " + file);
            }
        }
           
    }
}
