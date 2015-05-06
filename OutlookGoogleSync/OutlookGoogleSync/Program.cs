using System;
using System.Net;
using System.Windows.Forms;

namespace OutlookGoogleSync
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
            ConfigProxy();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());

		}

        private static void ConfigProxy()
        {

             WebProxy wp;

                wp = new WebProxy();
                wp.Address = new System.Uri("http://172.20.0.77:8080/");
                wp.BypassProxyOnLocal = true;
                //wp.Credentials = new NetworkCredential("hhegykozi", "");
                WebRequest.DefaultWebProxy = wp;

        }
		
	}
}
