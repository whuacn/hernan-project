using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Microsoft.Tools.Common
{
	internal static class SafeClipboard
	{
		public static void Clear()
		{
			try
			{
				Clipboard.Clear();
			}
			catch (ExternalException externalException)
			{
			}
		}

		public static void SetText(string text)
		{
			try
			{
				Clipboard.SetText(text);
			}
			catch (ExternalException externalException)
			{
			}
		}
	}
}