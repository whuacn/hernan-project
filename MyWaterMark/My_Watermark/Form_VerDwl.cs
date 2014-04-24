using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using My_Watermark.My;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Forms;
namespace My_Watermark
{
	[DesignerGenerated]
	public class Form_VerDwl : Form
	{
		private IContainer components;
		[AccessedThroughProperty("PictureBox1")]
		private PictureBox _PictureBox1;
		[AccessedThroughProperty("Label_CurVer")]
		private Label _Label_CurVer;
		[AccessedThroughProperty("Label_NewVer")]
		private Label _Label_NewVer;
		[AccessedThroughProperty("Button_Dwl")]
		private Button _Button_Dwl;
		[AccessedThroughProperty("Button_Web")]
		private Button _Button_Web;
		[AccessedThroughProperty("Timer_TM")]
		private Timer _Timer_TM;
		[AccessedThroughProperty("Button_Close")]
		private Button _Button_Close;
		[AccessedThroughProperty("TextBox_News")]
		private TextBox _TextBox_News;
		[AccessedThroughProperty("StatusStrip1")]
		private StatusStrip _StatusStrip1;
		[AccessedThroughProperty("TSSL_Status")]
		private ToolStripStatusLabel _TSSL_Status;
		private bool PRV_ZIP_Downloaded;
		internal virtual PictureBox PictureBox1
		{
			[DebuggerNonUserCode]
			get
			{
				return this._PictureBox1;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._PictureBox1 = value;
			}
		}
		internal virtual Label Label_CurVer
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_CurVer;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_CurVer = value;
			}
		}
		internal virtual Label Label_NewVer
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_NewVer;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_NewVer = value;
			}
		}
		internal virtual Button Button_Dwl
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Dwl;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Dwl_Click);
				if (this._Button_Dwl != null)
				{
					this._Button_Dwl.Click -= value2;
				}
				this._Button_Dwl = value;
				if (this._Button_Dwl != null)
				{
					this._Button_Dwl.Click += value2;
				}
			}
		}
		internal virtual Button Button_Web
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Web;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Web_Click);
				if (this._Button_Web != null)
				{
					this._Button_Web.Click -= value2;
				}
				this._Button_Web = value;
				if (this._Button_Web != null)
				{
					this._Button_Web.Click += value2;
				}
			}
		}
		internal virtual Timer Timer_TM
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Timer_TM;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Timer_TM_Tick);
				if (this._Timer_TM != null)
				{
					this._Timer_TM.Tick -= value2;
				}
				this._Timer_TM = value;
				if (this._Timer_TM != null)
				{
					this._Timer_TM.Tick += value2;
				}
			}
		}
		internal virtual Button Button_Close
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Close;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Close_Click);
				if (this._Button_Close != null)
				{
					this._Button_Close.Click -= value2;
				}
				this._Button_Close = value;
				if (this._Button_Close != null)
				{
					this._Button_Close.Click += value2;
				}
			}
		}
		internal virtual TextBox TextBox_News
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TextBox_News;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox_News = value;
			}
		}
		internal virtual StatusStrip StatusStrip1
		{
			[DebuggerNonUserCode]
			get
			{
				return this._StatusStrip1;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._StatusStrip1 = value;
			}
		}
		internal virtual ToolStripStatusLabel TSSL_Status
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TSSL_Status;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TSSL_Status = value;
			}
		}
		public Form_VerDwl()
		{
			base.Load += new EventHandler(this.Form_VerDwl_Load);
			this.PRV_ZIP_Downloaded = false;
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form_VerDwl));
			this.PictureBox1 = new PictureBox();
			this.Label_CurVer = new Label();
			this.Label_NewVer = new Label();
			this.Button_Dwl = new Button();
			this.Button_Web = new Button();
			this.Timer_TM = new Timer(this.components);
			this.Button_Close = new Button();
			this.TextBox_News = new TextBox();
			this.StatusStrip1 = new StatusStrip();
			this.TSSL_Status = new ToolStripStatusLabel();
			((ISupportInitialize)this.PictureBox1).BeginInit();
			this.StatusStrip1.SuspendLayout();
			this.SuspendLayout();
			this.PictureBox1.Image = (Image)componentResourceManager.GetObject("PictureBox1.Image");
			Control arg_D6_0 = this.PictureBox1;
			Point location = new Point(8, 7);
			arg_D6_0.Location = location;
			this.PictureBox1.Name = "PictureBox1";
			Control arg_FD_0 = this.PictureBox1;
			Size size = new Size(50, 50);
			arg_FD_0.Size = size;
			this.PictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
			this.PictureBox1.TabIndex = 0;
			this.PictureBox1.TabStop = false;
			this.Label_CurVer.AutoSize = true;
			this.Label_CurVer.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_161_0 = this.Label_CurVer;
			location = new Point(64, 14);
			arg_161_0.Location = location;
			this.Label_CurVer.Name = "Label_CurVer";
			Control arg_188_0 = this.Label_CurVer;
			size = new Size(86, 13);
			arg_188_0.Size = size;
			this.Label_CurVer.TabIndex = 1;
			this.Label_CurVer.Text = "Installed version:";
			this.Label_NewVer.AutoSize = true;
			this.Label_NewVer.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_1E4_0 = this.Label_NewVer;
			location = new Point(64, 35);
			arg_1E4_0.Location = location;
			this.Label_NewVer.Name = "Label_NewVer";
			Control arg_20B_0 = this.Label_NewVer;
			size = new Size(114, 13);
			arg_20B_0.Size = size;
			this.Label_NewVer.TabIndex = 2;
			this.Label_NewVer.Text = "New version available:";
			this.Button_Dwl.Cursor = Cursors.Hand;
			this.Button_Dwl.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_26A_0 = this.Button_Dwl;
			location = new Point(8, 67);
			arg_26A_0.Location = location;
			this.Button_Dwl.Name = "Button_Dwl";
			Control arg_294_0 = this.Button_Dwl;
			size = new Size(267, 23);
			arg_294_0.Size = size;
			this.Button_Dwl.TabIndex = 3;
			this.Button_Dwl.Text = "Download new version and update";
			this.Button_Dwl.UseVisualStyleBackColor = true;
			this.Button_Web.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.Button_Web.Cursor = Cursors.Hand;
			this.Button_Web.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_310_0 = this.Button_Web;
			location = new Point(355, 67);
			arg_310_0.Location = location;
			this.Button_Web.Name = "Button_Web";
			Control arg_337_0 = this.Button_Web;
			size = new Size(122, 23);
			arg_337_0.Size = size;
			this.Button_Web.TabIndex = 4;
			this.Button_Web.Text = "Visit website";
			this.Button_Web.UseVisualStyleBackColor = true;
			this.Timer_TM.Enabled = true;
			this.Timer_TM.Interval = 500;
			this.Button_Close.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.Button_Close.Cursor = Cursors.Hand;
			this.Button_Close.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_3CE_0 = this.Button_Close;
			location = new Point(408, 7);
			arg_3CE_0.Location = location;
			this.Button_Close.Name = "Button_Close";
			Control arg_3F5_0 = this.Button_Close;
			size = new Size(69, 23);
			arg_3F5_0.Size = size;
			this.Button_Close.TabIndex = 5;
			this.Button_Close.Text = "Close";
			this.Button_Close.UseVisualStyleBackColor = true;
			this.TextBox_News.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.TextBox_News.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_45D_0 = this.TextBox_News;
			location = new Point(8, 101);
			arg_45D_0.Location = location;
			this.TextBox_News.Multiline = true;
			this.TextBox_News.Name = "TextBox_News";
			this.TextBox_News.ScrollBars = ScrollBars.Vertical;
			Control arg_4A2_0 = this.TextBox_News;
			size = new Size(469, 136);
			arg_4A2_0.Size = size;
			this.TextBox_News.TabIndex = 6;
			this.TextBox_News.Text = "What's new in v1.5:";
			this.StatusStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.TSSL_Status
			});
			Control arg_4F8_0 = this.StatusStrip1;
			location = new Point(0, 240);
			arg_4F8_0.Location = location;
			this.StatusStrip1.Name = "StatusStrip1";
			Control arg_522_0 = this.StatusStrip1;
			size = new Size(484, 22);
			arg_522_0.Size = size;
			this.StatusStrip1.TabIndex = 7;
			this.StatusStrip1.Text = "StatusStrip1";
			this.TSSL_Status.Name = "TSSL_Status";
			ToolStripItem arg_565_0 = this.TSSL_Status;
			size = new Size(87, 17);
			arg_565_0.Size = size;
			this.TSSL_Status.Text = "Downloading...";
			SizeF autoScaleDimensions = new SizeF(6f, 13f);
			this.AutoScaleDimensions = autoScaleDimensions;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.WhiteSmoke;
			size = new Size(484, 262);
			this.ClientSize = size;
			this.Controls.Add(this.StatusStrip1);
			this.Controls.Add(this.TextBox_News);
			this.Controls.Add(this.Button_Close);
			this.Controls.Add(this.Button_Web);
			this.Controls.Add(this.Button_Dwl);
			this.Controls.Add(this.Label_NewVer);
			this.Controls.Add(this.Label_CurVer);
			this.Controls.Add(this.PictureBox1);
			this.ForeColor = Color.Black;
			this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			this.MinimizeBox = false;
			size = new Size(500, 300);
			this.MinimumSize = size;
			this.Name = "Form_VerDwl";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Version dwl";
			((ISupportInitialize)this.PictureBox1).EndInit();
			this.StatusStrip1.ResumeLayout(false);
			this.StatusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		public void showVerDwl(string my_NewVer)
		{
			this.Label_CurVer.Text = "Installed version: " + Module_Watermark.PRG_Version;
			this.Label_NewVer.Text = "New version available: " + my_NewVer;
			this.TextBox_News.Text = "What's new in " + my_NewVer + ":\r\n\r\n";
			checked
			{
				if (Operators.CompareString(Module_Internet.WEB_EXEUPD_NEW, "", false) == 0)
				{
					this.TextBox_News.Text = "changelog not available. Check the site for more info.";
				}
				else
				{
					string[] array = Module_Internet.WEB_EXEUPD_NEW.Split(new char[]
					{
						'|'
					});
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string str = array2[i];
						TextBox textBox_News = this.TextBox_News;
						textBox_News.Text = textBox_News.Text + "- " + str + "\r\n";
					}
				}
				this.Label_CurVer.Focus();
				this.Show();
			}
		}
		private void Form_VerDwl_Load(object sender, EventArgs e)
		{
			this.Text = " " + Module_Watermark.PRG_Name + " - new version found!";
			this.WindowState = FormWindowState.Normal;
			this.TSSL_Status.Text = "";
		}
		private void Button_Dwl_Click(object sender, EventArgs e)
		{
			this.Timer_TM.Enabled = false;
			this.Button_Dwl.Enabled = false;
			if (this.PRV_ZIP_Downloaded)
			{
				this.Timer_TM.Enabled = false;
				string text = "updater.bat";
				try
				{
					if (File.Exists(Module_MyFunctions.DAT_Path + text))
					{
						File.Delete(Module_MyFunctions.DAT_Path + text);
					}
				}
				catch (Exception expr_5F)
				{
					ProjectData.SetProjectError(expr_5F);
					Exception ex = expr_5F;
					Module_MyFunctions.Update_LOG("ERROR [Button_Dwl_Click.FileDelete(updater.bat)] [" + ex.Message + "]", false, "", false);
					text = Module_MyFunctions.Get_Unique_Filename(Module_MyFunctions.DAT_Path, "bat");
					ProjectData.ClearProjectError();
				}
				try
				{
					if (Operators.CompareString(Strings.Mid(Module_MyFunctions.DAT_Path, 2, 1), ":", false) != 0)
					{
						Module_MyFunctions.my_MsgBox("Sorry, the autoupdater works only on local hard drive and cannot be launched on network path.\r\n\r\nYou should download the new version manually.", MessageBoxIcon.Hand, false);
						try
						{
							File.Delete(Module_MyFunctions.DAT_Path + Module_Internet.WEB_EXEUPD_DAT);
							return;
						}
						catch (Exception expr_DF)
						{
							ProjectData.SetProjectError(expr_DF);
							ProjectData.ClearProjectError();
							return;
						}
					}
					StreamWriter streamWriter = new StreamWriter(Module_MyFunctions.DAT_Path + text, false);
					string text2 = Module_Internet.WEB_Updater_DATA.Replace("%PRGNAME%", Module_Watermark.PRG_Name);
					text2 = text2.Replace("%PRGEXE%", Path.GetFileName(Application.ExecutablePath));
					text2 = text2.Replace("%PRGUPD%", Module_Internet.WEB_EXEUPD_DAT);
					string value = Strings.Left(Module_MyFunctions.DAT_Path, 2);
					string str = Strings.Mid(Module_MyFunctions.DAT_Path, 3);
					streamWriter.WriteLine(value);
					streamWriter.WriteLine("cd \"" + str + "\"");
					streamWriter.WriteLine(text2);
					streamWriter.Close();
				}
				catch (Exception expr_18A)
				{
					ProjectData.SetProjectError(expr_18A);
					Exception ex2 = expr_18A;
					Module_MyFunctions.Update_LOG(string.Concat(new string[]
					{
						"ERROR [Button_Dwl_Click.StreamWrite(",
						text,
						")] [",
						ex2.Message,
						"]"
					}), false, "", false);
					Module_MyFunctions.my_MsgBox("ERROR cannot write updater batch file, verify the log!", MessageBoxIcon.Hand, false);
					this.Button_Dwl.Enabled = true;
					ProjectData.ClearProjectError();
					return;
				}
				Process.Start(Module_MyFunctions.DAT_Path + text);
				Module_MyFunctions.PRG_Exit = true;
				MyProject.Forms.Form_Main.Timer_Close.Enabled = true;
				if (Module_MyFunctions.DAT_Command | Module_MyFunctions.PRG_Loading)
				{
					this.Close();
				}
				else
				{
					MyProject.Forms.Form_Main.Close();
				}
			}
			else
			{
				bool flag = false;
				string text3 = Module_Internet.WEB_EXEUPD_URL;
				this.TSSL_Status.Text = "Downloading new version...";
				this.Button_Dwl.Text = "Downloading, please wait...";
				if (Module_MyFunctions.DEBUG_MODE)
				{
					text3 = text3.Replace("www.myportablesoftware.com", "localhost:1958");
				}
				if (Operators.CompareString(Module_Internet.WEB_Updater_DATA, "", false) == 0)
				{
					Module_Internet.WEB_Init_DATA(true);
				}
				string arg_2D6_0 = text3;
				string arg_2D6_1 = Module_MyFunctions.DAT_Path + Module_Internet.WEB_EXEUPD_DAT;
				bool arg_2D6_2 = false;
				ProgressBar progressBar = null;
				bool arg_2D6_4 = true;
				bool arg_2D6_5 = false;
				bool flag2 = false;
				bool flag3 = false;
				if (Module_Internet.WEB_Download(arg_2D6_0, arg_2D6_1, arg_2D6_2, progressBar, arg_2D6_4, arg_2D6_5, flag2, flag3, false) != null)
				{
					if (!File.Exists(Module_MyFunctions.DAT_Path + Module_Internet.WEB_EXEUPD_DAT))
					{
						Module_MyFunctions.my_MsgBox("ERROR update file " + Module_Internet.WEB_EXEUPD_DAT + " not found, verify the log!", MessageBoxIcon.Hand, false);
						flag = true;
					}
					else
					{
						MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
						FileStream fileStream = new FileStream(Module_MyFunctions.DAT_Path + Module_Internet.WEB_EXEUPD_DAT, FileMode.Open, FileAccess.Read);
						string left = Convert.ToBase64String(mD5CryptoServiceProvider.ComputeHash(fileStream));
						fileStream.Close();
						if (Operators.CompareString(left, Module_Internet.WEB_EXEUPD_MD5, false) != 0)
						{
							Module_MyFunctions.my_MsgBox("ERROR: updater file has not been downloaded correctly, please retry!", MessageBoxIcon.Hand, false);
							flag = true;
						}
						else
						{
							if (Operators.CompareString(Module_Internet.WEB_Updater_DATA, "", false) == 0)
							{
								Module_MyFunctions.my_MsgBox("ERROR: updater module has not been downloaded correctly, please retry!", MessageBoxIcon.Hand, false);
								flag = true;
							}
						}
					}
					if (flag)
					{
						this.TSSL_Status.Text = "Download error, please retry";
						this.Button_Dwl.Text = "Download new version and update";
					}
					else
					{
						this.PRV_ZIP_Downloaded = true;
						this.Button_Close.Text = "Cancel";
						Control arg_408_0 = this.Button_Dwl;
						Point p = new Point(this.TextBox_News.Width, this.Button_Dwl.Size.Height);
						arg_408_0.Size = (Size)p;
						this.Button_Web.Visible = false;
						this.Button_Dwl.Text = "Click here to update and launch new version!";
						this.TSSL_Status.Text = "New version downloaded!";
					}
				}
				else
				{
					this.TSSL_Status.Text = "Download error, please retry";
					this.Button_Dwl.Text = "Download new version and update";
				}
			}
			this.Button_Dwl.Enabled = true;
		}
		private void Button_Web_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(Module_Internet.WEB_Site_URL);
			}
			catch (Exception expr_0D)
			{
				ProjectData.SetProjectError(expr_0D);
				ProjectData.ClearProjectError();
			}
		}
		private void Timer_TM_Tick(object sender, EventArgs e)
		{
			if (!(Module_MyFunctions.DAT_Dialog | MyProject.Forms.Form_Donate.Visible))
			{
				this.TopMost = true;
				Application.DoEvents();
				this.TopMost = false;
			}
			this.Timer_TM.Interval = 5000;
		}
		private void Button_Close_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
