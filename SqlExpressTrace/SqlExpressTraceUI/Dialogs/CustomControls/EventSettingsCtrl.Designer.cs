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
namespace SqlExpressTraceUI.CustomControls
{
	partial class EventSettingsCtrl
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
			this.components = new System.ComponentModel.Container();
			this.columnListBox = new System.Windows.Forms.CheckedListBox();
			this.eventsListBox = new System.Windows.Forms.CheckedListBox();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.nextEventLbl = new System.Windows.Forms.LinkLabel();
			this.previousEventLbl = new System.Windows.Forms.LinkLabel();
			this.previousColumnLbl = new System.Windows.Forms.LinkLabel();
			this.nextColumnLbl = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// columnListBox
			// 
			this.columnListBox.Font = new System.Drawing.Font("Tahoma", 9F);
			this.columnListBox.ForeColor = System.Drawing.SystemColors.ControlText;
			this.columnListBox.FormattingEnabled = true;
			this.columnListBox.Location = new System.Drawing.Point(312, 40);
			this.columnListBox.Name = "columnListBox";
			this.columnListBox.Size = new System.Drawing.Size(168, 157);
			this.columnListBox.Sorted = true;
			this.columnListBox.TabIndex = 27;
			this.columnListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.onColumnListBoxItemCheck);
			this.columnListBox.SelectedIndexChanged += new System.EventHandler(this.onColumnListBoxSelectionChanged);
			this.columnListBox.Enter += new System.EventHandler(this.onColumnListBoxEnter);
			// 
			// eventsListBox
			// 
			this.eventsListBox.Font = new System.Drawing.Font("Tahoma", 9F);
			this.eventsListBox.ForeColor = System.Drawing.SystemColors.ControlText;
			this.eventsListBox.FormattingEnabled = true;
			this.eventsListBox.Location = new System.Drawing.Point(24, 40);
			this.eventsListBox.Name = "eventsListBox";
			this.eventsListBox.Size = new System.Drawing.Size(248, 157);
			this.eventsListBox.Sorted = true;
			this.eventsListBox.TabIndex = 29;
			this.eventsListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.onEventsListBoxItemCheck);
			this.eventsListBox.SelectedIndexChanged += new System.EventHandler(this.onEventsListBoxSelectionChanged);
			this.eventsListBox.Enter += new System.EventHandler(this.onEventsListBoxEnter);
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.descriptionLabel.Font = new System.Drawing.Font("Tahoma", 9F);
			this.descriptionLabel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.descriptionLabel.Location = new System.Drawing.Point(24, 208);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(456, 72);
			this.descriptionLabel.TabIndex = 31;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Tahoma", 9F);
			this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label7.Location = new System.Drawing.Point(24, 20);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(176, 20);
			this.label7.TabIndex = 30;
			this.label7.Text = "Select events to trace";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Tahoma", 9F);
			this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label6.Location = new System.Drawing.Point(312, 8);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(160, 32);
			this.label6.TabIndex = 28;
			this.label6.Text = "Columns to display for the selected event";
			// 
			// nextEventLbl
			// 
			this.nextEventLbl.AutoSize = true;
			this.nextEventLbl.Font = new System.Drawing.Font("Wingdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.nextEventLbl.ForeColor = System.Drawing.Color.DarkBlue;
			this.nextEventLbl.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.nextEventLbl.Location = new System.Drawing.Point(272, 164);
			this.nextEventLbl.Name = "nextEventLbl";
			this.nextEventLbl.Size = new System.Drawing.Size(21, 16);
			this.nextEventLbl.TabIndex = 36;
			this.nextEventLbl.TabStop = true;
			this.nextEventLbl.Text = "ê";
			this.toolTip1.SetToolTip(this.nextEventLbl, "Next event selection");
			this.nextEventLbl.VisitedLinkColor = System.Drawing.Color.DarkBlue;
			this.nextEventLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.onNextEventLabelClick);
			// 
			// previousEventLbl
			// 
			this.previousEventLbl.AutoSize = true;
			this.previousEventLbl.Font = new System.Drawing.Font("Wingdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.previousEventLbl.ForeColor = System.Drawing.Color.DarkBlue;
			this.previousEventLbl.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.previousEventLbl.Location = new System.Drawing.Point(272, 52);
			this.previousEventLbl.Name = "previousEventLbl";
			this.previousEventLbl.Size = new System.Drawing.Size(21, 16);
			this.previousEventLbl.TabIndex = 37;
			this.previousEventLbl.TabStop = true;
			this.previousEventLbl.Text = "é";
			this.toolTip1.SetToolTip(this.previousEventLbl, "Previous event selection");
			this.previousEventLbl.VisitedLinkColor = System.Drawing.Color.DarkBlue;
			this.previousEventLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.onPrevEventLabelClick);
			// 
			// previousColumnLbl
			// 
			this.previousColumnLbl.AutoSize = true;
			this.previousColumnLbl.Font = new System.Drawing.Font("Wingdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.previousColumnLbl.ForeColor = System.Drawing.Color.DarkBlue;
			this.previousColumnLbl.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.previousColumnLbl.Location = new System.Drawing.Point(480, 52);
			this.previousColumnLbl.Name = "previousColumnLbl";
			this.previousColumnLbl.Size = new System.Drawing.Size(21, 16);
			this.previousColumnLbl.TabIndex = 38;
			this.previousColumnLbl.TabStop = true;
			this.previousColumnLbl.Text = "é";
			this.toolTip1.SetToolTip(this.previousColumnLbl, "Previous column selection");
			this.previousColumnLbl.VisitedLinkColor = System.Drawing.Color.DarkBlue;
			this.previousColumnLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.onPrevColumnLabelClick);
			// 
			// nextColumnLbl
			// 
			this.nextColumnLbl.AutoSize = true;
			this.nextColumnLbl.Font = new System.Drawing.Font("Wingdings", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
			this.nextColumnLbl.ForeColor = System.Drawing.Color.DarkBlue;
			this.nextColumnLbl.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.nextColumnLbl.Location = new System.Drawing.Point(480, 164);
			this.nextColumnLbl.Name = "nextColumnLbl";
			this.nextColumnLbl.Size = new System.Drawing.Size(21, 16);
			this.nextColumnLbl.TabIndex = 39;
			this.nextColumnLbl.TabStop = true;
			this.nextColumnLbl.Text = "ê";
			this.toolTip1.SetToolTip(this.nextColumnLbl, "Next column selection");
			this.nextColumnLbl.VisitedLinkColor = System.Drawing.Color.DarkBlue;
			this.nextColumnLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.onNextColumnLabelClick);
			// 
			// EventSettingsCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.nextColumnLbl);
			this.Controls.Add(this.previousColumnLbl);
			this.Controls.Add(this.previousEventLbl);
			this.Controls.Add(this.nextEventLbl);
			this.Controls.Add(this.columnListBox);
			this.Controls.Add(this.eventsListBox);
			this.Controls.Add(this.descriptionLabel);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ForeColor = System.Drawing.SystemColors.ControlText;
			this.Name = "EventSettingsCtrl";
			this.Size = new System.Drawing.Size(528, 307);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox columnListBox;
		private System.Windows.Forms.CheckedListBox eventsListBox;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.LinkLabel nextEventLbl;
		private System.Windows.Forms.LinkLabel previousEventLbl;
		private System.Windows.Forms.LinkLabel previousColumnLbl;
		private System.Windows.Forms.LinkLabel nextColumnLbl;
	}
}
