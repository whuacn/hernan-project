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
namespace SqlTraceExpressUI.CustomControls
{
	partial class GeneralSettingsCtrl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.durationHoursCtrl = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.rowLimitCtrl = new System.Windows.Forms.NumericUpDown();
			this.limitRowsCheckBox = new System.Windows.Forms.CheckBox();
			this.loginPanel = new System.Windows.Forms.Panel();
			this.label4 = new System.Windows.Forms.Label();
			this.userIdTextbox = new System.Windows.Forms.TextBox();
			this.testConnectButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.serverTextbox = new System.Windows.Forms.TextBox();
			this.sqlNamePwdOption = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.integratedConnectOption = new System.Windows.Forms.RadioButton();
			this.nameTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.durationMinutesCtrl = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.durationHoursCtrl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.rowLimitCtrl)).BeginInit();
			this.loginPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.durationMinutesCtrl)).BeginInit();
			this.SuspendLayout();
			// 
			// durationHoursCtrl
			// 
			this.durationHoursCtrl.Location = new System.Drawing.Point(88, 224);
			this.durationHoursCtrl.Maximum = new decimal(new int[] {
            72,
            0,
            0,
            0});
			this.durationHoursCtrl.Name = "durationHoursCtrl";
			this.durationHoursCtrl.Size = new System.Drawing.Size(72, 22);
			this.durationHoursCtrl.TabIndex = 42;
			this.durationHoursCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.onDurationCtrlValidating);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(24, 224);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(57, 14);
			this.label8.TabIndex = 43;
			this.label8.Text = "Duration:";
			// 
			// rowLimitCtrl
			// 
			this.rowLimitCtrl.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.rowLimitCtrl.Location = new System.Drawing.Point(416, 224);
			this.rowLimitCtrl.Maximum = new decimal(new int[] {
            250000,
            0,
            0,
            0});
			this.rowLimitCtrl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.rowLimitCtrl.Name = "rowLimitCtrl";
			this.rowLimitCtrl.Size = new System.Drawing.Size(80, 22);
			this.rowLimitCtrl.TabIndex = 41;
			this.rowLimitCtrl.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// limitRowsCheckBox
			// 
			this.limitRowsCheckBox.AutoSize = true;
			this.limitRowsCheckBox.Location = new System.Drawing.Point(244, 224);
			this.limitRowsCheckBox.Name = "limitRowsCheckBox";
			this.limitRowsCheckBox.Size = new System.Drawing.Size(162, 18);
			this.limitRowsCheckBox.TabIndex = 40;
			this.limitRowsCheckBox.Text = "Limit number of rows to:";
			this.limitRowsCheckBox.UseVisualStyleBackColor = true;
			this.limitRowsCheckBox.CheckedChanged += new System.EventHandler(this.onLimitCheckChanged);
			// 
			// loginPanel
			// 
			this.loginPanel.Controls.Add(this.label4);
			this.loginPanel.Controls.Add(this.userIdTextbox);
			this.loginPanel.Enabled = false;
			this.loginPanel.Location = new System.Drawing.Point(82, 120);
			this.loginPanel.Name = "loginPanel";
			this.loginPanel.Size = new System.Drawing.Size(400, 48);
			this.loginPanel.TabIndex = 39;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(36, 12);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(51, 14);
			this.label4.TabIndex = 10;
			this.label4.Text = "User ID:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// userIdTextbox
			// 
			this.userIdTextbox.Location = new System.Drawing.Point(96, 10);
			this.userIdTextbox.MaxLength = 50;
			this.userIdTextbox.Name = "userIdTextbox";
			this.userIdTextbox.Size = new System.Drawing.Size(228, 22);
			this.userIdTextbox.TabIndex = 9;
			this.userIdTextbox.TextChanged += new System.EventHandler(this.onUserIdTextboxTextChanged);
			// 
			// testConnectButton
			// 
			this.testConnectButton.Location = new System.Drawing.Point(218, 176);
			this.testConnectButton.Name = "testConnectButton";
			this.testConnectButton.Size = new System.Drawing.Size(121, 24);
			this.testConnectButton.TabIndex = 36;
			this.testConnectButton.Text = "Test connection";
			this.testConnectButton.UseVisualStyleBackColor = true;
			this.testConnectButton.Click += new System.EventHandler(this.onTestConnectButtonClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Tahoma", 9F);
			this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label1.Location = new System.Drawing.Point(74, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 14);
			this.label1.TabIndex = 33;
			this.label1.Text = "Definition Name:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// serverTextbox
			// 
			this.serverTextbox.Font = new System.Drawing.Font("Tahoma", 9F);
			this.serverTextbox.ForeColor = System.Drawing.SystemColors.ControlText;
			this.serverTextbox.Location = new System.Drawing.Point(178, 56);
			this.serverTextbox.MaxLength = 50;
			this.serverTextbox.Name = "serverTextbox";
			this.serverTextbox.Size = new System.Drawing.Size(228, 22);
			this.serverTextbox.TabIndex = 34;
			this.serverTextbox.TextChanged += new System.EventHandler(this.onServerTextChanged);
			// 
			// sqlNamePwdOption
			// 
			this.sqlNamePwdOption.AutoSize = true;
			this.sqlNamePwdOption.Font = new System.Drawing.Font("Tahoma", 9F);
			this.sqlNamePwdOption.ForeColor = System.Drawing.SystemColors.ControlText;
			this.sqlNamePwdOption.Location = new System.Drawing.Point(298, 96);
			this.sqlNamePwdOption.Name = "sqlNamePwdOption";
			this.sqlNamePwdOption.Size = new System.Drawing.Size(197, 18);
			this.sqlNamePwdOption.TabIndex = 38;
			this.sqlNamePwdOption.Text = "Use SQL Server authentication.";
			this.sqlNamePwdOption.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Tahoma", 9F);
			this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label2.Location = new System.Drawing.Point(127, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(46, 14);
			this.label2.TabIndex = 35;
			this.label2.Text = "Server:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// integratedConnectOption
			// 
			this.integratedConnectOption.AutoSize = true;
			this.integratedConnectOption.Checked = true;
			this.integratedConnectOption.Font = new System.Drawing.Font("Tahoma", 9F);
			this.integratedConnectOption.ForeColor = System.Drawing.SystemColors.ControlText;
			this.integratedConnectOption.Location = new System.Drawing.Point(34, 96);
			this.integratedConnectOption.Name = "integratedConnectOption";
			this.integratedConnectOption.Size = new System.Drawing.Size(247, 18);
			this.integratedConnectOption.TabIndex = 37;
			this.integratedConnectOption.TabStop = true;
			this.integratedConnectOption.Text = "Use integrated Windows authentication.";
			this.integratedConnectOption.UseVisualStyleBackColor = true;
			this.integratedConnectOption.CheckedChanged += new System.EventHandler(this.onIntegratedConnectOptionChanged);
			// 
			// nameTextBox
			// 
			this.nameTextBox.Font = new System.Drawing.Font("Tahoma", 9F);
			this.nameTextBox.ForeColor = System.Drawing.SystemColors.ControlText;
			this.nameTextBox.Location = new System.Drawing.Point(180, 16);
			this.nameTextBox.MaxLength = 50;
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.Size = new System.Drawing.Size(228, 22);
			this.nameTextBox.TabIndex = 32;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(160, 224);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(37, 14);
			this.label3.TabIndex = 44;
			this.label3.Text = "hours";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(160, 248);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(50, 14);
			this.label5.TabIndex = 46;
			this.label5.Text = "minutes";
			// 
			// durationMinutesCtrl
			// 
			this.durationMinutesCtrl.Location = new System.Drawing.Point(88, 248);
			this.durationMinutesCtrl.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
			this.durationMinutesCtrl.Name = "durationMinutesCtrl";
			this.durationMinutesCtrl.Size = new System.Drawing.Size(72, 22);
			this.durationMinutesCtrl.TabIndex = 45;
			this.durationMinutesCtrl.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.durationMinutesCtrl.Validating += new System.ComponentModel.CancelEventHandler(this.onDurationCtrlValidating);
			// 
			// GeneralSettingsCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label5);
			this.Controls.Add(this.durationMinutesCtrl);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.durationHoursCtrl);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.rowLimitCtrl);
			this.Controls.Add(this.limitRowsCheckBox);
			this.Controls.Add(this.loginPanel);
			this.Controls.Add(this.testConnectButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.serverTextbox);
			this.Controls.Add(this.sqlNamePwdOption);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.integratedConnectOption);
			this.Controls.Add(this.nameTextBox);
			this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "GeneralSettingsCtrl";
			this.Size = new System.Drawing.Size(528, 309);
			this.Load += new System.EventHandler(this.onControlLoad);
			((System.ComponentModel.ISupportInitialize)(this.durationHoursCtrl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.rowLimitCtrl)).EndInit();
			this.loginPanel.ResumeLayout(false);
			this.loginPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.durationMinutesCtrl)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown durationHoursCtrl;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown rowLimitCtrl;
		private System.Windows.Forms.CheckBox limitRowsCheckBox;
		private System.Windows.Forms.Panel loginPanel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox userIdTextbox;
		private System.Windows.Forms.Button testConnectButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox serverTextbox;
		private System.Windows.Forms.RadioButton sqlNamePwdOption;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton integratedConnectOption;
		private System.Windows.Forms.TextBox nameTextBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown durationMinutesCtrl;

	}
}
