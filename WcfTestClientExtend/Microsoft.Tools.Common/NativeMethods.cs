using System;
using System.Runtime.InteropServices;

namespace Microsoft.Tools.Common
{
	internal static class NativeMethods
	{
		public const int ENDSESSION_CLOSEAPP = 1;

		public const int SMTO_ABORTIFHUNG = 2;

		public const int WM_CLOSE = 16;

		public const int WM_ENDSESSION = 22;

		public const int WM_MOUSEMOVE = 512;

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int EnumWindows(Microsoft.Tools.Common.NativeMethods.EnumWindowsCallback func, IntPtr lParam);

		public static bool Failed(int hr)
		{
			return !Microsoft.Tools.Common.NativeMethods.Succeeded(hr);
		}

		[DllImport("user32.dll", CharSet=CharSet.Unicode, ExactSpelling=false, SetLastError=true)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr GetDesktopWindow();

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern bool GetWindowRect(IntPtr hWnd, out Microsoft.Tools.Common.NativeMethods.RECT lpRect);

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern uint GetWindowThreadProcessId(SafeHandle hwnd, out int lpdwProcessId);

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern int IsWindow(SafeHandle hWnd);

		[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=false)]
		public static extern IntPtr SendMessage(SafeHandle hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern IntPtr SendMessageTimeout(SafeHandle hWnd, int msg, IntPtr wParam, IntPtr lParam, int flags, int timeout, out IntPtr pdwResult);

		[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		public static extern void SetLastError(uint dwErrCode);

		[DllImport("User32", CharSet=CharSet.Auto, ExactSpelling=true)]
		public static extern IntPtr SetParent(IntPtr hWnd, IntPtr hWndParent);

		public static bool Succeeded(int hr)
		{
			return hr >= 0;
		}

		public delegate bool EnumWindowsCallback(IntPtr hwnd, IntPtr lParam);

		public struct RECT
		{
			public int left;

			public int top;

			public int right;

			public int bottom;
		}
	}
}