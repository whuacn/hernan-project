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
using System.Windows.Forms;

namespace SqlExpressTraceUI
{



	public partial class AboutDlg : Form
	{
		public AboutDlg()
		{
			InitializeComponent();
		}

		private void onLoad(object sender, System.EventArgs e)
		{
			this.disclaimerLbl.Text =
				"2014 Pete_H\r\n\r\n" +
				"This software released under the Code Project Open License. Refer to the link below or " +
				"the copy of the Code Project Open License (CPOL.htm) included with this solution.\r\n\r\n" +
				"This code and the compiled components including libraries and the demonstration application " +
				"have been made available only for the purpose of learning, sharing and demonstrating ideas " +
				"and NOT to imply, recommend or suggest usage of any part of the code or components.\r\n\r\n" +
				"No claim of suitability, guarantee, or any warranty whatsoever is provided. The software is " +
				"provided \"as-is\". Usage of any of this code or components is entirely at your own risk.";
		}

		private void onLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(this.link.Text);
		}
	}
}