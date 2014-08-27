using Microsoft.Win32;
using System;
using System.Security;

namespace Microsoft.Tools.Common
{
	internal static class SdkPathUtility
	{
		private static string GetRegistryValue(string registryPath, string registryValueName)
		{
			string value;
			try
			{
				using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
				{
					using (RegistryKey registryKey1 = registryKey.OpenSubKey(registryPath, false))
					{
						value = registryKey1.GetValue(registryValueName, string.Empty) as string;
					}
				}
			}
			catch (UnauthorizedAccessException unauthorizedAccessException)
			{
				value = string.Empty;
			}
			catch (SecurityException securityException)
			{
				value = string.Empty;
			}
			return value;
		}

		internal static string GetSdkPath()
		{
			return SdkPathUtility.GetSdkPath(VersionNumbers.NETFxCurrentVersion);
		}

		internal static string GetSdkPath(Version targetFrameworkVersion)
		{
			string str = "SOFTWARE\\Microsoft\\Microsoft SDKs\\Windows";
			string str1 = string.Concat(str, "\\", VersionNumbers.GetMSSDKVersionString(targetFrameworkVersion));
			string str2 = "InstallationFolder";
			string str3 = "CurrentInstallFolder";
			str1 = (targetFrameworkVersion > new Version(3, 5) ? string.Concat(str1, "\\WinSDK-NetFx40Tools-x86") : string.Concat(str1, "\\WinSDK-NetFx35Tools-x86"));
			string registryValue = SdkPathUtility.GetRegistryValue(str1, str2);
			if (string.IsNullOrEmpty(registryValue))
			{
				registryValue = SdkPathUtility.GetRegistryValue(str, str3);
				if (!string.IsNullOrEmpty(registryValue))
				{
					registryValue = string.Concat(registryValue, "bin\\");
				}
			}
			return registryValue;
		}
	}
}