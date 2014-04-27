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
            bool cmd = false;
            if (args.Length < 2)
            {
                Console.WriteLine(@"Debe ingresar al menos 2 argumentos \\nombredePC y comandos a ejecutar");
                Environment.Exit(9);
            }

            string pcname = args[0].Replace(@"\\", "");
            
            if (args[1] == "/c")
            {
                cmd = true;
                args = args.Where(w => w != args[1]).ToArray();
            }
            args = args.Where(w => w != args[0]).ToArray();            
            string commandline = String.Join(" ", args);

            if (cmd)
            {
                commandline = "cmd.exe /c " + commandline;
            }

            Remote.Execute(commandline, pcname);

        }

        public static void ExecuteCommandAsync(object command)
        {
            ProcessWMI remote = new ProcessWMI();
            remote.ExecuteRemoteProcessWMI("HERNANDESKTOP", command.ToString());
        }
    }
}
