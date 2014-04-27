using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Threading;

namespace RemoteCmd
{
    public class ProcessWMI
    {
        public uint ProcessId;
        public int ExitCode;
        public bool EventArrived;
        public ManualResetEvent mre = new ManualResetEvent(false);
        public void ProcessStoptEventArrived(object sender, EventArrivedEventArgs e)
        {
            if ((uint)e.NewEvent.Properties["ProcessId"].Value == ProcessId)
            {
                Console.WriteLine("Proceso: {0}, finalizado con el código: {1}", (int)(uint)e.NewEvent.Properties["ProcessId"].Value, (int)(uint)e.NewEvent.Properties["ExitStatus"].Value);
                ExitCode = (int)(uint)e.NewEvent.Properties["ExitStatus"].Value;

                foreach (PropertyData prop in e.NewEvent.Properties)
                {
                    Console.WriteLine(prop.Name + ": " + prop.Value);
                }
                EventArrived = true;
                mre.Set();
            }
        }
        public ProcessWMI()
        {
            this.ProcessId = 0;
            ExitCode = -1;
            EventArrived = false;
        }
        public void ExecuteRemoteProcessWMI(string remoteComputerName, string arguments) //, int WaitTimePerCommand
        {
            string strUserName = string.Empty;
            try
            {
                ConnectionOptions connOptions = new ConnectionOptions();
                connOptions.Impersonation = ImpersonationLevel.Impersonate;
                connOptions.EnablePrivileges = true;
                ManagementScope manScope = new ManagementScope(String.Format(@"\\{0}\ROOT\CIMV2", remoteComputerName), connOptions);

                try
                {
                    manScope.Connect();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error al conectar a la pc remota " + remoteComputerName);
                    Environment.Exit(8);
                }
                ObjectGetOptions objectGetOptions = new ObjectGetOptions();
                ManagementPath managementPath = new ManagementPath("Win32_Process");
                using (ManagementClass processClass = new ManagementClass(manScope, managementPath, objectGetOptions))
                {
                    using (ManagementBaseObject inParams = processClass.GetMethodParameters("Create"))
                    {
                        inParams["CommandLine"] = arguments;
                        using (ManagementBaseObject outParams = processClass.InvokeMethod("Create", inParams, null))
                        {

                            if ((uint)outParams["returnValue"] != 0)
                            {
                                Console.WriteLine("Error al iniciar el proceso " + arguments + " en la pc remota " + remoteComputerName + ", exit code: " + outParams["returnValue"]);
                                Environment.Exit(9);
                            }
                            this.ProcessId = (uint)outParams["processId"];
                        }
                    }
                }

                SelectQuery CheckProcess = new SelectQuery("Select * from Win32_Process Where ProcessId = " + ProcessId);
                using (ManagementObjectSearcher ProcessSearcher = new ManagementObjectSearcher(manScope, CheckProcess))
                {
                    using (ManagementObjectCollection MoC = ProcessSearcher.Get())
                    {
                        if (MoC.Count == 0)
                        {
                            Console.WriteLine("ADVERTENCIA: Proceso " + arguments + " terminado antes de que pudiera ser seguido en " + remoteComputerName);
                            Environment.Exit(10);
                        }

                       /* foreach (ManagementBaseObject mbo in MoC)
                        {
                            Console.WriteLine(mbo.GetText(new TextFormat()));
                        }*/
                    }
                }

                WqlEventQuery q = new WqlEventQuery("Win32_ProcessStopTrace");
                using (ManagementEventWatcher w = new ManagementEventWatcher(manScope, q))
                {
                    w.EventArrived += new EventArrivedEventHandler(this.ProcessStoptEventArrived);
                    w.Start();
                    if (!mre.WaitOne())
                    {
                        w.Stop();
                        this.EventArrived = false;
                    }
                }
                if (!this.EventArrived)
                {
                    SelectQuery sq = new SelectQuery("Select * from Win32_Process Where ProcessId = " + ProcessId);
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(manScope, sq))
                    {
                        foreach (ManagementObject queryObj in searcher.Get())
                        {
                            queryObj.InvokeMethod("Terminate", null);
                            queryObj.Dispose();
                            Console.WriteLine("El proceso " + arguments + " agotó el tiempo de espera en la PC remota " + remoteComputerName);
                            Environment.Exit(this.ExitCode);
                        }
                    }
                }
                else
                {
                    if (this.ExitCode != 0)
                    {
                        Console.WriteLine("El proceso " + arguments + " terminó con el codigo de salida " + this.ExitCode + " en " + remoteComputerName);
                        Environment.Exit(this.ExitCode);
                    }
                    else
                        Console.WriteLine("Proceso finalizado satisfactoriamente.");
                }

            }
            catch (Exception e)
            {
                //throw new Exception(string.Format("Execute process failed Machinename {0}, ProcessName {1}, RunAs {2}, Error is {3}, Stack trace {4}", remoteComputerName, arguments, strUserName, e.Message, e.StackTrace), e);
                throw e;
            }
        }
    }
}
