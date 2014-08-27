using Microsoft.Tools.Common;
using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Tools.TestClient
{
	internal static class ToolingEnvironment
	{
		private const string svcConfigEditorName = "SvcConfigEditor.exe";

		private const string svcutilBinaryName = "svcutil.exe";

		internal readonly static string TestClientProjectFolderName;

		private readonly static string expectedDirectory;

		private static string svcConfigEditorPath;

		private static string svcutilPath;

		internal static string MetadataTool
		{
			get
			{
				if (ToolingEnvironment.svcutilPath == null)
				{
					string sdkPath = SdkPathUtility.GetSdkPath();
					if (sdkPath != null)
					{
						string str = Path.Combine(sdkPath, "svcutil.exe");
						if (File.Exists(str))
						{
							string str1 = str;
							ToolingEnvironment.svcutilPath = str1;
							return str1;
						}
					}
					string location = Assembly.GetExecutingAssembly().Location;
					string str2 = Path.Combine(Path.GetDirectoryName(location), "svcutil.exe");
					if (File.Exists(str2))
					{
						string str3 = str2;
						ToolingEnvironment.svcutilPath = str3;
						return str3;
					}
					string str4 = "svcutil.exe";
					if (File.Exists(str4))
					{
						string str5 = str4;
						ToolingEnvironment.svcutilPath = str5;
						return str5;
					}
				}
				return ToolingEnvironment.svcutilPath;
			}
		}

		internal static string SavedDataBase
		{
			get
			{
				if (!Directory.Exists(ToolingEnvironment.expectedDirectory))
				{
					try
					{
						Directory.CreateDirectory(ToolingEnvironment.expectedDirectory);
					}
					catch (IOException oException)
					{
						ApplicationSettings.ReportCreateFolderError(ToolingEnvironment.expectedDirectory);
					}
					catch (UnauthorizedAccessException unauthorizedAccessException)
					{
						ApplicationSettings.ReportCreateFolderError(ToolingEnvironment.expectedDirectory);
					}
				}
				return ToolingEnvironment.expectedDirectory;
			}
		}

		internal static string SvcConfigEditorPath
		{
			get
			{
				if (ToolingEnvironment.svcConfigEditorPath == null)
				{
					string sdkPath = SdkPathUtility.GetSdkPath();
					if (sdkPath != null)
					{
						string str = Path.Combine(sdkPath, "SvcConfigEditor.exe");
						if (File.Exists(str))
						{
							string str1 = str;
							ToolingEnvironment.svcConfigEditorPath = str1;
							return str1;
						}
					}
				}
				return ToolingEnvironment.svcConfigEditorPath;
			}
		}

		static ToolingEnvironment()
		{
			ToolingEnvironment.TestClientProjectFolderName = string.Concat("Test Client Projects\\", VersionNumbers.VSCurrentVersionString);
			ToolingEnvironment.expectedDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ToolingEnvironment.TestClientProjectFolderName);
		}
	}
}