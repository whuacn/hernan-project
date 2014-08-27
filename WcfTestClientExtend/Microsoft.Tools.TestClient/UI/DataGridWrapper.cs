using Microsoft.Tools.TestClient;
using System;
using System.Windows.Forms;
using WcfTestClient;

namespace Microsoft.Tools.TestClient.UI
{
	internal class DataGridWrapper : DataGrid
	{
		private bool isNavigatingForward;

		public DataGridWrapper()
		{
			base.Navigate += new NavigateEventHandler(this.DataGridWrapper_Navigate);
		}

		protected void DataGridWrapper_Navigate(object sender, NavigateEventArgs ne)
		{
			this.isNavigatingForward = ne.Forward;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			try
			{
				base.OnPaint(pe);
				this.isNavigatingForward = false;
			}
			catch (IndexOutOfRangeException indexOutOfRangeException)
			{
				if (!this.isNavigatingForward)
				{
					throw;
				}
				else
				{
					base.NavigateBack();
					this.isNavigatingForward = false;
					RtlAwareMessageBox.Show(StringResources.FailedToNavigateForward, StringResources.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, 0);
				}
			}
		}

		public override bool PreProcessMessage(ref Message msg)
		{
			bool flag;
			try
			{
				flag = base.PreProcessMessage(ref msg);
			}
			catch (ArgumentOutOfRangeException argumentOutOfRangeException)
			{
				flag = false;
			}
			catch (IndexOutOfRangeException indexOutOfRangeException)
			{
				flag = false;
			}
			return flag;
		}
	}
}