using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
namespace Persits.PDF
{
	internal class UserManager
	{
		private const int LOGON32_LOGON_INTERACTIVE = 2;
		private const int LOGON32_LOGON_NETWORK = 3;
		private const int LOGON32_LOGON_BATCH = 4;
		private const int LOGON32_LOGON_SERVICE = 5;
		private const int LOGON32_LOGON_UNLOCK = 7;
		private const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
		private const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
		private const int LOGON32_PROVIDER_DEFAULT = 0;
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
		[DllImport("advapi32.dll", SetLastError = true)]
		public static extern int ImpersonateLoggedOnUser(IntPtr hToken);
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern int RevertToSelf();
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int CloseHandle(IntPtr hObject);
		public void LogonUser(string Domain, string Username, string Password)
		{
			IntPtr intPtr;
			if (UserManager.LogonUser(Username, Domain, Password, 2, 0, out intPtr) == 0)
			{
				Win32Exception ex = new Win32Exception(Marshal.GetLastWin32Error());
				AuxException.Throw(ex.Message, PdfErrors._ERROR_LOGONUSER);
			}
			if (UserManager.ImpersonateLoggedOnUser(intPtr) == 0)
			{
				Win32Exception ex2 = new Win32Exception(Marshal.GetLastWin32Error());
				AuxException.Throw(ex2.Message, PdfErrors._ERROR_LOGONUSER);
			}
			UserManager.CloseHandle(intPtr);
		}
		public void Revert()
		{
			UserManager.RevertToSelf();
		}
	}
}
