using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.Tools.Common
{
	internal static class VersionNumbers
	{
		public readonly static Version VSCurrentVersion;

		public readonly static string VSCurrentVersionString;

		private readonly static Version MSSDKVersion80;

		private readonly static Version MSSDKVersion81;

		public readonly static Version NETFxCurrentVersion;

		static VersionNumbers()
		{
			VersionNumbers.VSCurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;
			VersionNumbers.VSCurrentVersionString = VersionNumbers.VSCurrentVersion.ToString(2);
			VersionNumbers.MSSDKVersion80 = new Version(8, 0);
			VersionNumbers.MSSDKVersion81 = new Version(8, 1);
			VersionNumbers.NETFxCurrentVersion = typeof(object).Assembly.GetName().Version;
		}

		public static string GetMSSDKVersionString(Version targetFrameworkVersion)
		{
			Version version;
			version = (targetFrameworkVersion > new Version(3, 5) ? VersionNumbers.MSSDKVersion81 : VersionNumbers.MSSDKVersion80);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			object[] str = new object[] { version.ToString(2) };
			return string.Format(invariantCulture, "v{0}A", str);
		}
	}
}