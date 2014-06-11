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
	public class Form_Bundle_Q : Form
	{
		private IContainer components;
		[AccessedThroughProperty("Label_Bundle")]
		private Label _Label_Bundle;
		[AccessedThroughProperty("Button_Continue")]
		private Button _Button_Continue;
		[AccessedThroughProperty("Label_Donate_2")]
		private Label _Label_Donate_2;
		[AccessedThroughProperty("Label1")]
		private Label _Label1;
		[AccessedThroughProperty("Button_Skip")]
		private Button _Button_Skip;
		private bool FRM_Button_Click;
		internal virtual Label Label_Bundle
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Bundle;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_Bundle = value;
			}
		}
		internal virtual Button Button_Continue
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Continue;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Continue_Click);
				if (this._Button_Continue != null)
				{
					this._Button_Continue.Click -= value2;
				}
				this._Button_Continue = value;
				if (this._Button_Continue != null)
				{
					this._Button_Continue.Click += value2;
				}
			}
		}
		internal virtual Label Label_Donate_2
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Donate_2;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_Donate_2 = value;
			}
		}
		internal virtual Label Label1
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label1;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label1 = value;
			}
		}
		internal virtual Button Button_Skip
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Skip;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Skip_Click);
				if (this._Button_Skip != null)
				{
					this._Button_Skip.Click -= value2;
				}
				this._Button_Skip = value;
				if (this._Button_Skip != null)
				{
					this._Button_Skip.Click += value2;
				}
			}
		}
		public Form_Bundle_Q()
		{
			base.FormClosing += new FormClosingEventHandler(this.Form_Bundle_Q_FormClosing);
			base.Load += new EventHandler(this.Form_Bundle_Q_Load);
			this.FRM_Button_Click = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form_Bundle_Q));
			this.Label_Bundle = new Label();
			this.Button_Continue = new Button();
			this.Label_Donate_2 = new Label();
			this.Label1 = new Label();
			this.Button_Skip = new Button();
			this.SuspendLayout();
			this.Label_Bundle.BackColor = Color.WhiteSmoke;
			this.Label_Bundle.Cursor = Cursors.Default;
			this.Label_Bundle.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_Bundle.ForeColor = Color.Black;
			Control arg_AC_0 = this.Label_Bundle;
			Point location = new Point(12, 9);
			arg_AC_0.Location = location;
			this.Label_Bundle.Name = "Label_Bundle";
			Control arg_D9_0 = this.Label_Bundle;
			Size size = new Size(386, 146);
			arg_D9_0.Size = size;
			this.Label_Bundle.TabIndex = 113;
			this.Label_Bundle.Text = componentResourceManager.GetString("Label_Bundle.Text");
			this.Button_Continue.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.Button_Continue.Cursor = Cursors.Hand;
			this.Button_Continue.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			Control arg_150_0 = this.Button_Continue;
			location = new Point(29, 156);
			arg_150_0.Location = location;
			this.Button_Continue.Name = "Button_Continue";
			Control arg_17A_0 = this.Button_Continue;
			size = new Size(169, 34);
			arg_17A_0.Size = size;
			this.Button_Continue.TabIndex = 114;
			this.Button_Continue.Text = "Download license";
			this.Button_Continue.UseVisualStyleBackColor = true;
			this.Label_Donate_2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Label_Donate_2.BackColor = Color.Gold;
			this.Label_Donate_2.BorderStyle = BorderStyle.FixedSingle;
			this.Label_Donate_2.Cursor = Cursors.Default;
			this.Label_Donate_2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.Label_Donate_2.ForeColor = Color.Black;
			Control arg_222_0 = this.Label_Donate_2;
			location = new Point(12, 196);
			arg_222_0.Location = location;
			this.Label_Donate_2.Name = "Label_Donate_2";
			Control arg_24C_0 = this.Label_Donate_2;
			size = new Size(383, 52);
			arg_24C_0.Size = size;
			this.Label_Donate_2.TabIndex = 116;
			this.Label_Donate_2.Text = "Note: you can also register this version\r\nby donating with PayPal.";
			this.Label_Donate_2.TextAlign = ContentAlignment.MiddleCenter;
			this.Label1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Label1.BackColor = Color.WhiteSmoke;
			this.Label1.BorderStyle = BorderStyle.FixedSingle;
			this.Label1.Cursor = Cursors.Default;
			this.Label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.Label1.ForeColor = Color.Black;
			Control arg_2F5_0 = this.Label1;
			location = new Point(-9, 253);
			arg_2F5_0.Location = location;
			this.Label1.Name = "Label1";
			Control arg_31F_0 = this.Label1;
			size = new Size(429, 23);
			arg_31F_0.Size = size;
			this.Label1.TabIndex = 117;
			this.Label1.Text = "== this window will not be displayed anymore ==";
			this.Label1.TextAlign = ContentAlignment.MiddleCenter;
			this.Button_Skip.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.Button_Skip.Cursor = Cursors.Hand;
			this.Button_Skip.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			Control arg_3A0_0 = this.Button_Skip;
			location = new Point(271, 156);
			arg_3A0_0.Location = location;
			this.Button_Skip.Name = "Button_Skip";
			Control arg_3C7_0 = this.Button_Skip;
			size = new Size(108, 34);
			arg_3C7_0.Size = size;
			this.Button_Skip.TabIndex = 118;
			this.Button_Skip.Text = "SKIP";
			this.Button_Skip.UseVisualStyleBackColor = true;
			SizeF autoScaleDimensions = new SizeF(6f, 13f);
			this.AutoScaleDimensions = autoScaleDimensions;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.WhiteSmoke;
			size = new Size(406, 280);
			this.ClientSize = size;
			this.Controls.Add(this.Button_Skip);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.Label_Donate_2);
			this.Controls.Add(this.Button_Continue);
			this.Controls.Add(this.Label_Bundle);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form_Bundle_Q";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.ResumeLayout(false);
		}
		private void Form_Bundle_Q_FormClosing(object sender, FormClosingEventArgs e)
		{
			Module_Donate.DAT_Bundle_Question_Wait = false;
		}
		private void Form_Bundle_Q_Load(object sender, EventArgs e)
		{
			this.Text = Module_Watermark.PRG_Title;
			this.Icon = MyProject.Forms.Form_Main.Icon;
			this.WindowState = FormWindowState.Normal;
			this.Label_Bundle.Text = this.Label_Bundle.Text.Replace("%PRGNAME%", Module_Watermark.PRG_Name);
			this.TopMost = true;
			Application.DoEvents();
			this.TopMost = false;
		}
		private void Button_Continue_Click(object sender, EventArgs e)
		{
			if (Operators.CompareString(Module_Internet.WEB_Site_DATA, "", false) == 0 | Operators.CompareString(Module_Watermark.PRG_LIC_URL_Main, "", false) == 0 | Operators.CompareString(Module_Watermark.PRG_LIC_URL_Mirror, "", false) == 0)
			{
				//Module_Internet.WEB_Init_DATA(false);
			}
			if (Operators.CompareString(Module_Watermark.PRG_LIC_URL_Main, "", false) == 0)
			{
				//Module_MyFunctions.my_MsgBox("Cannot download license info, please retry (verify your firewall/proxy).", MessageBoxIcon.Exclamation, false);
			}
			else
			{
				Process.Start(Module_Watermark.PRG_LIC_URL_Main);
			}
			this.Close();
		}
		private void Button_Skip_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
