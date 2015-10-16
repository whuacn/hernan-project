using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;

namespace WindowsServiceTest
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            /* ServiceBase[] ServicesToRun;
             ServicesToRun = new ServiceBase[] 
             { 
                 new ServiceTest() 
             };
             ServiceBase.Run(ServicesToRun);*/

            try
            {
                if (Environment.UserInteractive)
                    ManagedInstallerClass.InstallHelper(new string[] { System.Reflection.Assembly.GetExecutingAssembly().Location });
                else
                    ServiceBase.Run(new ServiceTest());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
