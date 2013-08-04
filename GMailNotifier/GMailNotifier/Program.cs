using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GMailNotifier.Engine;

namespace GMailNotifier
{
    static class Program
    {
        internal static Inbox inbox = null;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmNotifier());
        }
    }
}
