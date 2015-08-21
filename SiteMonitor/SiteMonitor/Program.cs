using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SiteMonitor
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Form1 form = new Form1();
      form.Opacity = 0;
      form.StartMonitoring();
      Application.Run(form);
    }
  }
}