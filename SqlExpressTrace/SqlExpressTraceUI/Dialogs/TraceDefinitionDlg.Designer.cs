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
namespace SqlTraceExpressUI
{
	partial class TraceDefinitionDlg
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.generalSettingsTab = new System.Windows.Forms.TabPage();
			this.generalSettingsCtrl = new SqlTraceExpressUI.CustomControls.GeneralSettingsCtrl();
			this.eventsTab = new System.Windows.Forms.TabPage();
			this.eventSettingsCtrl = new SqlExpressTraceUI.CustomControls.EventSettingsCtrl();
			this.filtersTab = new System.Windows.Forms.TabPage();
			this.filterSettingsCtrl = new SqlExpressTraceUI.CustomControls.FilterSettingsCtrl();
			this.tabControl1.SuspendLayout();
			this.generalSettingsTab.SuspendLayout();
			this.eventsTab.SuspendLayout();
			this.filtersTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Font = new System.Drawing.Font("Tahoma", 9F);
			this.okButton.Location = new System.Drawing.Point(408, 360);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(71, 22);
			this.okButton.TabIndex = 19;
			this.okButton.Text = "Ok";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.onOkButtonClick);
			// 
			// cancelButton
			// 
			this.cancelButton.Font = new System.Drawing.Font("Tahoma", 9F);
			this.cancelButton.Location = new System.Drawing.Point(488, 360);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(71, 22);
			this.cancelButton.TabIndex = 20;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.onCancelButtonClick);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.generalSettingsTab);
			this.tabControl1.Controls.Add(this.eventsTab);
			this.tabControl1.Controls.Add(this.filtersTab);
			this.tabControl1.Location = new System.Drawing.Point(24, 16);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(536, 336);
			this.tabControl1.TabIndex = 31;
			// 
			// generalSettingsTab
			// 
			this.generalSettingsTab.Controls.Add(this.generalSettingsCtrl);
			this.generalSettingsTab.Location = new System.Drawing.Point(4, 23);
			this.generalSettingsTab.Name = "generalSettingsTab";
			this.generalSettingsTab.Padding = new System.Windows.Forms.Padding(3);
			this.generalSettingsTab.Size = new System.Drawing.Size(528, 309);
			this.generalSettingsTab.TabIndex = 0;
			this.generalSettingsTab.Text = "General";
			// 
			// generalSettingsCtrl
			// 
			this.generalSettingsCtrl.AllowEditingName = true;
			this.generalSettingsCtrl.BackColor = System.Drawing.SystemColors.Control;
			this.generalSettingsCtrl.DefinitionName = "New Trace Definition";
			this.generalSettingsCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.generalSettingsCtrl.DurationHours = 0;
			this.generalSettingsCtrl.DurationMinutes = 5;
			this.generalSettingsCtrl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.generalSettingsCtrl.IntegratedConnect = true;
			this.generalSettingsCtrl.Location = new System.Drawing.Point(3, 3);
			this.generalSettingsCtrl.Name = "generalSettingsCtrl";
			this.generalSettingsCtrl.RecordLimit = 1000;
			this.generalSettingsCtrl.ServerName = "";
			this.generalSettingsCtrl.Size = new System.Drawing.Size(522, 303);
			this.generalSettingsCtrl.TabIndex = 0;
			this.generalSettingsCtrl.UserId = "";
			// 
			// eventsTab
			// 
			this.eventsTab.Controls.Add(this.eventSettingsCtrl);
			this.eventsTab.Location = new System.Drawing.Point(4, 23);
			this.eventsTab.Name = "eventsTab";
			this.eventsTab.Padding = new System.Windows.Forms.Padding(3);
			this.eventsTab.Size = new System.Drawing.Size(528, 309);
			this.eventsTab.TabIndex = 1;
			this.eventsTab.Text = "Events";
			// 
			// eventSettingsCtrl
			// 
			this.eventSettingsCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.eventSettingsCtrl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.eventSettingsCtrl.Location = new System.Drawing.Point(3, 3);
			this.eventSettingsCtrl.Name = "eventSettingsCtrl";
			this.eventSettingsCtrl.Size = new System.Drawing.Size(522, 303);
			this.eventSettingsCtrl.TabIndex = 0;
			// 
			// filtersTab
			// 
			this.filtersTab.Controls.Add(this.filterSettingsCtrl);
			this.filtersTab.Location = new System.Drawing.Point(4, 23);
			this.filtersTab.Name = "filtersTab";
			this.filtersTab.Padding = new System.Windows.Forms.Padding(3);
			this.filtersTab.Size = new System.Drawing.Size(528, 309);
			this.filtersTab.TabIndex = 2;
			this.filtersTab.Text = "Trace Filters";
			// 
			// filterSettingsCtrl
			// 
			this.filterSettingsCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.filterSettingsCtrl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.filterSettingsCtrl.Location = new System.Drawing.Point(3, 3);
			this.filterSettingsCtrl.Name = "filterSettingsCtrl";
			this.filterSettingsCtrl.Size = new System.Drawing.Size(522, 303);
			this.filterSettingsCtrl.TabIndex = 0;
			// 
			// TraceDefinitionDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(585, 393);
			this.ControlBox = false;
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Font = new System.Drawing.Font("Tahoma", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TraceDefinitionDlg";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Trace Definition for";
			this.tabControl1.ResumeLayout(false);
			this.generalSettingsTab.ResumeLayout(false);
			this.eventsTab.ResumeLayout(false);
			this.filtersTab.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage generalSettingsTab;
		private System.Windows.Forms.TabPage eventsTab;
		private System.Windows.Forms.TabPage filtersTab;
		private SqlTraceExpressUI.CustomControls.GeneralSettingsCtrl generalSettingsCtrl;
		private SqlExpressTraceUI.CustomControls.EventSettingsCtrl eventSettingsCtrl;
		private SqlExpressTraceUI.CustomControls.FilterSettingsCtrl filterSettingsCtrl;
	}
}