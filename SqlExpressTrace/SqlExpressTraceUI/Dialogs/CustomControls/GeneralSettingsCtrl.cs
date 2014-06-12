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
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SqlTraceExpressUI.CustomControls
{
	public partial class GeneralSettingsCtrl : UserControl
	{
		private bool _allowEditingName = true;

		public GeneralSettingsCtrl()
		{
			InitializeComponent();
		}

		public bool AllowEditingName
		{
			get { return _allowEditingName; }

			set
			{
				_allowEditingName = value;
				this.nameTextBox.Enabled = _allowEditingName;
			}
		}

		public string DefinitionName
		{
			get { return this.nameTextBox.Text.Trim(); }

			set
			{
				this.nameTextBox.Text = value;
				this.nameTextBox.SelectAll();
			}
		}

		public int DurationHours
		{
			get { return (int)this.durationHoursCtrl.Value; }
			set { this.durationHoursCtrl.Value = value; }
		}

		public int DurationMinutes
		{
			get { return (int)this.durationMinutesCtrl.Value; }
			set { this.durationMinutesCtrl.Value = value; }
		}

		public bool IntegratedConnect
		{
			get { return this.integratedConnectOption.Checked; }

			set
			{
				this.integratedConnectOption.Checked = value;
				this.sqlNamePwdOption.Checked = !value;
			}
		}

		public int RecordLimit
		{
			get
			{
				if (this.limitRowsCheckBox.Enabled) return (int)this.rowLimitCtrl.Value;
				else return 0;
			}

			set
			{
				if (value > 0)
				{
					this.limitRowsCheckBox.Checked = true;
					this.rowLimitCtrl.Value = value;
				}
				else
				{
					this.limitRowsCheckBox.Checked = false;
				}

				this.rowLimitCtrl.Enabled = this.limitRowsCheckBox.Checked;
			}
		}

		public string ServerName
		{
			get { return this.serverTextbox.Text.Trim(); }

			set
			{
				this.serverTextbox.Text = value;
			}
		}

		public string UserId
		{
			get { return this.userIdTextbox.Text.Trim(); }
			set { this.userIdTextbox.Text = value; }
		}

		public void FocusServerName()
		{
			this.serverTextbox.Focus();
			this.serverTextbox.SelectAll();
		}

		public void FocusTraceDefinitionName()
		{
			this.nameTextBox.Focus();
			this.nameTextBox.SelectAll();
		}

		public void FocusUserId()
		{
			this.userIdTextbox.Focus();
			this.userIdTextbox.SelectAll();
		}

		private void onControlLoad(object sender, EventArgs e)
		{

		}

		private void onDurationCtrlValidating(object sender, CancelEventArgs e)
		{
			if (this.durationHoursCtrl.Value == 0)
			{
				this.durationMinutesCtrl.Value = (this.durationMinutesCtrl.Value > 0) ?
					this.durationMinutesCtrl.Value : 1;
			}
			else if (this.durationHoursCtrl.Value == 72)
			{
				this.durationMinutesCtrl.Value = 0;
			}
		}

		private void onIntegratedConnectOptionChanged(object sender, EventArgs e)
		{
			setControlStatesBasedOnEntries();
		}

		private void onLimitCheckChanged(object sender, EventArgs e)
		{
			this.rowLimitCtrl.Enabled = this.limitRowsCheckBox.Checked;
		}

		private void onServerTextChanged(object sender, EventArgs e)
		{
			setControlStatesBasedOnEntries();
		}

		private void onTestConnectButtonClick(object sender, EventArgs e)
		{
			string connectString = string.Empty;

			if (!this.IntegratedConnect)
			{
				string prompt = string.Format("Enter password to connect to {0} as {1}.",
					this.ServerName, this.UserId);
				string password = string.Empty;

				if (InputBox.ShowDialog(this.ParentForm, "Enter password.", prompt, ref password,
				usePasswordChars: true) == DialogResult.Cancel) return;

				connectString = string.Format("Server={0}; Database=master; User Id={1}; Password={2}",
					this.ServerName, this.UserId, password);
			}
			else connectString = string.Format("Server={0}; Database=master; Trusted_Connection=True;", this.ServerName);

			try
			{
				using (SqlConnection connection = new SqlConnection(connectString))
				{
					connection.Open();
				}
				string caption = "Success!";
				string msg = string.Format("The attempt to connect to {0} was successful.", this.ServerName);
				MessageBox.Show(this, msg, caption);
			}
			catch (SqlException ex)
			{
				string caption = "Test Failed!";
				string msg = string.Format("The attempt to connect to {0} because:\r\n\"{1}\"",
					this.ServerName, ex.Message);
				MessageBox.Show(this, msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void onUserIdTextboxTextChanged(object sender, EventArgs e)
		{
			setControlStatesBasedOnEntries();
		}

		private void setControlStatesBasedOnEntries()
		{
			this.testConnectButton.Enabled = ((this.ServerName.Length > 0) &&
				(this.IntegratedConnect || (this.UserId.Length > 0)));

			this.loginPanel.Enabled = !this.IntegratedConnect;
		}
	}
}