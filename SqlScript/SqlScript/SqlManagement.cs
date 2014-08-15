using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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
                if (!opt.MultipleFiles)
                {
                    sb.Append(Environment.NewLine);
                    sb.Append("--TABLES----------------");
                    sb.Append(Environment.NewLine);
                }
                StringBuilder sbtemp = new StringBuilder();
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
                        sbtemp.Append(str);
                        sbtemp.Append(Environment.NewLine);
                        sbtemp.Append("GO");
                        sbtemp.Append(Environment.NewLine);
                    }

                    if (opt.MultipleFiles)
                    {
                        System.IO.StreamWriter fs = System.IO.File.CreateText(Path.Combine(opt.OutPut,tbl.Name + ".sql"));
                        fs.Write(sbtemp.ToString());
                        fs.Close();
                        sbtemp = null;
                        sbtemp = new StringBuilder();
                    }
                }

                if (!opt.MultipleFiles)                
                    sb.Append(sbtemp);
                sbtemp = null;

            }

            if (opt.StoredProcedure || opt.Complete)
            {
                if (!opt.MultipleFiles)
                {
                    sb.Append(Environment.NewLine);
                    sb.Append("--STORED PROCEDURES----------------");
                    sb.Append(Environment.NewLine);
                }
                StringBuilder sbtemp = new StringBuilder();
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
                            sbtemp.Append(str);
                            sbtemp.Append(Environment.NewLine);
                            sbtemp.Append("GO");
                            sbtemp.Append(Environment.NewLine);
                        }

                        if (opt.MultipleFiles)
                        {
                            System.IO.StreamWriter fs = System.IO.File.CreateText(Path.Combine(opt.OutPut, sp.Name + ".sql"));
                            fs.Write(sbtemp.ToString());
                            fs.Close();
                            sbtemp = null;
                            sbtemp = new StringBuilder();
                        }
                    }
                }

                if (!opt.MultipleFiles)
                    sb.Append(sbtemp);
                sbtemp = null;
            }

            if (opt.Functions || opt.Complete)
            {
                if (!opt.MultipleFiles)
                {
                    sb.Append(Environment.NewLine);
                    sb.Append("--FUNCTIONS----------------");
                    sb.Append(Environment.NewLine);
                }
                StringBuilder sbtemp = new StringBuilder();
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
                            sbtemp.Append(str);
                            sbtemp.Append(Environment.NewLine);
                            sbtemp.Append("GO");
                            sbtemp.Append(Environment.NewLine);
                        }
                        if (opt.MultipleFiles)
                        {
                            System.IO.StreamWriter fs = System.IO.File.CreateText(Path.Combine(opt.OutPut, f.Name + ".sql"));
                            fs.Write(sbtemp.ToString());
                            fs.Close();
                            sbtemp = null;
                            sbtemp = new StringBuilder();
                        }
                    }
                }

                if (!opt.MultipleFiles)
                    sb.Append(sbtemp);
                sbtemp = null;
            }

            if (opt.View || opt.Complete)
            {
                if (!opt.MultipleFiles)
                {
                    sb.Append(Environment.NewLine);
                    sb.Append("--VIEWS----------------");
                    sb.Append(Environment.NewLine);
                }
                StringBuilder sbtemp = new StringBuilder();
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
                            sbtemp.Append(str);
                            sbtemp.Append(Environment.NewLine);
                            sbtemp.Append("GO");
                            sbtemp.Append(Environment.NewLine);
                        }
                        if (opt.MultipleFiles)
                        {
                            System.IO.StreamWriter fs = System.IO.File.CreateText(Path.Combine(opt.OutPut, v.Name + ".sql"));
                            fs.Write(sbtemp.ToString());
                            fs.Close();
                            sbtemp = null;
                            sbtemp = new StringBuilder();
                        }
                    }
                }
                if (!opt.MultipleFiles)
                    sb.Append(sbtemp);
                sbtemp = null;
            }
            if (!opt.MultipleFiles)
            {
                System.IO.StreamWriter fs = System.IO.File.CreateText(opt.OutPut);
                fs.Write(sb.ToString());
                fs.Close();
            }
            sb = null;

        }

    }
}
