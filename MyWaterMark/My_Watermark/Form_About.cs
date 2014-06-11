using Microsoft.VisualBasic.CompilerServices;
using My_Watermark.My;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
namespace My_Watermark
{
	[DesignerGenerated]
	public class Form_About : Form
	{
		private IContainer components;
		[AccessedThroughProperty("Label_Title")]
		private Label _Label_Title;
		[AccessedThroughProperty("Label_C")]
		private Label _Label_C;
		[AccessedThroughProperty("Label_Icons")]
		private Label _Label_Icons;
		[AccessedThroughProperty("Label_Icons2")]
		private Label _Label_Icons2;
		internal virtual Label Label_Title
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Title;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_Title = value;
			}
		}
		internal virtual Label Label_C
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_C;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Label_C_Click);
				if (this._Label_C != null)
				{
					this._Label_C.Click -= value2;
				}
				this._Label_C = value;
				if (this._Label_C != null)
				{
					this._Label_C.Click += value2;
				}
			}
		}
		internal virtual Label Label_Icons
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Icons;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Label_Icons_Click);
				if (this._Label_Icons != null)
				{
					this._Label_Icons.Click -= value2;
				}
				this._Label_Icons = value;
				if (this._Label_Icons != null)
				{
					this._Label_Icons.Click += value2;
				}
			}
		}
		internal virtual Label Label_Icons2
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Icons2;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Label_Icons2_Click);
				if (this._Label_Icons2 != null)
				{
					this._Label_Icons2.Click -= value2;
				}
				this._Label_Icons2 = value;
				if (this._Label_Icons2 != null)
				{
					this._Label_Icons2.Click += value2;
				}
			}
		}
		[DebuggerNonUserCode]
		public Form_About()
		{
			base.Load += new EventHandler(this.Form_About_Load);
			this.InitializeComponent();
		}
		[DebuggerNonUserCode]
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this.components != null)
				{
					this.components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		[DebuggerStepThrough]
		private void InitializeComponent()
		{
			this.Label_Title = new Label();
			this.Label_C = new Label();
			this.Label_Icons = new Label();
			this.Label_Icons2 = new Label();
			this.SuspendLayout();
			this.Label_Title.BackColor = Color.Transparent;
			this.Label_Title.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.Label_Title.ForeColor = Color.Black;
			Control arg_80_0 = this.Label_Title;
			Point location = new Point(12, 7);
			arg_80_0.Location = location;
			this.Label_Title.Name = "Label_Title";
			Control arg_AA_0 = this.Label_Title;
			Size size = new Size(471, 22);
			arg_AA_0.Size = size;
			this.Label_Title.TabIndex = 6;
			this.Label_Title.Text = "Label_Title";
			this.Label_Title.TextAlign = ContentAlignment.MiddleCenter;
			this.Label_C.BackColor = Color.Transparent;
			this.Label_C.Cursor = Cursors.Hand;
			this.Label_C.ForeColor = Color.Black;
			Control arg_11A_0 = this.Label_C;
			location = new Point(12, 34);
			arg_11A_0.Location = location;
			this.Label_C.Name = "Label_C";
			Control arg_144_0 = this.Label_C;
			size = new Size(471, 18);
			arg_144_0.Size = size;
			this.Label_C.TabIndex = 7;
			this.Label_C.Text = "(C)";
			this.Label_C.TextAlign = ContentAlignment.MiddleCenter;
			this.Label_Icons.BackColor = Color.Transparent;
			this.Label_Icons.Cursor = Cursors.Hand;
			this.Label_Icons.ForeColor = Color.Black;
			Control arg_1B4_0 = this.Label_Icons;
			location = new Point(12, 76);
			arg_1B4_0.Location = location;
			this.Label_Icons.Name = "Label_Icons";
			Control arg_1DE_0 = this.Label_Icons;
			size = new Size(471, 18);
			arg_1DE_0.Size = size;
			this.Label_Icons.TabIndex = 8;
			this.Label_Icons.Text = "Label_Icons";
			this.Label_Icons.TextAlign = ContentAlignment.MiddleCenter;
			this.Label_Icons2.BackColor = Color.Transparent;
			this.Label_Icons2.Cursor = Cursors.Hand;
			this.Label_Icons2.ForeColor = Color.Black;
			Control arg_24E_0 = this.Label_Icons2;
			location = new Point(12, 100);
			arg_24E_0.Location = location;
			this.Label_Icons2.Name = "Label_Icons2";
			Control arg_278_0 = this.Label_Icons2;
			size = new Size(471, 18);
			arg_278_0.Size = size;
			this.Label_Icons2.TabIndex = 9;
			this.Label_Icons2.Text = "Label1";
			this.Label_Icons2.TextAlign = ContentAlignment.MiddleCenter;
			SizeF autoScaleDimensions = new SizeF(6f, 13f);
			this.AutoScaleDimensions = autoScaleDimensions;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.White;
			size = new Size(495, 126);
			this.ClientSize = size;
			this.Controls.Add(this.Label_Icons2);
			this.Controls.Add(this.Label_Icons);
			this.Controls.Add(this.Label_C);
			this.Controls.Add(this.Label_Title);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form_About";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "About";
			this.ResumeLayout(false);
		}
		private void Form_About_Load(object sender, EventArgs e)
		{
			this.Icon = MyProject.Forms.Form_Main.Icon;
			this.Text = " " + Module_Watermark.PRG_Name + " - About";
			this.Show();
			Application.DoEvents();
			this.WindowState = FormWindowState.Normal;
			this.TopMost = true;
			Application.DoEvents();
			this.TopMost = false;
			/*this.Label_Title.Text = Module_Watermark.PRG_Title;
			this.Label_C.Text = Module_MyFunctions.PRG_Copyright + " - " + Module_Internet.WEB_Site_URL;
			this.Label_Icons.Text = Module_Watermark.DAT_Icons;
			this.Label_Icons2.Text = Module_Watermark.DAT_Icons2;*/
		}
		private void Label_C_Click(object sender, EventArgs e)
		{
			//Process.Start(Module_Internet.WEB_Site_URL);
		}
		private void Label_Icons_Click(object sender, EventArgs e)
		{
			//Process.Start(Module_Watermark.DAT_Icons_URL);
		}
		private void Label_Icons2_Click(object sender, EventArgs e)
		{
			//Process.Start(Module_Watermark.DAT_Icons_URL2);
		}
	}
}
