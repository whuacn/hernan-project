using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace SqlScript
{
    public class ArgsOptions
    {


        [Option('a',"auth", DefaultValue=AuthType.integrada, HelpText = "Define el tipo de autenticacion.")]
        public AuthType AuthType { get; set; }

        [Option('u', "user", HelpText = "Nombre de usuario.")]
        public string UserName { get; set; }

        [Option('p', "pass", HelpText = "Password.")]
        public string Password { get; set; }

        [Option('i', "instance", Required = true, HelpText = "Nombre de la instancia sql.")]
        public string Instance { get; set; }

        [Option('d', "database", Required = true, HelpText = "Nombre de la base de datos.")]
        public string DataBase { get; set; }

        [Option('c', "complete", HelpText = "Indica si se realiza el script completo de la base de datos.")]
        public bool Complete { get; set; }

        [Option('t', "tables", HelpText = "Indica si se realiza el script de las tablas.")]
        public bool Tables { get; set; }

        [Option('s', "storedprocedure", HelpText = "Indica si se realiza el script de los stored procedures.")]
        public bool StoredProcedure { get; set; }

        [Option('v', "view", HelpText = "Indica si se realiza el script de las vistas.")]
        public bool View { get; set; }

        [Option('f', "functions", HelpText = "Indica si se realiza el script de las funciones.")]
        public bool Functions { get; set; }

        [Option('m', "permission", DefaultValue = true, HelpText = "Indica si se agregan los permisos de los objetos.")]
        public bool Permission { get; set; }

        [Option('o', "output", Required = true, HelpText = "Nombre del archivo de texto de salida.")]
        public string OutPut { get; set; }
    }

    public enum AuthType
    {
        integrada,
        sql
    }
}
