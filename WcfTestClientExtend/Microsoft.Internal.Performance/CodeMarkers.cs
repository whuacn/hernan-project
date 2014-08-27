using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Internal.Performance
{
	internal sealed class CodeMarkers
	{
		private const string AtomName = "VSCodeMarkersEnabled";

		private const string TestDllName = "Microsoft.Internal.Performance.CodeMarkers.dll";

		private const string ProductDllName = "Microsoft.VisualStudio.CodeMarkers.dll";

		public readonly static CodeMarkers Instance;

		private CodeMarkers.State state;

		private RegistryView registryView;

		private string regroot;

		private bool? shouldUseTestDll;

		public bool IsEnabled
		{
			get
			{
				return this.state == CodeMarkers.State.Enabled;
			}
		}

		public bool ShouldUseTestDll
		{
			get
			{
				if (!this.shouldUseTestDll.HasValue)
				{
					try
					{
						if (this.regroot != null)
						{
							this.shouldUseTestDll = new bool?(CodeMarkers.UsePrivateCodeMarkers(this.regroot, this.registryView));
						}
						else
						{
							this.shouldUseTestDll = new bool?(CodeMarkers.NativeMethods.GetModuleHandle("Microsoft.VisualStudio.CodeMarkers.dll") == IntPtr.Zero);
						}
					}
					catch (Exception exception)
					{
						this.shouldUseTestDll = new bool?(true);
					}
				}
				return this.shouldUseTestDll.Value;
			}
		}

		static CodeMarkers()
		{
			CodeMarkers.Instance = new CodeMarkers();
		}

		private CodeMarkers()
		{
			this.state = (CodeMarkers.NativeMethods.FindAtom("VSCodeMarkersEnabled") != 0 ? CodeMarkers.State.Enabled : CodeMarkers.State.Disabled);
		}

		public bool CodeMarker(int nTimerID)
		{
			bool flag;
			if (!this.IsEnabled)
			{
				return false;
			}
			try
			{
				if (!this.ShouldUseTestDll)
				{
					CodeMarkers.NativeMethods.ProductDllPerfCodeMarker(nTimerID, null, 0);
				}
				else
				{
					CodeMarkers.NativeMethods.TestDllPerfCodeMarker(nTimerID, null, 0);
				}
				return true;
			}
			catch (DllNotFoundException dllNotFoundException)
			{
				this.state = CodeMarkers.State.DisabledDueToDllImportException;
				flag = false;
			}
			return flag;
		}

		public bool CodeMarkerEx(int nTimerID, byte[] aBuff)
		{
			bool flag;
			if (!this.IsEnabled)
			{
				return false;
			}
			if (aBuff == null)
			{
				throw new ArgumentNullException("aBuff");
			}
			try
			{
				if (!this.ShouldUseTestDll)
				{
					CodeMarkers.NativeMethods.ProductDllPerfCodeMarker(nTimerID, aBuff, (int)aBuff.Length);
				}
				else
				{
					CodeMarkers.NativeMethods.TestDllPerfCodeMarker(nTimerID, aBuff, (int)aBuff.Length);
				}
				return true;
			}
			catch (DllNotFoundException dllNotFoundException)
			{
				this.state = CodeMarkers.State.DisabledDueToDllImportException;
				flag = false;
			}
			return flag;
		}

		public bool CodeMarkerEx(int nTimerID, Guid guidData)
		{
			return this.CodeMarkerEx(nTimerID, guidData.ToByteArray());
		}

		public bool CodeMarkerEx(int nTimerID, string stringData)
		{
			bool flag;
			if (!this.IsEnabled)
			{
				return false;
			}
			if (stringData == null)
			{
				throw new ArgumentNullException("stringData");
			}
			try
			{
				int byteCount = Encoding.Unicode.GetByteCount(stringData) + 2;
				if (!this.ShouldUseTestDll)
				{
					CodeMarkers.NativeMethods.ProductDllPerfCodeMarkerString(nTimerID, stringData, byteCount);
				}
				else
				{
					CodeMarkers.NativeMethods.TestDllPerfCodeMarkerString(nTimerID, stringData, byteCount);
				}
				return true;
			}
			catch (DllNotFoundException dllNotFoundException)
			{
				this.state = CodeMarkers.State.DisabledDueToDllImportException;
				flag = false;
			}
			return flag;
		}

		public bool CodeMarkerEx(int nTimerID, uint uintData)
		{
			return this.CodeMarkerEx(nTimerID, BitConverter.GetBytes(uintData));
		}

		public bool CodeMarkerEx(int nTimerID, ulong ulongData)
		{
			return this.CodeMarkerEx(nTimerID, BitConverter.GetBytes(ulongData));
		}

		public bool InitPerformanceDll(int iApp, string strRegRoot)
		{
			return this.InitPerformanceDll(iApp, strRegRoot, RegistryView.Default);
		}

		public bool InitPerformanceDll(int iApp, string strRegRoot, RegistryView registryView)
		{
			if (this.IsEnabled)
			{
				return true;
			}
			if (strRegRoot == null)
			{
				throw new ArgumentNullException("strRegRoot");
			}
			this.regroot = strRegRoot;
			this.registryView = registryView;
			try
			{
				if (!this.ShouldUseTestDll)
				{
					CodeMarkers.NativeMethods.ProductDllInitPerf(iApp);
				}
				else
				{
					CodeMarkers.NativeMethods.TestDllInitPerf(iApp);
				}
				this.state = CodeMarkers.State.Enabled;
				CodeMarkers.NativeMethods.AddAtom("VSCodeMarkersEnabled");
			}
			catch (BadImageFormatException badImageFormatException)
			{
				this.state = CodeMarkers.State.DisabledDueToDllImportException;
			}
			catch (DllNotFoundException dllNotFoundException)
			{
				this.state = CodeMarkers.State.DisabledDueToDllImportException;
				return false;
			}
			return true;
		}

		public void SetStateDLLException()
		{
			this.state = CodeMarkers.State.DisabledDueToDllImportException;
		}

		internal static byte[] StringToBytesZeroTerminated(string stringData)
		{
			Encoding unicode = Encoding.Unicode;
			byte[] numArray = new byte[unicode.GetByteCount(stringData) + 2];
			unicode.GetBytes(stringData, 0, stringData.Length, numArray, 0);
			return numArray;
		}

		public void UninitializePerformanceDLL(int iApp)
		{
			bool? nullable = this.shouldUseTestDll;
			this.shouldUseTestDll = null;
			this.regroot = null;
			if (!this.IsEnabled)
			{
				return;
			}
			this.state = CodeMarkers.State.Disabled;
			ushort num = CodeMarkers.NativeMethods.FindAtom("VSCodeMarkersEnabled");
			if (num != 0)
			{
				CodeMarkers.NativeMethods.DeleteAtom(num);
			}
			try
			{
				if (nullable.HasValue)
				{
					if (!nullable.Value)
					{
						CodeMarkers.NativeMethods.ProductDllUnInitPerf(iApp);
					}
					else
					{
						CodeMarkers.NativeMethods.TestDllUnInitPerf(iApp);
					}
				}
			}
			catch (DllNotFoundException dllNotFoundException)
			{
			}
		}

		private static bool UsePrivateCodeMarkers(string regRoot, RegistryView registryView)
		{
			bool flag;
			if (regRoot == null)
			{
				throw new ArgumentNullException("regRoot");
			}
			using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
			{
				using (RegistryKey registryKey1 = registryKey.OpenSubKey(string.Concat(regRoot, "\\Performance")))
				{
					if (registryKey1 != null)
					{
						string str = registryKey1.GetValue(string.Empty).ToString();
						flag = !string.IsNullOrEmpty(str);
						return flag;
					}
				}
				return false;
			}
			return flag;
		}

		private static class NativeMethods
		{
			[DllImport("kernel32.dll", CharSet=CharSet.Unicode, ExactSpelling=false)]
			public static extern ushort AddAtom(string lpString);

			[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
			public static extern ushort DeleteAtom(ushort atom);

			[DllImport("kernel32.dll", CharSet=CharSet.Unicode, ExactSpelling=false)]
			public static extern ushort FindAtom(string lpString);

			[DllImport("kernel32.dll", CharSet=CharSet.Unicode, ExactSpelling=false)]
			public static extern IntPtr GetModuleHandle(string lpModuleName);

			[DllImport("Microsoft.VisualStudio.CodeMarkers.dll", CharSet=CharSet.None, EntryPoint="InitPerf", ExactSpelling=false)]
			public static extern void ProductDllInitPerf(int iApp);

			[DllImport("Microsoft.VisualStudio.CodeMarkers.dll", CharSet=CharSet.None, EntryPoint="PerfCodeMarker", ExactSpelling=false)]
			public static extern void ProductDllPerfCodeMarker(int nTimerID, byte[] aUserParams, int cbParams);

			[DllImport("Microsoft.VisualStudio.CodeMarkers.dll", CharSet=CharSet.None, EntryPoint="PerfCodeMarker", ExactSpelling=false)]
			public static extern void ProductDllPerfCodeMarkerString(int nTimerID, string aUserParams, int cbParams);

			[DllImport("Microsoft.VisualStudio.CodeMarkers.dll", CharSet=CharSet.None, EntryPoint="UnInitPerf", ExactSpelling=false)]
			public static extern void ProductDllUnInitPerf(int iApp);

			[DllImport("Microsoft.Internal.Performance.CodeMarkers.dll", CharSet=CharSet.None, EntryPoint="InitPerf", ExactSpelling=false)]
			public static extern void TestDllInitPerf(int iApp);

			[DllImport("Microsoft.Internal.Performance.CodeMarkers.dll", CharSet=CharSet.None, EntryPoint="PerfCodeMarker", ExactSpelling=false)]
			public static extern void TestDllPerfCodeMarker(int nTimerID, byte[] aUserParams, int cbParams);

			[DllImport("Microsoft.Internal.Performance.CodeMarkers.dll", CharSet=CharSet.None, EntryPoint="PerfCodeMarker", ExactSpelling=false)]
			public static extern void TestDllPerfCodeMarkerString(int nTimerID, string aUserParams, int cbParams);

			[DllImport("Microsoft.Internal.Performance.CodeMarkers.dll", CharSet=CharSet.None, EntryPoint="UnInitPerf", ExactSpelling=false)]
			public static extern void TestDllUnInitPerf(int iApp);
		}

		private enum State
		{
			Enabled,
			Disabled,
			DisabledDueToDllImportException
		}
	}
}