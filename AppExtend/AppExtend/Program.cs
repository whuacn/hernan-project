using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Contracts;

namespace AppExtend
{
    static class Program
    {
        public static frmPrincipal FormPrincipal;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormPrincipal = new frmPrincipal();
            CargarMenu();
            Application.Run(FormPrincipal);


        }

        static void CargarMenu()
        {
            List<IMenu> PlugMenu = Plugin.PluginManager.LoadPluginsOf<IMenu>(false, "Plugin/", String.Empty);
            AppExtend.Plugin.LoadMenu.Load(PlugMenu);
        }
    }
}
