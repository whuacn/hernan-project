using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace RemoteCmd
{
    public static class Remote
    {

        public static void Execute(string[] commands, string machine)
        {
            String sBatFile = "";
            if (machine != string.Empty)
                sBatFile = @"\\" + machine + "\\admin$\\RemoteCmd.bat";
            else
            {
                Console.WriteLine("Nombre de PC invalido");
                Environment.Exit(9);
            }

            String commandline = String.Join("\n\r", commands);

            try
            {
                if (File.Exists(sBatFile))
                    File.Delete(sBatFile);

                StreamWriter sw = new StreamWriter(sBatFile);
                sw.Write(commandline);
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrio un error al generar el archivo bat.\r\n" + ex.Message);
                Environment.Exit(9);
            }

            Execute(sBatFile, machine);
        }
        public static void Execute(string command, string machine)
        {
            ConnectionOptions connection = new ConnectionOptions();
            connection.Impersonation = ImpersonationLevel.Impersonate;
            connection.EnablePrivileges = true;
            ManagementPath managementPath = new ManagementPath("Win32_Process");
            ManagementScope manScope = new ManagementScope(String.Format("\\\\{0}\\root\\cimv2", machine), connection);
            ObjectGetOptions objectGetOptions = new ObjectGetOptions();

            try
            {
                manScope.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("No se ha podido conectar a la PC remota " + machine + ".\r\n" + ex.Message);
                Environment.Exit(9);
            }

            using (ManagementClass process = new ManagementClass(manScope, managementPath, objectGetOptions))
            {
                using (ManagementBaseObject inParams = process.GetMethodParameters("Create"))
                {
                    inParams["CommandLine"] = command;

                    using (ManagementBaseObject outParams = process.InvokeMethod("Create", inParams, null))
                    {

                        if ((uint)outParams["returnValue"] != 0)
                        {
                            Console.WriteLine("Error al iniciar el proceso " + command + ", ha devuelto el código de retorno " + outParams["returnValue"] + " en la PC remota " + machine);
                            Environment.Exit(Convert.ToInt32(outParams["returnValue"]));
                        }
                        uint ProcessId = (uint)outParams["processId"];
                    }
                }
            }
        }
       
        public static bool IsComputerAlive(string machine)
        {
            try
            {
                IPHostEntry host;
                host = Dns.GetHostEntry(machine);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
