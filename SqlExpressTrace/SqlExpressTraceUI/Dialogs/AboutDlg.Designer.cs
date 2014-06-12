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
namespace SqlExpressTraceUI
{
	partial class AboutDlg
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
			this.disclaimerLbl = new System.Windows.Forms.Label();
			this.link = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// disclaimerLbl
			// 
			this.disclaimerLbl.Dock = System.Windows.Forms.DockStyle.Top;
			this.disclaimerLbl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.disclaimerLbl.Location = new System.Drawing.Point(0, 0);
			this.disclaimerLbl.Name = "disclaimerLbl";
			this.disclaimerLbl.Size = new System.Drawing.Size(535, 220);
			this.disclaimerLbl.TabIndex = 0;
			this.disclaimerLbl.Text = "label1";
			// 
			// link
			// 
			this.link.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.link.AutoSize = true;
			this.link.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.link.Location = new System.Drawing.Point(140, 228);
			this.link.Name = "link";
			this.link.Size = new System.Drawing.Size(266, 14);
			this.link.TabIndex = 1;
			this.link.TabStop = true;
			this.link.Text = "http://www.codeproject.com/info/cpol10.aspx";
			this.link.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.onLinkClicked);
			// 
			// AboutDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(535, 248);
			this.Controls.Add(this.link);
			this.Controls.Add(this.disclaimerLbl);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About Sql ExpressTrace";
			this.Load += new System.EventHandler(this.onLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label disclaimerLbl;
		private System.Windows.Forms.LinkLabel link;

	}
}