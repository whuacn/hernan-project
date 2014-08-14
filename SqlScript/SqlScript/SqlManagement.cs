using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;

namespace SqlScript
{
    public class SqlManagement
    {
        Server srv;
        
        public void Generar(ArgsOptions opt)
        {
            conectar(opt);
            Process(opt);
        }
        void conectar(ArgsOptions opt)
        {
            srv = new Server();
            if (opt.AuthType == AuthType.integrada)
            {
                srv.ConnectionContext.LoginSecure = true;
            }
            else
            {
                srv.ConnectionContext.LoginSecure = false;
                srv.ConnectionContext.Login = opt.UserName;
                srv.ConnectionContext.Password = opt.Password;
            }
            
            srv.ConnectionContext.ServerInstance = opt.Instance;
        }

        void Process(ArgsOptions opt)
        {
            Database db = new Database();
            db = srv.Databases[opt.DataBase];
            StringBuilder sb = new StringBuilder();

            if (opt.Tables || opt.Complete)
            {
                sb.Append(Environment.NewLine);
                sb.Append("--TABLES----------------");
                sb.Append(Environment.NewLine);
                foreach (Table tbl in db.Tables)
                {
                    ScriptingOptions options = new ScriptingOptions();
                    options.ClusteredIndexes = true;
                    options.Default = true;
                    options.DriAll = true;
                    options.Indexes = true;
                    options.IncludeHeaders = true;

                    if (opt.Permission)
                        options.Permissions = true;

                    StringCollection coll = tbl.Script(options);
                    foreach (string str in coll)
                    {
                        sb.Append(str);
                        sb.Append(Environment.NewLine);
                        sb.Append("GO");
                        sb.Append(Environment.NewLine);
                    }
                }
            }

            if (opt.StoredProcedure || opt.Complete)
            {
                sb.Append(Environment.NewLine);
                sb.Append("--STORED PROCEDURES----------------");
                sb.Append(Environment.NewLine);
                foreach (StoredProcedure sp in db.StoredProcedures)
                {
                    if (!sp.IsSystemObject)
                    {
                        ScriptingOptions options = new ScriptingOptions();
                        options.Default = true;
                        options.DriAll = true;
                        options.IncludeHeaders = true;

                        if (opt.Permission)
                            options.Permissions = true;

                        StringCollection coll = sp.Script(options);
                        foreach (string str in coll)
                        {
                            sb.Append(str);
                            sb.Append(Environment.NewLine);
                            sb.Append("GO");
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
            }

            if (opt.Functions || opt.Complete)
            {
                sb.Append(Environment.NewLine);
                sb.Append("--FUNCTIONS----------------");
                sb.Append(Environment.NewLine);
                foreach (UserDefinedFunction f in db.UserDefinedFunctions)
                {
                    if (!f.IsSystemObject)
                    {
                        ScriptingOptions options = new ScriptingOptions();
                        options.Default = true;
                        options.DriAll = true;
                        options.IncludeHeaders = true;

                        if (opt.Permission)
                            options.Permissions = true;

                        StringCollection coll = f.Script(options);
                        foreach (string str in coll)
                        {
                            sb.Append(str);
                            sb.Append(Environment.NewLine);
                            sb.Append("GO");
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
            }

            if (opt.View || opt.Complete)
            {
                sb.Append(Environment.NewLine);
                sb.Append("--VIEWS----------------");
                sb.Append(Environment.NewLine);
                foreach (View v in db.Views)
                {
                    if (!v.IsSystemObject)
                    {
                        ScriptingOptions options = new ScriptingOptions();
                        options.Default = true;
                        options.DriAll = true;
                        options.IncludeHeaders = true;

                        if (opt.Permission)
                            options.Permissions = true;

                        StringCollection coll = v.Script(options);
                        foreach (string str in coll)
                        {
                            sb.Append(str);
                            sb.Append(Environment.NewLine);
                            sb.Append("GO");
                            sb.Append(Environment.NewLine);
                        }
                    }
                }
            }
            System.IO.StreamWriter fs = System.IO.File.CreateText(opt.OutPut);
            fs.Write(sb.ToString());
            fs.Close();

        }

    }
}
