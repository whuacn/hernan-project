using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FSWatcherLib;
using System.IO;

namespace FSWatcher
{
    static class Program
    {
        public static List<FileSystemWatcherExt> Watchers = new List<FileSystemWatcherExt>();
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Principal());
        }
        
    }
}
