using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.SqlEnum;
using System.Configuration;
using System.Collections.Specialized;

namespace SMOTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Server srv = new Server();
            srv.ConnectionContext.LoginSecure = false;
            srv.ConnectionContext.Login = "sa";
            srv.ConnectionContext.Password = "123456";
            srv.ConnectionContext.ServerInstance = "localhost";
            string dbName = "sundae";

            Database db = new Database();
            db = srv.Databases[dbName];

            StringBuilder sb = new StringBuilder();
            Console.WriteLine("Conectado");
            foreach (Table tbl in db.Tables)
            {
                ScriptingOptions options = new ScriptingOptions();
                options.ClusteredIndexes = true;
                options.Default = true;
                options.DriAll = true;
                options.Indexes = true;
                options.IncludeHeaders = true;
                options.Permissions = true;
                
                StringCollection coll = tbl.Script(options);
                foreach (string str in coll)
                {
                    sb.Append(str);
                    sb.Append(Environment.NewLine);
                }
                Console.WriteLine(String.Format("TABLA: {0} creada",tbl.Name));
            }

            foreach (StoredProcedure sp in db.StoredProcedures)
            {
                if (!sp.IsSystemObject)
                {
                    ScriptingOptions options = new ScriptingOptions();
                    options.Default = true;
                    options.DriAll = true;
                    options.IncludeHeaders = true;
                    options.Permissions = true;

                    //sb.Append(sp.TextBody);
                    //sb.Append(Environment.NewLine);
                    StringCollection coll = sp.Script(options);
                    foreach (string str in coll)
                    {
                        sb.Append(str);
                        sb.Append(Environment.NewLine);
                    }
                    Console.WriteLine(String.Format("SP: {0} creado", sp.Name));
                }
                else
                {

                    //Console.WriteLine(String.Format("SP: {0} NO creado", sp.Name));
                }
            }
            System.IO.StreamWriter fs = System.IO.File.CreateText("d:\\output.txt");
            fs.Write(sb.ToString());
            fs.Close();
        }
    }
}
