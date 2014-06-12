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
	partial class TraceEventDisplay
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TraceEventDisplay));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.displayOptionsPanel = new System.Windows.Forms.Panel();
			this.saveButton = new System.Windows.Forms.Button();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.clearButton = new System.Windows.Forms.Button();
			this.fontButton = new System.Windows.Forms.Button();
			this.autoScrollOption = new System.Windows.Forms.CheckBox();
			this.groupColumnsOption = new System.Windows.Forms.CheckBox();
			this.freezeColumnOption = new System.Windows.Forms.CheckBox();
			this.textDataTextBox = new System.Windows.Forms.TextBox();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.traceDisplayGrid = new SqlExpressTraceUI.CustomControls.CustomDataGridView();
			this.displayOptionsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.traceDisplayGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// displayOptionsPanel
			// 
			this.displayOptionsPanel.BackColor = System.Drawing.Color.Transparent;
			this.displayOptionsPanel.Controls.Add(this.saveButton);
			this.displayOptionsPanel.Controls.Add(this.clearButton);
			this.displayOptionsPanel.Controls.Add(this.fontButton);
			this.displayOptionsPanel.Controls.Add(this.autoScrollOption);
			this.displayOptionsPanel.Controls.Add(this.groupColumnsOption);
			this.displayOptionsPanel.Controls.Add(this.freezeColumnOption);
			this.displayOptionsPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.displayOptionsPanel.Location = new System.Drawing.Point(0, 0);
			this.displayOptionsPanel.Name = "displayOptionsPanel";
			this.displayOptionsPanel.Size = new System.Drawing.Size(601, 40);
			this.displayOptionsPanel.TabIndex = 1;
			// 
			// saveButton
			// 
			this.saveButton.ImageKey = "save.bmp";
			this.saveButton.ImageList = this.imageList;
			this.saveButton.Location = new System.Drawing.Point(8, 8);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(26, 26);
			this.saveButton.TabIndex = 36;
			this.toolTip.SetToolTip(this.saveButton, "Save the trace records to a csv file");
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.onSaveButtonClick);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.White;
			this.imageList.Images.SetKeyName(0, "eraser.bmp");
			this.imageList.Images.SetKeyName(1, "save.bmp");
			// 
			// clearButton
			// 
			this.clearButton.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.clearButton.ForeColor = System.Drawing.Color.DarkBlue;
			this.clearButton.ImageKey = "eraser.bmp";
			this.clearButton.ImageList = this.imageList;
			this.clearButton.Location = new System.Drawing.Point(40, 8);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(26, 26);
			this.clearButton.TabIndex = 35;
			this.toolTip.SetToolTip(this.clearButton, "Clear rows from the trace display grid");
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler(this.onClearButtonClick);
			// 
			// fontButton
			// 
			this.fontButton.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fontButton.ForeColor = System.Drawing.Color.DarkBlue;
			this.fontButton.Location = new System.Drawing.Point(72, 8);
			this.fontButton.Name = "fontButton";
			this.fontButton.Size = new System.Drawing.Size(26, 26);
			this.fontButton.TabIndex = 34;
			this.fontButton.Text = "F";
			this.toolTip.SetToolTip(this.fontButton, "Change the trace display grid font");
			this.fontButton.UseVisualStyleBackColor = true;
			this.fontButton.Click += new System.EventHandler(this.onFontButtonClick);
			// 
			// autoScrollOption
			// 
			this.autoScrollOption.AutoSize = true;
			this.autoScrollOption.Location = new System.Drawing.Point(376, 16);
			this.autoScrollOption.Name = "autoScrollOption";
			this.autoScrollOption.Size = new System.Drawing.Size(74, 17);
			this.autoScrollOption.TabIndex = 33;
			this.autoScrollOption.Text = "AutoScroll";
			this.autoScrollOption.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.toolTip.SetToolTip(this.autoScrollOption, "Select to keep the most recent rows added in view");
			this.autoScrollOption.UseVisualStyleBackColor = true;
			// 
			// groupColumnsOption
			// 
			this.groupColumnsOption.AutoSize = true;
			this.groupColumnsOption.Location = new System.Drawing.Point(236, 16);
			this.groupColumnsOption.Name = "groupColumnsOption";
			this.groupColumnsOption.Size = new System.Drawing.Size(129, 17);
			this.groupColumnsOption.TabIndex = 29;
			this.groupColumnsOption.Text = "Group sorted columns";
			this.groupColumnsOption.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.toolTip.SetToolTip(this.groupColumnsOption, "Select to group row data having the value in the column chosen for sorting");
			this.groupColumnsOption.UseVisualStyleBackColor = true;
			this.groupColumnsOption.CheckStateChanged += new System.EventHandler(this.onGroupColumnsOptionChanged);
			// 
			// freezeColumnOption
			// 
			this.freezeColumnOption.AutoSize = true;
			this.freezeColumnOption.Location = new System.Drawing.Point(112, 16);
			this.freezeColumnOption.Name = "freezeColumnOption";
			this.freezeColumnOption.Size = new System.Drawing.Size(114, 17);
			this.freezeColumnOption.TabIndex = 31;
			this.freezeColumnOption.Text = "Freeze first column";
			this.freezeColumnOption.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.toolTip.SetToolTip(this.freezeColumnOption, "Select to keep left-most column locked in place while scrolling horizontally.");
			this.freezeColumnOption.UseVisualStyleBackColor = true;
			this.freezeColumnOption.CheckedChanged += new System.EventHandler(this.onFreezeColumnOptionChange);
			// 
			// textDataTextBox
			// 
			this.textDataTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
			this.textDataTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.textDataTextBox.Location = new System.Drawing.Point(0, 236);
			this.textDataTextBox.Multiline = true;
			this.textDataTextBox.Name = "textDataTextBox";
			this.textDataTextBox.ReadOnly = true;
			this.textDataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textDataTextBox.Size = new System.Drawing.Size(601, 74);
			this.textDataTextBox.TabIndex = 30;
			// 
			// traceDisplayGrid
			// 
			this.traceDisplayGrid.AllowUserToAddRows = false;
			this.traceDisplayGrid.AllowUserToDeleteRows = false;
			this.traceDisplayGrid.AllowUserToOrderColumns = true;
			this.traceDisplayGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.traceDisplayGrid.DisplayRowNumbers = true;
			this.traceDisplayGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.traceDisplayGrid.Font = new System.Drawing.Font("Consolas", 9F);
			this.traceDisplayGrid.GroupSortedColumnValues = false;
			this.traceDisplayGrid.Location = new System.Drawing.Point(0, 40);
			this.traceDisplayGrid.Name = "traceDisplayGrid";
			this.traceDisplayGrid.ReadOnly = true;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9F);
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.traceDisplayGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
			this.traceDisplayGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.traceDisplayGrid.ShowCellErrors = false;
			this.traceDisplayGrid.ShowRowErrors = false;
			this.traceDisplayGrid.Size = new System.Drawing.Size(601, 196);
			this.traceDisplayGrid.TabIndex = 0;
			this.traceDisplayGrid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.onCellEnter);
			this.traceDisplayGrid.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.onColumnDisplayIndexChanged);
			// 
			// TraceEventDisplay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightGray;
			this.Controls.Add(this.traceDisplayGrid);
			this.Controls.Add(this.textDataTextBox);
			this.Controls.Add(this.displayOptionsPanel);
			this.Name = "TraceEventDisplay";
			this.Size = new System.Drawing.Size(601, 310);
			this.displayOptionsPanel.ResumeLayout(false);
			this.displayOptionsPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.traceDisplayGrid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Button fontButton;
		private System.Windows.Forms.Button clearButton;

		#endregion

		private SqlExpressTraceUI.CustomControls.CustomDataGridView traceDisplayGrid;
		private System.Windows.Forms.Panel displayOptionsPanel;
		private System.Windows.Forms.CheckBox groupColumnsOption;
		private System.Windows.Forms.CheckBox freezeColumnOption;
		private System.Windows.Forms.TextBox textDataTextBox;
		private System.Windows.Forms.CheckBox autoScrollOption;
		private System.Windows.Forms.Button saveButton;
	}
}
