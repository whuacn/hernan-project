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
	partial class FilterSettingsCtrl
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
			this.label3 = new System.Windows.Forms.Label();
			this.currentFiltersListBox = new System.Windows.Forms.ListBox();
			this.addButton = new System.Windows.Forms.Button();
			this.removeButton = new System.Windows.Forms.Button();
			this.filterColumnListBox = new System.Windows.Forms.ListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.optionOr = new System.Windows.Forms.RadioButton();
			this.optionAdd = new System.Windows.Forms.RadioButton();
			this.filterValueTextBox = new System.Windows.Forms.TextBox();
			this.operatorList = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 188);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(209, 14);
			this.label3.TabIndex = 40;
			this.label3.Text = "Current filters for this trace definition";
			// 
			// currentFiltersListBox
			// 
			this.currentFiltersListBox.FormattingEnabled = true;
			this.currentFiltersListBox.ItemHeight = 14;
			this.currentFiltersListBox.Location = new System.Drawing.Point(16, 204);
			this.currentFiltersListBox.Name = "currentFiltersListBox";
			this.currentFiltersListBox.Size = new System.Drawing.Size(400, 74);
			this.currentFiltersListBox.TabIndex = 39;
			this.currentFiltersListBox.SelectedIndexChanged += new System.EventHandler(this.onCurrentFiltersSelectChanged);
			// 
			// addButton
			// 
			this.addButton.Enabled = false;
			this.addButton.Location = new System.Drawing.Point(437, 68);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(72, 26);
			this.addButton.TabIndex = 35;
			this.addButton.Text = "Add";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.onAddFilterClick);
			// 
			// removeButton
			// 
			this.removeButton.Enabled = false;
			this.removeButton.Location = new System.Drawing.Point(437, 232);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(72, 26);
			this.removeButton.TabIndex = 38;
			this.removeButton.Text = "Remove";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.onRemoveButtonClick);
			// 
			// filterColumnListBox
			// 
			this.filterColumnListBox.FormattingEnabled = true;
			this.filterColumnListBox.ItemHeight = 14;
			this.filterColumnListBox.Location = new System.Drawing.Point(16, 44);
			this.filterColumnListBox.Name = "filterColumnListBox";
			this.filterColumnListBox.Size = new System.Drawing.Size(173, 116);
			this.filterColumnListBox.Sorted = true;
			this.filterColumnListBox.TabIndex = 37;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.optionOr);
			this.panel1.Controls.Add(this.optionAdd);
			this.panel1.Controls.Add(this.filterValueTextBox);
			this.panel1.Controls.Add(this.operatorList);
			this.panel1.Location = new System.Drawing.Point(197, 48);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(233, 116);
			this.panel1.TabIndex = 36;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 14);
			this.label1.TabIndex = 31;
			this.label1.Text = "Value:";
			// 
			// optionOr
			// 
			this.optionOr.AutoSize = true;
			this.optionOr.Location = new System.Drawing.Point(84, 9);
			this.optionOr.Name = "optionOr";
			this.optionOr.Size = new System.Drawing.Size(38, 18);
			this.optionOr.TabIndex = 30;
			this.optionOr.TabStop = true;
			this.optionOr.Text = "Or";
			this.optionOr.UseVisualStyleBackColor = true;
			// 
			// optionAdd
			// 
			this.optionAdd.AutoSize = true;
			this.optionAdd.Checked = true;
			this.optionAdd.Location = new System.Drawing.Point(19, 9);
			this.optionAdd.Name = "optionAdd";
			this.optionAdd.Size = new System.Drawing.Size(47, 18);
			this.optionAdd.TabIndex = 29;
			this.optionAdd.TabStop = true;
			this.optionAdd.Text = "And";
			this.optionAdd.UseVisualStyleBackColor = true;
			// 
			// filterValueTextBox
			// 
			this.filterValueTextBox.Font = new System.Drawing.Font("Tahoma", 9F);
			this.filterValueTextBox.ForeColor = System.Drawing.SystemColors.ControlText;
			this.filterValueTextBox.Location = new System.Drawing.Point(16, 88);
			this.filterValueTextBox.Name = "filterValueTextBox";
			this.filterValueTextBox.Size = new System.Drawing.Size(205, 22);
			this.filterValueTextBox.TabIndex = 25;
			this.filterValueTextBox.TextChanged += new System.EventHandler(this.onValueChanged);
			// 
			// operatorList
			// 
			this.operatorList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.operatorList.Font = new System.Drawing.Font("Tahoma", 9F);
			this.operatorList.ForeColor = System.Drawing.SystemColors.ControlText;
			this.operatorList.FormattingEnabled = true;
			this.operatorList.Location = new System.Drawing.Point(19, 34);
			this.operatorList.Name = "operatorList";
			this.operatorList.Size = new System.Drawing.Size(158, 22);
			this.operatorList.TabIndex = 24;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(128, 14);
			this.label2.TabIndex = 41;
			this.label2.Text = "Select column to filter";
			// 
			// FilterSettingsCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.currentFiltersListBox);
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.removeButton);
			this.Controls.Add(this.filterColumnListBox);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "FilterSettingsCtrl";
			this.Size = new System.Drawing.Size(528, 309);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox currentFiltersListBox;
		private System.Windows.Forms.Button addButton;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.ListBox filterColumnListBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RadioButton optionOr;
		private System.Windows.Forms.RadioButton optionAdd;
		private System.Windows.Forms.TextBox filterValueTextBox;
		private System.Windows.Forms.ComboBox operatorList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}
