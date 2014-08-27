using System;
using System.Globalization;
using System.Windows.Forms;

namespace Microsoft.Tools.TestClient
{
	internal static class RtlAwareMessageBox
	{
		private static bool IsRightToLeft(IWin32Window owner)
		{
			Control control = owner as Control;
			if (control == null)
			{
				return CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
			}
			return control.RightToLeft == RightToLeft.Yes;
		}

		public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
		{
			return RtlAwareMessageBox.Show(null, text, caption, buttons, icon, defaultButton, options);
		}

		public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
		{
			if (RtlAwareMessageBox.IsRightToLeft(owner))
			{
				options = options | MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
			}
			return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
		}
	}
}