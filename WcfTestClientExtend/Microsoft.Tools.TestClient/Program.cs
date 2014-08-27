using Microsoft.Tools.TestClient.UI;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient
{
	internal static class Program
	{
		[LoaderOptimization(LoaderOptimization.MultiDomainHost)]
		[STAThread]
		internal static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			if (args != null && (int)args.Length > 0)
			{
				if (string.Equals(args[0], "/ProjectPath", StringComparison.OrdinalIgnoreCase))
				{
					if ((int)args.Length != 2)
					{
						Program.ShowUsage();
						return;
					}
					Program.SetProjectBase(args[1]);
					return;
				}
				if (string.Equals(args[0], "/RestoreProjectPath", StringComparison.OrdinalIgnoreCase))
				{
					if ((int)args.Length != 1)
					{
						Program.ShowUsage();
						return;
					}
					ApplicationSettings.GetInstance().ProjectBase = string.Empty;
					return;
				}
				string[] strArrays = args;
				int num = 0;
				while (num < (int)strArrays.Length)
				{
					string str = strArrays[num];
					if (string.Equals("/?", str, StringComparison.Ordinal) || string.Equals("-?", str, StringComparison.Ordinal))
					{
						Program.ShowUsage();
						return;
					}
					else
					{
						num++;
					}
				}
			}
			Application.Run(new MainForm(args));
		}

		private static void ReportError(string error)
		{
			RtlAwareMessageBox.Show(error, StringResources.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 0);
		}

		private static void SetProjectBase(string value)
		{
			value = Path.GetFullPath(value);
			if (Directory.Exists(value))
			{
				ApplicationSettings.GetInstance().ProjectBase = value;
				return;
			}
			CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
			string directoryNotExist = StringResources.DirectoryNotExist;
			object[] objArray = new object[] { value };
			Program.ReportError(string.Format(currentUICulture, directoryNotExist, objArray));
		}

		private static void ShowUsage()
		{
			RtlAwareMessageBox.Show(StringResources.Usage, StringResources.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, 0);
		}
	}
}