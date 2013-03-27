using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Entidades;

namespace Servicios
{
    public static class Remote
    {

        public static void Execute(string[] commands, string machine)
        {
            String sBatFile = "";
            if (machine != string.Empty)
                sBatFile = @"\\" + machine + "\\admin$\\SiproProcess.bat";
            else
                throw new Exception("Nombre de PC invalido");

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
                throw new Exception("Ocurrio un error al generar el archivo bat.\r\n" + ex.Message);
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
                throw new Exception("No se ha podido conectar a la PC remota " + machine + ".\r\n" + ex.Message);
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
                            throw new Exception("Error al iniciar el proceso " + command + ", ha devuelto el código de retorno " + outParams["returnValue"] + " en la PC remota " + machine);
                        }
                        uint ProcessId = (uint)outParams["processId"];
                    }
                }
            }
        }
        public static bool TestPort(Machine machine, int port)
        {
            bool test = false;

            try
            {

                using (TcpClient tcp = new TcpClient())
                {
                    IAsyncResult ar = tcp.BeginConnect(machine.Name, port, null, null);
                    System.Threading.WaitHandle wh = ar.AsyncWaitHandle;
                    try
                    {
                        if (!ar.AsyncWaitHandle.WaitOne(500, false))
                        {
                            tcp.Close();
                            test = false;
                            throw new TimeoutException();
                        }

                        tcp.EndConnect(ar);
                    }
                    finally
                    {
                        wh.Close();

                    }
                }
                /*
                System.Net.Sockets.Socket s = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                s.Connect(machine.Name, port);
                if (s.Connected == true)
                    test = true;
                s.Close();
                 */
                //TcpClient c = new TcpClient(machine.Name, port);
                test = true;
            }
            catch (Exception)
            {
                test = false;
            }
            

            return test;
        }


        public static bool IsComputerAlive(Machine machine)
        {
            try
            {
                /*IPAddress[] addresslist = Dns.GetHostAddresses(machine.Name);
                machine.IP = addresslist[0].AddressFamily.ToString();
                */
                IPHostEntry host;
                host = Dns.GetHostEntry(machine.Name);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
