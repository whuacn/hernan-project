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
	partial class MainDlg
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDlg));
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.traceDefSelectCtrl = new System.Windows.Forms.ComboBox();
			this.startButton = new System.Windows.Forms.Button();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.pauseButton = new System.Windows.Forms.Button();
			this.stopButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.editButton = new System.Windows.Forms.Button();
			this.deleteButton = new System.Windows.Forms.Button();
			this.closeButton = new System.Windows.Forms.Button();
			this.traceDefControlPanel = new System.Windows.Forms.Panel();
			this.aboutLabel = new System.Windows.Forms.LinkLabel();
			this.traceDefEditPanel = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.statusBarPanel = new System.Windows.Forms.Panel();
			this.statusLabel = new System.Windows.Forms.Label();
			this.traceEventDisplay = new SqlExpressTraceUI.CustomControls.TraceEventDisplay();
			this.traceDefControlPanel.SuspendLayout();
			this.traceDefEditPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.statusBarPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolTip
			// 
			this.toolTip.AutomaticDelay = 750;
			this.toolTip.AutoPopDelay = 5000;
			this.toolTip.ForeColor = System.Drawing.Color.CadetBlue;
			this.toolTip.InitialDelay = 750;
			this.toolTip.ReshowDelay = 150;
			// 
			// traceDefSelectCtrl
			// 
			this.traceDefSelectCtrl.BackColor = System.Drawing.SystemColors.Window;
			this.traceDefSelectCtrl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.traceDefSelectCtrl.FormattingEnabled = true;
			this.traceDefSelectCtrl.Location = new System.Drawing.Point(4, 4);
			this.traceDefSelectCtrl.Name = "traceDefSelectCtrl";
			this.traceDefSelectCtrl.Size = new System.Drawing.Size(212, 22);
			this.traceDefSelectCtrl.Sorted = true;
			this.traceDefSelectCtrl.TabIndex = 39;
			this.toolTip.SetToolTip(this.traceDefSelectCtrl, "Select a trace definition from the current list");
			this.traceDefSelectCtrl.SelectedIndexChanged += new System.EventHandler(this.onDefinitionSelectionChanged);
			// 
			// startButton
			// 
			this.startButton.ImageKey = "start.bmp";
			this.startButton.ImageList = this.imageList;
			this.startButton.Location = new System.Drawing.Point(8, 4);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(26, 26);
			this.startButton.TabIndex = 41;
			this.toolTip.SetToolTip(this.startButton, "Start tracing using the selected trace definition");
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.onStartButtonClick);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.White;
			this.imageList.Images.SetKeyName(0, "pause.bmp");
			this.imageList.Images.SetKeyName(1, "delete.bmp");
			this.imageList.Images.SetKeyName(2, "stop.bmp");
			this.imageList.Images.SetKeyName(3, "add.bmp");
			this.imageList.Images.SetKeyName(4, "start.bmp");
			this.imageList.Images.SetKeyName(5, "edit.bmp");
			// 
			// pauseButton
			// 
			this.pauseButton.ImageKey = "pause.bmp";
			this.pauseButton.ImageList = this.imageList;
			this.pauseButton.Location = new System.Drawing.Point(40, 4);
			this.pauseButton.Name = "pauseButton";
			this.pauseButton.Size = new System.Drawing.Size(26, 26);
			this.pauseButton.TabIndex = 40;
			this.toolTip.SetToolTip(this.pauseButton, "Pause (Stop) tracing");
			this.pauseButton.UseVisualStyleBackColor = true;
			this.pauseButton.Click += new System.EventHandler(this.onPauseButtonClick);
			// 
			// stopButton
			// 
			this.stopButton.ImageKey = "stop.bmp";
			this.stopButton.ImageList = this.imageList;
			this.stopButton.Location = new System.Drawing.Point(72, 4);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(26, 26);
			this.stopButton.TabIndex = 39;
			this.toolTip.SetToolTip(this.stopButton, "Stop and close tracing");
			this.stopButton.UseVisualStyleBackColor = true;
			this.stopButton.Click += new System.EventHandler(this.onStopButtonClick);
			// 
			// addButton
			// 
			this.addButton.ImageKey = "add.bmp";
			this.addButton.ImageList = this.imageList;
			this.addButton.Location = new System.Drawing.Point(228, 4);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(26, 26);
			this.addButton.TabIndex = 44;
			this.toolTip.SetToolTip(this.addButton, "Add a new trace definition");
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.EnabledChanged += new System.EventHandler(this.onButtonEnableChanged);
			this.addButton.Click += new System.EventHandler(this.onAddButtonClick);
			// 
			// editButton
			// 
			this.editButton.ImageKey = "edit.bmp";
			this.editButton.ImageList = this.imageList;
			this.editButton.Location = new System.Drawing.Point(260, 4);
			this.editButton.Name = "editButton";
			this.editButton.Size = new System.Drawing.Size(26, 26);
			this.editButton.TabIndex = 43;
			this.toolTip.SetToolTip(this.editButton, "edit the selected trace definition");
			this.editButton.UseVisualStyleBackColor = true;
			this.editButton.Click += new System.EventHandler(this.onEditButtonClick);
			// 
			// deleteButton
			// 
			this.deleteButton.ImageKey = "delete.bmp";
			this.deleteButton.ImageList = this.imageList;
			this.deleteButton.Location = new System.Drawing.Point(292, 4);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(26, 26);
			this.deleteButton.TabIndex = 42;
			this.toolTip.SetToolTip(this.deleteButton, "Delete the selected trace definition");
			this.deleteButton.UseVisualStyleBackColor = true;
			this.deleteButton.Click += new System.EventHandler(this.onDeleteButtonClick);
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.Location = new System.Drawing.Point(596, 12);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(62, 26);
			this.closeButton.TabIndex = 12;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.onCloseClick);
			// 
			// traceDefControlPanel
			// 
			this.traceDefControlPanel.BackColor = System.Drawing.Color.LightGray;
			this.traceDefControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.traceDefControlPanel.Controls.Add(this.aboutLabel);
			this.traceDefControlPanel.Controls.Add(this.traceDefEditPanel);
			this.traceDefControlPanel.Controls.Add(this.panel1);
			this.traceDefControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.traceDefControlPanel.Location = new System.Drawing.Point(0, 0);
			this.traceDefControlPanel.Name = "traceDefControlPanel";
			this.traceDefControlPanel.Size = new System.Drawing.Size(667, 44);
			this.traceDefControlPanel.TabIndex = 26;
			// 
			// aboutLabel
			// 
			this.aboutLabel.ActiveLinkColor = System.Drawing.Color.LightBlue;
			this.aboutLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.aboutLabel.AutoSize = true;
			this.aboutLabel.ForeColor = System.Drawing.Color.DarkBlue;
			this.aboutLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.aboutLabel.LinkColor = System.Drawing.Color.DarkBlue;
			this.aboutLabel.Location = new System.Drawing.Point(608, 24);
			this.aboutLabel.Name = "aboutLabel";
			this.aboutLabel.Size = new System.Drawing.Size(53, 14);
			this.aboutLabel.TabIndex = 39;
			this.aboutLabel.TabStop = true;
			this.aboutLabel.Text = "About...";
			this.aboutLabel.VisitedLinkColor = System.Drawing.Color.DarkBlue;
			this.aboutLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.onAboutLabelClick);
			// 
			// traceDefEditPanel
			// 
			this.traceDefEditPanel.Controls.Add(this.addButton);
			this.traceDefEditPanel.Controls.Add(this.traceDefSelectCtrl);
			this.traceDefEditPanel.Controls.Add(this.editButton);
			this.traceDefEditPanel.Controls.Add(this.deleteButton);
			this.traceDefEditPanel.Location = new System.Drawing.Point(4, 4);
			this.traceDefEditPanel.Name = "traceDefEditPanel";
			this.traceDefEditPanel.Size = new System.Drawing.Size(328, 32);
			this.traceDefEditPanel.TabIndex = 38;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.startButton);
			this.panel1.Controls.Add(this.pauseButton);
			this.panel1.Controls.Add(this.stopButton);
			this.panel1.Location = new System.Drawing.Point(356, 4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(108, 32);
			this.panel1.TabIndex = 37;
			// 
			// statusBarPanel
			// 
			this.statusBarPanel.BackColor = System.Drawing.Color.LightGray;
			this.statusBarPanel.Controls.Add(this.statusLabel);
			this.statusBarPanel.Controls.Add(this.closeButton);
			this.statusBarPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.statusBarPanel.Location = new System.Drawing.Point(0, 323);
			this.statusBarPanel.Name = "statusBarPanel";
			this.statusBarPanel.Size = new System.Drawing.Size(667, 49);
			this.statusBarPanel.TabIndex = 27;
			// 
			// statusLabel
			// 
			this.statusLabel.AutoSize = true;
			this.statusLabel.Location = new System.Drawing.Point(20, 20);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(159, 14);
			this.statusLabel.TabIndex = 13;
			this.statusLabel.Text = "Status: No Trace is Running";
			// 
			// traceEventDisplay
			// 
			this.traceEventDisplay.BackColor = System.Drawing.Color.LightGray;
			this.traceEventDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.traceEventDisplay.DisplayGridFont = new System.Drawing.Font("Consolas", 9F);
			this.traceEventDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
			this.traceEventDisplay.Location = new System.Drawing.Point(0, 44);
			this.traceEventDisplay.Name = "traceEventDisplay";
			this.traceEventDisplay.Size = new System.Drawing.Size(667, 279);
			this.traceEventDisplay.Status = SqlExpressTraceLib.TraceStatus.Stopped;
			this.traceEventDisplay.TabIndex = 28;
			this.traceEventDisplay.ColumnOrderChanged += new SqlExpressTraceUI.CustomControls.ColumnOrderChangedEventHandler(this.onColumnOrderChanged);
			// 
			// MainDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightGray;
			this.ClientSize = new System.Drawing.Size(667, 372);
			this.Controls.Add(this.traceEventDisplay);
			this.Controls.Add(this.traceDefControlPanel);
			this.Controls.Add(this.statusBarPanel);
			this.Font = new System.Drawing.Font("Tahoma", 9F);
			this.MinimumSize = new System.Drawing.Size(600, 319);
			this.Name = "MainDlg";
			this.Text = "Sql ExpressTrace";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onFormClosing);
			this.Load += new System.EventHandler(this.onFormLoad);
			this.traceDefControlPanel.ResumeLayout(false);
			this.traceDefControlPanel.PerformLayout();
			this.traceDefEditPanel.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.statusBarPanel.ResumeLayout(false);
			this.statusBarPanel.PerformLayout();
			this.ResumeLayout(false);

		}
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button editButton;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button pauseButton;
		private System.Windows.Forms.Button startButton;

		#endregion

		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Panel traceDefControlPanel;
		private System.Windows.Forms.Panel statusBarPanel;
		private System.Windows.Forms.ComboBox traceDefSelectCtrl;
		private System.Windows.Forms.Panel traceDefEditPanel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.ImageList imageList;
		private SqlExpressTraceUI.CustomControls.TraceEventDisplay traceEventDisplay;
		private System.Windows.Forms.LinkLabel aboutLabel;
		private System.Windows.Forms.Label statusLabel;
	}
}

