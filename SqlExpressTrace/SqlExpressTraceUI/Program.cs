/************************************************ 2014 Pete_H *******************************************************
 * 
 * This software released under the Code Project Open License. Refer to: http://www.codeproject.com/info/cpol10.aspx
 * or refer to the copy of the Code Project Open License (CPOL.htm) included with this solution. 
 * 
 * This code and the compiled components including libraries and the demonstration application have been made 
 * available only for the purpose of learning, sharing and demonstrating ideas and NOT to imply, recommend or 
 * suggest usage of any part of the code or components.
 * 
 * No claim of suitability, guarantee, or any warranty whatsoever is provided. The software is provided "as-is"
 * Usage of any of this code or components is entirely at your own risk.
 * 
 ********************************************************************************************************************/
using System;
using System.Windows.Forms;

namespace SqlTraceExpressUI
{
	internal static class Program
	{
		public static void DisplayException(IWin32Window owner, Exception ex,
					string caption = "An error occurred in Sql ExpressTrace.")
		{
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			MessageBoxIcon icon = MessageBoxIcon.Error;

			MessageBox.Show(owner, ex.ToString(), caption, buttons, icon);
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Application.ThreadException += onApplicationThreadException;
			Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
			AppDomain.CurrentDomain.UnhandledException += onUnhandledException;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Application.Run(new MainDlg());
		}

		private static void onApplicationThreadException(object sender, System.Threading.ThreadExceptionEventArgs ex)
		{
			DisplayException(null, ex.Exception, "from onApplicationThreadException");
		}

		private static void onUnhandledException(object sender, UnhandledExceptionEventArgs ex)
		{
			DisplayException(null, (Exception)ex.ExceptionObject, "from onUnhandledException");
		}
	}
}