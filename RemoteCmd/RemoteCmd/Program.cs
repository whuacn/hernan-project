using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RemoteCmd
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 2)
                Console.WriteLine(@"Debe ingresar al menos 2 argumentos \\nombredePC y comandos a ejecutar");

            string pcname = args[0].Replace(@"\\", "");
            args = args.Where(w => w != args[0]).ToArray();
            string commandline = String.Join(" ", args); 
            Remote.Execute(commandline, pcname);

        }

        public static void ExecuteCommandAsync(object command)
        {
            ProcessWMI remote = new ProcessWMI();
            remote.ExecuteRemoteProcessWMI("HERNANDESKTOP", command.ToString());
        }
    }
}
