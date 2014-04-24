using Microsoft.VisualBasic;
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
	public class Form_Donate : Form
	{
		private IContainer components;
		[AccessedThroughProperty("Label_Donate")]
		private Label _Label_Donate;
		[AccessedThroughProperty("WebBrowser_Donate")]
		private WebBrowser _WebBrowser_Donate;
		[AccessedThroughProperty("Timer_TM")]
		private Timer _Timer_TM;
		[AccessedThroughProperty("Button_Donate")]
		private Button _Button_Donate;
		[AccessedThroughProperty("TextBox_EMail")]
		private TextBox _TextBox_EMail;
		[AccessedThroughProperty("Label1")]
		private Label _Label1;
		[AccessedThroughProperty("Button_Skip_Donation")]
		private Button _Button_Skip_Donation;
		[AccessedThroughProperty("Label2")]
		private Label _Label2;
		[AccessedThroughProperty("TextBox_PayPalID")]
		private TextBox _TextBox_PayPalID;
		[AccessedThroughProperty("Label3")]
		private Label _Label3;
		[AccessedThroughProperty("Label_Donate_2")]
		private Label _Label_Donate_2;
		[AccessedThroughProperty("GroupBox1")]
		private GroupBox _GroupBox1;
		[AccessedThroughProperty("GroupBox2")]
		private GroupBox _GroupBox2;
		[AccessedThroughProperty("Button_DWL_Mirror_Link")]
		private Button _Button_DWL_Mirror_Link;
		[AccessedThroughProperty("Label_Bundle")]
		private Label _Label_Bundle;
		[AccessedThroughProperty("Button_DWL_Main_Link")]
		private Button _Button_DWL_Main_Link;
		[AccessedThroughProperty("Label4")]
		private Label _Label4;
		[AccessedThroughProperty("Label_License_Bundle")]
		private Label _Label_License_Bundle;
		[AccessedThroughProperty("GroupBox4")]
		private GroupBox _GroupBox4;
		[AccessedThroughProperty("WebBrowser_Sponsor")]
		private WebBrowser _WebBrowser_Sponsor;
		[AccessedThroughProperty("Button_DWL_Register")]
		private Button _Button_DWL_Register;
		[AccessedThroughProperty("TextBox_DWL_License_Code")]
		private TextBox _TextBox_DWL_License_Code;
		[AccessedThroughProperty("Label6")]
		private Label _Label6;
		[AccessedThroughProperty("Label7")]
		private Label _Label7;
		[AccessedThroughProperty("Label8")]
		private Label _Label8;
		[AccessedThroughProperty("Label9")]
		private Label _Label9;
		[AccessedThroughProperty("Timer_Blink")]
		private Timer _Timer_Blink;
		[AccessedThroughProperty("Button_PayPalWEB")]
		private Button _Button_PayPalWEB;
		[AccessedThroughProperty("Label_Line")]
		private Label _Label_Line;
		private int PRV_Countdown;
		private bool PRV_Form_TOP;
		private bool FRM_Closing;
		private bool FRM_Dialog;
		private int FRM_Blink;
		internal virtual Label Label_Donate
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Donate;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_Donate = value;
			}
		}
		internal virtual WebBrowser WebBrowser_Donate
		{
			[DebuggerNonUserCode]
			get
			{
				return this._WebBrowser_Donate;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._WebBrowser_Donate = value;
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
		internal virtual Button Button_Donate
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Donate;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Donate_Click);
				if (this._Button_Donate != null)
				{
					this._Button_Donate.Click -= value2;
				}
				this._Button_Donate = value;
				if (this._Button_Donate != null)
				{
					this._Button_Donate.Click += value2;
				}
			}
		}
		internal virtual TextBox TextBox_EMail
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TextBox_EMail;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox_EMail = value;
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
		internal virtual Button Button_Skip_Donation
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Skip_Donation;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Close_Click);
				if (this._Button_Skip_Donation != null)
				{
					this._Button_Skip_Donation.Click -= value2;
				}
				this._Button_Skip_Donation = value;
				if (this._Button_Skip_Donation != null)
				{
					this._Button_Skip_Donation.Click += value2;
				}
			}
		}
		internal virtual Label Label2
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label2;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label2 = value;
			}
		}
		internal virtual TextBox TextBox_PayPalID
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TextBox_PayPalID;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox_PayPalID = value;
			}
		}
		internal virtual Label Label3
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label3;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label3 = value;
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
		internal virtual GroupBox GroupBox1
		{
			[DebuggerNonUserCode]
			get
			{
				return this._GroupBox1;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._GroupBox1 = value;
			}
		}
		internal virtual GroupBox GroupBox2
		{
			[DebuggerNonUserCode]
			get
			{
				return this._GroupBox2;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._GroupBox2 = value;
			}
		}
		internal virtual Button Button_DWL_Mirror_Link
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_DWL_Mirror_Link;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_DWL_LIC_Link_Click);
				if (this._Button_DWL_Mirror_Link != null)
				{
					this._Button_DWL_Mirror_Link.Click -= value2;
				}
				this._Button_DWL_Mirror_Link = value;
				if (this._Button_DWL_Mirror_Link != null)
				{
					this._Button_DWL_Mirror_Link.Click += value2;
				}
			}
		}
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
		internal virtual Button Button_DWL_Main_Link
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_DWL_Main_Link;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_DWL_LIC_Link_Click);
				if (this._Button_DWL_Main_Link != null)
				{
					this._Button_DWL_Main_Link.Click -= value2;
				}
				this._Button_DWL_Main_Link = value;
				if (this._Button_DWL_Main_Link != null)
				{
					this._Button_DWL_Main_Link.Click += value2;
				}
			}
		}
		internal virtual Label Label4
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label4;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label4 = value;
			}
		}
		internal virtual Label Label_License_Bundle
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_License_Bundle;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_License_Bundle = value;
			}
		}
		internal virtual GroupBox GroupBox4
		{
			[DebuggerNonUserCode]
			get
			{
				return this._GroupBox4;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._GroupBox4 = value;
			}
		}
		internal virtual WebBrowser WebBrowser_Sponsor
		{
			[DebuggerNonUserCode]
			get
			{
				return this._WebBrowser_Sponsor;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._WebBrowser_Sponsor = value;
			}
		}
		internal virtual Button Button_DWL_Register
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_DWL_Register;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_DWL_Register_Click);
				if (this._Button_DWL_Register != null)
				{
					this._Button_DWL_Register.Click -= value2;
				}
				this._Button_DWL_Register = value;
				if (this._Button_DWL_Register != null)
				{
					this._Button_DWL_Register.Click += value2;
				}
			}
		}
		internal virtual TextBox TextBox_DWL_License_Code
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TextBox_DWL_License_Code;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox_DWL_License_Code = value;
			}
		}
		internal virtual Label Label6
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label6;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label6 = value;
			}
		}
		internal virtual Label Label7
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label7;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label7 = value;
			}
		}
		internal virtual Label Label8
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label8;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label8 = value;
			}
		}
		internal virtual Label Label9
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label9;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label9 = value;
			}
		}
		internal virtual Timer Timer_Blink
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Timer_Blink;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Timer_Blink_Tick);
				if (this._Timer_Blink != null)
				{
					this._Timer_Blink.Tick -= value2;
				}
				this._Timer_Blink = value;
				if (this._Timer_Blink != null)
				{
					this._Timer_Blink.Tick += value2;
				}
			}
		}
		internal virtual Button Button_PayPalWEB
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_PayPalWEB;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_PayPalWEB_Click);
				if (this._Button_PayPalWEB != null)
				{
					this._Button_PayPalWEB.Click -= value2;
				}
				this._Button_PayPalWEB = value;
				if (this._Button_PayPalWEB != null)
				{
					this._Button_PayPalWEB.Click += value2;
				}
			}
		}
		internal virtual Label Label_Line
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Line;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_Line = value;
			}
		}
		public Form_Donate()
		{
			base.FormClosing += new FormClosingEventHandler(this.Form_Donate_FormClosing);
			base.Load += new EventHandler(this.Form_Donate_Load);
			this.PRV_Form_TOP = false;
			this.FRM_Closing = false;
			this.FRM_Dialog = false;
			this.FRM_Blink = 0;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form_Donate));
			this.Label_Donate = new Label();
			this.WebBrowser_Donate = new WebBrowser();
			this.Timer_TM = new Timer(this.components);
			this.Button_Donate = new Button();
			this.TextBox_EMail = new TextBox();
			this.Label1 = new Label();
			this.Button_Skip_Donation = new Button();
			this.Label2 = new Label();
			this.TextBox_PayPalID = new TextBox();
			this.Label3 = new Label();
			this.Label_Donate_2 = new Label();
			this.GroupBox1 = new GroupBox();
			this.Button_PayPalWEB = new Button();
			this.Label4 = new Label();
			this.GroupBox2 = new GroupBox();
			this.Label9 = new Label();
			this.Button_DWL_Register = new Button();
			this.TextBox_DWL_License_Code = new TextBox();
			this.Button_DWL_Main_Link = new Button();
			this.Label_Bundle = new Label();
			this.Button_DWL_Mirror_Link = new Button();
			this.Label_License_Bundle = new Label();
			this.GroupBox4 = new GroupBox();
			this.WebBrowser_Sponsor = new WebBrowser();
			this.Label6 = new Label();
			this.Label7 = new Label();
			this.Label8 = new Label();
			this.Timer_Blink = new Timer(this.components);
			this.Label_Line = new Label();
			this.GroupBox1.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			this.GroupBox4.SuspendLayout();
			this.SuspendLayout();
			this.Label_Donate.BackColor = Color.WhiteSmoke;
			this.Label_Donate.Cursor = Cursors.Default;
			this.Label_Donate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_Donate.ForeColor = Color.Black;
			Control arg_1EB_0 = this.Label_Donate;
			Point location = new Point(6, 13);
			arg_1EB_0.Location = location;
			this.Label_Donate.Name = "Label_Donate";
			Control arg_212_0 = this.Label_Donate;
			Padding padding = new Padding(5, 7, 5, 5);
			arg_212_0.Padding = padding;
			Control arg_22C_0 = this.Label_Donate;
			Size size = new Size(745, 102);
			arg_22C_0.Size = size;
			this.Label_Donate.TabIndex = 92;
			this.Label_Donate.Text = componentResourceManager.GetString("Label_Donate.Text");
			Control arg_265_0 = this.WebBrowser_Donate;
			location = new Point(8, 24);
			arg_265_0.Location = location;
			Control arg_279_0 = this.WebBrowser_Donate;
			padding = new Padding(0);
			arg_279_0.Margin = padding;
			Control arg_290_0 = this.WebBrowser_Donate;
			size = new Size(20, 20);
			arg_290_0.MinimumSize = size;
			this.WebBrowser_Donate.Name = "WebBrowser_Donate";
			this.WebBrowser_Donate.ScriptErrorsSuppressed = true;
			this.WebBrowser_Donate.ScrollBarsEnabled = false;
			Control arg_2D2_0 = this.WebBrowser_Donate;
			size = new Size(435, 75);
			arg_2D2_0.Size = size;
			this.WebBrowser_Donate.TabIndex = 94;
			this.Timer_TM.Interval = 1000;
			this.Button_Donate.Cursor = Cursors.Hand;
			this.Button_Donate.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			Control arg_336_0 = this.Button_Donate;
			location = new Point(601, 67);
			arg_336_0.Location = location;
			this.Button_Donate.Name = "Button_Donate";
			Control arg_360_0 = this.Button_Donate;
			size = new Size(144, 35);
			arg_360_0.Size = size;
			this.Button_Donate.TabIndex = 3;
			this.Button_Donate.Text = "Register";
			this.Button_Donate.UseVisualStyleBackColor = true;
			this.TextBox_EMail.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_3BF_0 = this.TextBox_EMail;
			location = new Point(601, 22);
			arg_3BF_0.Location = location;
			this.TextBox_EMail.Name = "TextBox_EMail";
			Control arg_3E9_0 = this.TextBox_EMail;
			size = new Size(144, 20);
			arg_3E9_0.Size = size;
			this.TextBox_EMail.TabIndex = 1;
			this.Label1.BackColor = Color.Gainsboro;
			this.Label1.Cursor = Cursors.Default;
			this.Label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label1.ForeColor = Color.Black;
			Control arg_45C_0 = this.Label1;
			location = new Point(449, 24);
			arg_45C_0.Location = location;
			this.Label1.Name = "Label1";
			Control arg_486_0 = this.Label1;
			size = new Size(148, 16);
			arg_486_0.Size = size;
			this.Label1.TabIndex = 97;
			this.Label1.Text = "EMail used on PayPal";
			this.Label1.TextAlign = ContentAlignment.TopRight;
			this.Button_Skip_Donation.BackColor = Color.WhiteSmoke;
			this.Button_Skip_Donation.Cursor = Cursors.Hand;
			this.Button_Skip_Donation.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_506_0 = this.Button_Skip_Donation;
			location = new Point(575, 15);
			arg_506_0.Location = location;
			this.Button_Skip_Donation.Name = "Button_Skip_Donation";
			Control arg_530_0 = this.Button_Skip_Donation;
			size = new Size(173, 26);
			arg_530_0.Size = size;
			this.Button_Skip_Donation.TabIndex = 100;
			this.Button_Skip_Donation.Text = "Skip donation";
			this.Button_Skip_Donation.UseVisualStyleBackColor = false;
			this.Label2.BackColor = Color.Gainsboro;
			this.Label2.Cursor = Cursors.Default;
			this.Label2.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label2.ForeColor = Color.Black;
			Control arg_5C0_0 = this.Label2;
			location = new Point(449, 46);
			arg_5C0_0.Location = location;
			this.Label2.Name = "Label2";
			Control arg_5EA_0 = this.Label2;
			size = new Size(148, 16);
			arg_5EA_0.Size = size;
			this.Label2.TabIndex = 101;
			this.Label2.Text = "PayPal transaction ID";
			this.Label2.TextAlign = ContentAlignment.TopRight;
			this.TextBox_PayPalID.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_64A_0 = this.TextBox_PayPalID;
			location = new Point(601, 44);
			arg_64A_0.Location = location;
			this.TextBox_PayPalID.Name = "TextBox_PayPalID";
			Control arg_674_0 = this.TextBox_PayPalID;
			size = new Size(144, 20);
			arg_674_0.Size = size;
			this.TextBox_PayPalID.TabIndex = 2;
			this.Label3.BackColor = Color.Gainsboro;
			this.Label3.Cursor = Cursors.Default;
			this.Label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label3.ForeColor = Color.Blue;
			Control arg_6E7_0 = this.Label3;
			location = new Point(212, 107);
			arg_6E7_0.Location = location;
			this.Label3.Name = "Label3";
			Control arg_711_0 = this.Label3;
			size = new Size(533, 20);
			arg_711_0.Size = size;
			this.Label3.TabIndex = 102;
			this.Label3.Text = "The registration code is valid for all our software! With only one donation you can register all our software!";
			this.Label3.TextAlign = ContentAlignment.MiddleCenter;
			this.Label_Donate_2.BackColor = Color.Yellow;
			this.Label_Donate_2.BorderStyle = BorderStyle.FixedSingle;
			this.Label_Donate_2.Cursor = Cursors.Default;
			this.Label_Donate_2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.Label_Donate_2.ForeColor = Color.Black;
			Control arg_7AD_0 = this.Label_Donate_2;
			location = new Point(4, 380);
			arg_7AD_0.Location = location;
			this.Label_Donate_2.Name = "Label_Donate_2";
			Control arg_7D7_0 = this.Label_Donate_2;
			size = new Size(752, 19);
			arg_7D7_0.Size = size;
			this.Label_Donate_2.TabIndex = 103;
			this.Label_Donate_2.Text = "After registering, this window %LOGO%will never displayed anymore :)";
			this.Label_Donate_2.TextAlign = ContentAlignment.MiddleCenter;
			this.GroupBox1.BackColor = Color.WhiteSmoke;
			this.GroupBox1.Controls.Add(this.Button_PayPalWEB);
			this.GroupBox1.Controls.Add(this.Button_Donate);
			this.GroupBox1.Controls.Add(this.WebBrowser_Donate);
			this.GroupBox1.Controls.Add(this.TextBox_EMail);
			this.GroupBox1.Controls.Add(this.Label3);
			this.GroupBox1.Controls.Add(this.Label1);
			this.GroupBox1.Controls.Add(this.TextBox_PayPalID);
			this.GroupBox1.Controls.Add(this.Label2);
			Control arg_8DA_0 = this.GroupBox1;
			location = new Point(4, 131);
			arg_8DA_0.Location = location;
			this.GroupBox1.Name = "GroupBox1";
			Control arg_907_0 = this.GroupBox1;
			size = new Size(751, 135);
			arg_907_0.Size = size;
			this.GroupBox1.TabIndex = 110;
			this.GroupBox1.TabStop = false;
			this.Button_PayPalWEB.Cursor = Cursors.Hand;
			this.Button_PayPalWEB.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_963_0 = this.Button_PayPalWEB;
			location = new Point(8, 106);
			arg_963_0.Location = location;
			this.Button_PayPalWEB.Name = "Button_PayPalWEB";
			Control arg_98D_0 = this.Button_PayPalWEB;
			size = new Size(195, 22);
			arg_98D_0.Size = size;
			this.Button_PayPalWEB.TabIndex = 116;
			this.Button_PayPalWEB.Text = "Open PayPal in the webbrowser";
			this.Button_PayPalWEB.UseVisualStyleBackColor = true;
			this.Label4.BackColor = Color.GreenYellow;
			this.Label4.BorderStyle = BorderStyle.FixedSingle;
			this.Label4.Cursor = Cursors.Default;
			this.Label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.Label4.ForeColor = Color.Black;
			Control arg_A29_0 = this.Label4;
			location = new Point(224, 124);
			arg_A29_0.Location = location;
			this.Label4.Name = "Label4";
			Control arg_A53_0 = this.Label4;
			size = new Size(212, 25);
			arg_A53_0.Size = size;
			this.Label4.TabIndex = 113;
			this.Label4.Text = "License type: LIFETIME";
			this.Label4.TextAlign = ContentAlignment.MiddleCenter;
			this.GroupBox2.Controls.Add(this.Label9);
			this.GroupBox2.Controls.Add(this.Button_DWL_Register);
			this.GroupBox2.Controls.Add(this.TextBox_DWL_License_Code);
			this.GroupBox2.Controls.Add(this.Button_DWL_Main_Link);
			this.GroupBox2.Controls.Add(this.Label_Bundle);
			this.GroupBox2.Controls.Add(this.Button_DWL_Mirror_Link);
			Control arg_B1A_0 = this.GroupBox2;
			location = new Point(4, 284);
			arg_B1A_0.Location = location;
			this.GroupBox2.Name = "GroupBox2";
			Control arg_B44_0 = this.GroupBox2;
			size = new Size(751, 93);
			arg_B44_0.Size = size;
			this.GroupBox2.TabIndex = 111;
			this.GroupBox2.TabStop = false;
			this.GroupBox2.Text = "Register by DOWNLOADING";
			this.Label9.BackColor = Color.Gainsboro;
			this.Label9.Cursor = Cursors.Default;
			this.Label9.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label9.ForeColor = Color.Black;
			Control arg_BD4_0 = this.Label9;
			location = new Point(418, 25);
			arg_BD4_0.Location = location;
			this.Label9.Name = "Label9";
			Control arg_BFB_0 = this.Label9;
			size = new Size(106, 16);
			arg_BFB_0.Size = size;
			this.Label9.TabIndex = 118;
			this.Label9.Text = "License code";
			this.Label9.TextAlign = ContentAlignment.TopRight;
			this.Button_DWL_Register.Cursor = Cursors.Hand;
			this.Button_DWL_Register.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			Control arg_C6B_0 = this.Button_DWL_Register;
			location = new Point(601, 49);
			arg_C6B_0.Location = location;
			this.Button_DWL_Register.Name = "Button_DWL_Register";
			Control arg_C95_0 = this.Button_DWL_Register;
			size = new Size(144, 35);
			arg_C95_0.Size = size;
			this.Button_DWL_Register.TabIndex = 116;
			this.Button_DWL_Register.Text = "Register";
			this.Button_DWL_Register.UseVisualStyleBackColor = true;
			this.TextBox_DWL_License_Code.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_CF5_0 = this.TextBox_DWL_License_Code;
			location = new Point(529, 23);
			arg_CF5_0.Location = location;
			this.TextBox_DWL_License_Code.Name = "TextBox_DWL_License_Code";
			Control arg_D1F_0 = this.TextBox_DWL_License_Code;
			size = new Size(215, 20);
			arg_D1F_0.Size = size;
			this.TextBox_DWL_License_Code.TabIndex = 115;
			this.Button_DWL_Main_Link.Cursor = Cursors.Hand;
			this.Button_DWL_Main_Link.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_D6F_0 = this.Button_DWL_Main_Link;
			location = new Point(8, 64);
			arg_D6F_0.Location = location;
			this.Button_DWL_Main_Link.Name = "Button_DWL_Main_Link";
			Control arg_D99_0 = this.Button_DWL_Main_Link;
			size = new Size(209, 23);
			arg_D99_0.Size = size;
			this.Button_DWL_Main_Link.TabIndex = 113;
			this.Button_DWL_Main_Link.Text = "Download license code (MAIN link)";
			this.Button_DWL_Main_Link.UseVisualStyleBackColor = true;
			this.Label_Bundle.BackColor = Color.FromArgb(192, 255, 192);
			this.Label_Bundle.Cursor = Cursors.Default;
			this.Label_Bundle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_Bundle.ForeColor = Color.Black;
			Control arg_E34_0 = this.Label_Bundle;
			location = new Point(7, 25);
			arg_E34_0.Location = location;
			this.Label_Bundle.Name = "Label_Bundle";
			Control arg_E5E_0 = this.Label_Bundle;
			size = new Size(407, 36);
			arg_E5E_0.Size = size;
			this.Label_Bundle.TabIndex = 112;
			this.Label_Bundle.Text = "To register %PRGNAME% simply download the license code.";
			this.Button_DWL_Mirror_Link.Cursor = Cursors.Hand;
			this.Button_DWL_Mirror_Link.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_EC2_0 = this.Button_DWL_Mirror_Link;
			location = new Point(223, 64);
			arg_EC2_0.Location = location;
			this.Button_DWL_Mirror_Link.Name = "Button_DWL_Mirror_Link";
			Control arg_EEC_0 = this.Button_DWL_Mirror_Link;
			size = new Size(191, 23);
			arg_EEC_0.Size = size;
			this.Button_DWL_Mirror_Link.TabIndex = 100;
			this.Button_DWL_Mirror_Link.Text = "Download from MIRROR link";
			this.Button_DWL_Mirror_Link.UseVisualStyleBackColor = true;
			this.Label_License_Bundle.BackColor = Color.GreenYellow;
			this.Label_License_Bundle.BorderStyle = BorderStyle.FixedSingle;
			this.Label_License_Bundle.Cursor = Cursors.Default;
			this.Label_License_Bundle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.Label_License_Bundle.ForeColor = Color.Black;
			Control arg_F8B_0 = this.Label_License_Bundle;
			location = new Point(224, 278);
			arg_F8B_0.Location = location;
			this.Label_License_Bundle.Name = "Label_License_Bundle";
			Control arg_FB5_0 = this.Label_License_Bundle;
			size = new Size(212, 25);
			arg_FB5_0.Size = size;
			this.Label_License_Bundle.TabIndex = 114;
			this.Label_License_Bundle.Text = "License type: %MONTHS%";
			this.Label_License_Bundle.TextAlign = ContentAlignment.MiddleCenter;
			this.GroupBox4.Controls.Add(this.WebBrowser_Sponsor);
			Control arg_100E_0 = this.GroupBox4;
			location = new Point(6, 402);
			arg_100E_0.Location = location;
			this.GroupBox4.Name = "GroupBox4";
			Control arg_1038_0 = this.GroupBox4;
			size = new Size(749, 112);
			arg_1038_0.Size = size;
			this.GroupBox4.TabIndex = 113;
			this.GroupBox4.TabStop = false;
			this.GroupBox4.Text = "Sponsor";
			Control arg_1078_0 = this.WebBrowser_Sponsor;
			location = new Point(10, 15);
			arg_1078_0.Location = location;
			Control arg_108C_0 = this.WebBrowser_Sponsor;
			padding = new Padding(0);
			arg_108C_0.Margin = padding;
			Control arg_10A3_0 = this.WebBrowser_Sponsor;
			size = new Size(20, 20);
			arg_10A3_0.MinimumSize = size;
			this.WebBrowser_Sponsor.Name = "WebBrowser_Sponsor";
			this.WebBrowser_Sponsor.ScriptErrorsSuppressed = true;
			this.WebBrowser_Sponsor.ScrollBarsEnabled = false;
			Control arg_10E5_0 = this.WebBrowser_Sponsor;
			size = new Size(728, 90);
			arg_10E5_0.Size = size;
			this.WebBrowser_Sponsor.TabIndex = 95;
			this.Label6.BackColor = Color.Orange;
			this.Label6.Cursor = Cursors.Default;
			this.Label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label6.ForeColor = Color.Black;
			Control arg_1155_0 = this.Label6;
			location = new Point(3, 9);
			arg_1155_0.Location = location;
			this.Label6.Name = "Label6";
			Control arg_117F_0 = this.Label6;
			size = new Size(752, 110);
			arg_117F_0.Size = size;
			this.Label6.TabIndex = 114;
			this.Label7.BackColor = Color.Gold;
			this.Label7.BorderStyle = BorderStyle.FixedSingle;
			this.Label7.Cursor = Cursors.Default;
			this.Label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.Label7.ForeColor = Color.Black;
			Control arg_11FC_0 = this.Label7;
			location = new Point(12, 124);
			arg_11FC_0.Location = location;
			this.Label7.Name = "Label7";
			Control arg_1226_0 = this.Label7;
			size = new Size(195, 25);
			arg_1226_0.Size = size;
			this.Label7.TabIndex = 114;
			this.Label7.Text = "Register by DONATING";
			this.Label7.TextAlign = ContentAlignment.MiddleCenter;
			this.Label8.BackColor = Color.Gold;
			this.Label8.BorderStyle = BorderStyle.FixedSingle;
			this.Label8.Cursor = Cursors.Default;
			this.Label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.Label8.ForeColor = Color.Black;
			Control arg_12C3_0 = this.Label8;
			location = new Point(12, 278);
			arg_12C3_0.Location = location;
			this.Label8.Name = "Label8";
			Control arg_12ED_0 = this.Label8;
			size = new Size(195, 25);
			arg_12ED_0.Size = size;
			this.Label8.TabIndex = 115;
			this.Label8.Text = "Register by DOWNLOADING";
			this.Label8.TextAlign = ContentAlignment.MiddleCenter;
			this.Timer_Blink.Interval = 200;
			this.Label_Line.BackColor = Color.Black;
			this.Label_Line.Cursor = Cursors.Default;
			this.Label_Line.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_Line.ForeColor = Color.Black;
			Control arg_138D_0 = this.Label_Line;
			location = new Point(4, 270);
			arg_138D_0.Location = location;
			this.Label_Line.Name = "Label_Line";
			Control arg_13B6_0 = this.Label_Line;
			size = new Size(752, 2);
			arg_13B6_0.Size = size;
			this.Label_Line.TabIndex = 116;
			SizeF autoScaleDimensions = new SizeF(6f, 13f);
			this.AutoScaleDimensions = autoScaleDimensions;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.WhiteSmoke;
			size = new Size(761, 517);
			this.ClientSize = size;
			this.Controls.Add(this.Label_Line);
			this.Controls.Add(this.Label8);
			this.Controls.Add(this.Label_License_Bundle);
			this.Controls.Add(this.Label7);
			this.Controls.Add(this.Label4);
			this.Controls.Add(this.Button_Skip_Donation);
			this.Controls.Add(this.GroupBox4);
			this.Controls.Add(this.GroupBox2);
			this.Controls.Add(this.GroupBox1);
			this.Controls.Add(this.Label_Donate_2);
			this.Controls.Add(this.Label_Donate);
			this.Controls.Add(this.Label6);
			this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ForeColor = Color.Black;
			this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			this.MaximizeBox = false;
			size = new Size(777, 555);
			this.MaximumSize = size;
			this.MinimizeBox = false;
			size = new Size(777, 550);
			this.MinimumSize = size;
			this.Name = "Form_Donate";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Donate";
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			this.GroupBox2.ResumeLayout(false);
			this.GroupBox2.PerformLayout();
			this.GroupBox4.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private void Form_Donate_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.PRV_Countdown > 1 & !Module_MyFunctions.PRG_Exit)
			{
				e.Cancel = true;
				this.Timer_Blink.Enabled = true;
			}
			else
			{
				this.FRM_Closing = true;
			}
		}
		private void Form_Donate_Load(object sender, EventArgs e)
		{
			this.Text = Module_Watermark.PRG_Name + " - Donate and register";
			this.Label_Bundle.Text = this.Label_Bundle.Text.Replace("%PRGNAME%", Module_Watermark.PRG_Name);
			this.Label_License_Bundle.Text = "License type: " + Module_Donate.PRG_Donate_Bundle_License_Months_Desc;
			this.WebBrowser_Donate.DocumentText = "<head><body style='margin: 0px 0px 0px 0px; background-color:#FFD700;'><div style='text-align:center; font-face:Arial; color:#000000;'><strong>LOADING PayPal data...<br><br>Please wait</strong></div></body></head>";
			Application.DoEvents();
			if (Operators.CompareString(Module_Watermark.PRG_Donate_WEBHTML, "", false) == 0)
			{
				string arg_8D_0 = "http://www.myportablesoftware.com/donate.aspx";
				bool arg_8D_1 = false;
				bool arg_8D_2 = false;
				bool flag = false;
				bool flag2 = false;
				string text = Module_Internet.WEB_Get(arg_8D_0, arg_8D_1, arg_8D_2, flag, flag2, false);
				if (text != null && text.Contains("<!--MPS.DONATE.PAGE-->"))
				{
					Module_Watermark.PRG_Donate_WEBHTML = text;
				}
			}
			if (Operators.CompareString(Module_Watermark.PRG_Donate_WEBHTML, "", false) == 0)
			{
				Module_Watermark.PRG_Donate_WEBHTML = "<head><body style='margin: 0px 0px 0px 0px; background-color:#F5F5F5;'><form action='https://www.paypal.com/cgi-bin/webscr' method='post' target='_blank'><table align='center' border='0' cellpadding='2' cellspacing='0' style='text-align:center;'><tr><td align='left'><img src='http://www.myportablesoftware.com/img/Logo_PayPal_y25.png' border='0' alt='' align='middle'></td></tr><tr><td valign='middle'><input type='hidden' name='cmd' value='_s-xclick'><input type='hidden' name='on0' value='Donation amount'>Donation amount:&nbsp;<select name='os0'><option value='Low'>Low €10,00 EUR</option><option value='Standard' selected>Standard €15,00 EUR</option><option value='Medium'>Medium €20,00 EUR</option><option value='Generous'>Generous €30,00 EUR</option><option value='Very generous'>Very generous €40,00 EUR</option><option value='I love your work'>I love your work €50,00 EUR</option></select><input type='hidden' name='currency_code' value='EUR'><input type='hidden' name='encrypted' value='-----BEGIN PKCS7-----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-----END PKCS7-----'>&nbsp;<input type='image' src='https://www.paypalobjects.com/en_GB/i/btn/btn_donate_SM.gif' border='0' name='submit' alt='PayPal — The safer, easier way to pay online.' align='top'><img alt='' border='0' src='https://www.paypalobjects.com/it_IT/i/scr/pixel.gif' width='1' height='1'></td></tr></table></form></body></head>";
			}
			this.Show();
			Application.DoEvents();
			this.WebBrowser_Donate.DocumentText = Module_Watermark.PRG_Donate_WEBHTML;
			Application.DoEvents();
			if (this.WebBrowser_Donate.DocumentText.Length < 100)
			{
				Module_MyFunctions.my_Pause(2);
				this.WebBrowser_Donate.DocumentText = Module_Watermark.PRG_Donate_WEBHTML;
				Application.DoEvents();
				if (this.WebBrowser_Donate.DocumentText.Length < 100)
				{
					Module_MyFunctions.my_Pause(1);
					this.WebBrowser_Donate.DocumentText = Module_Watermark.PRG_Donate_WEBHTML;
				}
			}
			this.WebBrowser_Sponsor.Navigate("http://www.myportablesoftware.com/adv.aspx?software=" + Module_Watermark.PRG_KEY_WebName);
			if (Module_Watermark.PRG_PayPal_Logo)
			{
				this.Label_Donate_2.Text = this.Label_Donate_2.Text.Replace("%LOGO%", "and the DONATE logo ");
			}
			else
			{
				this.Label_Donate_2.Text = this.Label_Donate_2.Text.Replace("%LOGO%", "");
			}
			this.TextBox_EMail.Text = Module_MyFunctions.Config_Read("Donate_Email", "", true);
			this.TextBox_PayPalID.Text = Module_MyFunctions.Config_Read("Donate_ID", "", true);
			string text2 = "";
			if (Operators.CompareString(Module_MyFunctions.Config_Read("DWLlicense", "", true), "", false) != 0)
			{
				string text3 = Module_Encrypter.my_Decrypt(Module_MyFunctions.Config_Read("DWLlicense", "", true), null);
				text2 = Module_MyFunctions.Parse_Page_Data(text3, "#mpslic.start#|#mpslic.end#", false, false, false, true);
			}
			if (Module_Donate.PRG_Donate)
			{
				this.PRV_Countdown = 0;
				this.Button_Skip_Donation.Text = "Close";
				this.Button_Skip_Donation.Enabled = true;
				if (Module_Donate.PRG_Donate_Bundle)
				{
					this.Button_DWL_Mirror_Link.Enabled = false;
					this.Button_DWL_Mirror_Link.Text = "License expire: " + text2;
					this.TextBox_DWL_License_Code.Text = "[TEMPORARY LICENSE INSTALLED]";
				}
				else
				{
					this.TextBox_EMail.Enabled = false;
					this.TextBox_PayPalID.Enabled = false;
					this.Button_Donate.Enabled = false;
				}
				this.Timer_TM.Enabled = false;
			}
			else
			{
				this.PRV_Countdown = 10;
				this.Button_Skip_Donation.Text = "(10) Skip donation";
				if (Module_Donate.PRG_Donate_Bundle_License_Expired | Operators.CompareString(Module_MyFunctions.Config_Read("DWLlicense", "", true), "", false) != 0)
				{
					this.Text = this.Text + " - Bundle license expired on: " + text2;
				}
				this.Button_Skip_Donation.Enabled = false;
				this.Timer_TM.Enabled = true;
			}
		}
		private void Timer_TM_Tick(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Normal;
			if (MyProject.Forms.Form_VerDwl.Visible | Module_MyFunctions.DAT_Dialog | this.FRM_Dialog)
			{
				this.TopMost = false;
				if (Module_MyFunctions.DAT_Dialog)
				{
					this.SendToBack();
				}
			}
			else
			{
				if (!this.TopMost & !this.PRV_Form_TOP)
				{
					this.TopMost = true;
					this.PRV_Form_TOP = true;
				}
			}
			checked
			{
				if (this.PRV_Countdown < 1)
				{
					this.TopMost = false;
					this.Button_Skip_Donation.Enabled = true;
					this.Button_Skip_Donation.Text = "Skip donation";
					this.Timer_TM.Enabled = false;
				}
				else
				{
					this.PRV_Countdown--;
					this.Button_Skip_Donation.Enabled = false;
					this.Button_Skip_Donation.Text = "(" + Conversions.ToString(this.PRV_Countdown) + ") Skip donation";
				}
			}
		}
		private void Button_Donate_Click(object sender, EventArgs e)
		{
			this.FRM_Dialog = true;
			Application.DoEvents();
			this.TextBox_EMail.Text = Strings.Trim(this.TextBox_EMail.Text);
			this.TextBox_PayPalID.Text = Strings.Trim(this.TextBox_PayPalID.Text).ToUpper();
			Application.DoEvents();
			if (Operators.CompareString(this.TextBox_EMail.Text, "", false) == 0)
			{
				MessageBox.Show("Insert the email!", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (Operators.CompareString(this.TextBox_PayPalID.Text, "", false) == 0)
			{
				MessageBox.Show("Insert the PayPal transaction id!", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.Button_Donate.Enabled = false;
			this.Button_Donate.Text = "WAIT";
			Application.DoEvents();
			string text = Module_Donate.Donate_Check_Email(this.TextBox_EMail.Text, this.TextBox_PayPalID.Text);
			if (Operators.CompareString(text, "E", false) == 0)
			{
				Module_MyFunctions.my_MsgBox("Cannot verify your email. Network connection error. Please retry.", MessageBoxIcon.Hand, false);
			}
			else
			{
				if (Operators.CompareString(text, "D", false) == 0)
				{
					Module_Donate.PRG_Donate = true;
					this.PRV_Form_TOP = true;
					MyProject.Forms.Form_Main.Donate_Set();
					Module_MyFunctions.my_MsgBox("Thank you so much for your donation!\r\n\r\nThe software has been registered!", MessageBoxIcon.Asterisk, false);
					this.Close();
				}
				else
				{
					if (text.StartsWith("I"))
					{
						Module_Donate.PRG_Donate = false;
						Module_MyFunctions.my_MsgBox("The registration code is wrong.\r\n\r\nIt can be found in the email you have received from us with the subject:\r\n\r\n'My Portable Software - registration code'\r\n\r\nVerify that this email has not been sent to the spam folder!\r\n\r\nHINT: your registration code starts with: " + Strings.Mid(text, 2) + ".....", MessageBoxIcon.Hand, false);
					}
					else
					{
						if (Operators.CompareString(text, "B", false) == 0)
						{
							Module_Donate.PRG_Donate = false;
							Module_MyFunctions.my_MsgBox("This email has been blacklisted. Contact us for more info.", MessageBoxIcon.Hand, false);
						}
						else
						{
							Module_MyFunctions.my_MsgBox("There isn't a donation made by " + this.TextBox_EMail.Text + "\r\n\r\nMake sure you have entered the same email used on PayPal (it's case sensitive! Enter it EXACTLY as on PayPal).\r\n\r\nIf you made it today, it will be validated within ONE HOUR.\r\n\r\nYou'll receive a confirmation email from us with the subject:\r\n'My Portable Software - registration code'\r\n\r\nVerify that this email has not been sent to the spam folder!", MessageBoxIcon.Exclamation, false);
						}
					}
				}
			}
			this.Button_Donate.Enabled = true;
			this.Button_Donate.Text = "Register";
		}
		private void Button_Close_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void Button_DWL_LIC_Link_Click(object sender, EventArgs e)
		{
			this.FRM_Dialog = true;
			Application.DoEvents();
			if (Operators.CompareString(Module_Internet.WEB_Site_DATA, "", false) == 0 | (sender == this.Button_DWL_Main_Link & Operators.CompareString(Module_Watermark.PRG_LIC_URL_Main, "", false) == 0) | (sender == this.Button_DWL_Mirror_Link & Operators.CompareString(Module_Watermark.PRG_LIC_URL_Mirror, "", false) == 0))
			{
				Module_Internet.WEB_Init_DATA(false);
			}
			if ((sender == this.Button_DWL_Main_Link & Operators.CompareString(Module_Watermark.PRG_LIC_URL_Main, "", false) == 0) | (sender == this.Button_DWL_Mirror_Link & Operators.CompareString(Module_Watermark.PRG_LIC_URL_Mirror, "", false) == 0))
			{
				Module_MyFunctions.my_MsgBox("Cannot download license info, please retry (verify your firewall/proxy).", MessageBoxIcon.Exclamation, false);
			}
			else
			{
				if (sender == this.Button_DWL_Main_Link)
				{
					Process.Start(Module_Watermark.PRG_LIC_URL_Main);
				}
				else
				{
					Process.Start(Module_Watermark.PRG_LIC_URL_Mirror);
				}
			}
		}
		private void Button_DWL_Register_Click(object sender, EventArgs e)
		{
			try
			{
				this.FRM_Dialog = true;
				Application.DoEvents();
				this.TextBox_DWL_License_Code.Text = this.TextBox_DWL_License_Code.Text.Trim();
				if (Operators.CompareString(this.TextBox_DWL_License_Code.Text, "", false) == 0)
				{
					Module_MyFunctions.my_MsgBox("Insert the license code", MessageBoxIcon.Exclamation, false);
				}
				else
				{
					if (Operators.CompareString(this.TextBox_DWL_License_Code.Text, "[TEMPORARY LICENSE INSTALLED]", false) == 0 & Module_Donate.PRG_Donate_Bundle)
					{
						Module_MyFunctions.my_MsgBox("The software has been already registered with a temporary license.", MessageBoxIcon.Exclamation, false);
					}
					else
					{
						string text = Module_Encrypter.my_Decrypt(this.TextBox_DWL_License_Code.Text, null);
						if (Operators.CompareString(text, "", false) == 0 | !text.StartsWith("#lic#"))
						{
							Module_MyFunctions.my_MsgBox("The license code is invalid.\r\n\r\nPlease verify that you have copied it correctly.", MessageBoxIcon.Exclamation, false);
						}
						else
						{
							string[] array = Strings.Split(Strings.Mid(text, 6), "|", -1, CompareMethod.Binary);
							if (Operators.CompareString(array[0], "", false) != 0 && Module_Watermark.PRG_LIC_BlackList.Contains("|" + array[0] + "|"))
							{
								Module_MyFunctions.my_MsgBox("This license code has been blacklisted. Please download a new one.", MessageBoxIcon.Exclamation, false);
							}
							else
							{
								string[] array2 = array[0].Split(new char[]
								{
									'-'
								});
								DateTime dateTime = new DateTime(Conversions.ToInteger(array2[0]), Conversions.ToInteger(array2[1]), 1);
								if (DateTime.Compare(dateTime.AddMonths(1), DateAndTime.Now) < 0)
								{
									Module_MyFunctions.my_MsgBox("This license code is expired.\r\n\r\nPlease download a new one.", MessageBoxIcon.Exclamation, false);
								}
								else
								{
									DateTime dateTime2 = DateAndTime.Now.AddMonths(Module_Donate.PRG_Donate_Bundle_License_Months);
									string my_Value = Module_Encrypter.my_Encrypt(string.Concat(new string[]
									{
										"#mpslic.start#",
										Conversions.ToString(dateTime2.Year),
										"-",
										dateTime2.Month.ToString().PadLeft(2, '0'),
										"-",
										dateTime2.Day.ToString().PadLeft(2, '0'),
										"#mpslic.end##mpslicoriginal.start#",
										array[0],
										"#mpslicoriginal.end#"
									}), null);
									Module_MyFunctions.Config_Save("DWLlicense", my_Value, "", false, true);
									Module_Donate.Donate_Check(0);
									this.PRV_Countdown = 0;
									Application.DoEvents();
									Module_MyFunctions.my_Pause(1);
									this.TopMost = false;
									Application.DoEvents();
									Module_MyFunctions.my_MsgBox("Thank you so much, software registered!", MessageBoxIcon.Asterisk, false);
									MyProject.Forms.Form_Main.Donate_Set();
									this.Close();
								}
							}
						}
					}
				}
			}
			catch (Exception expr_28A)
			{
				ProjectData.SetProjectError(expr_28A);
				ProjectData.ClearProjectError();
			}
		}
		private void Timer_Blink_Tick(object sender, EventArgs e)
		{
			this.Timer_Blink.Enabled = false;
			checked
			{
				this.FRM_Blink++;
				if (this.Button_Skip_Donation.BackColor == Color.WhiteSmoke)
				{
					this.Button_Skip_Donation.BackColor = Color.Gold;
				}
				else
				{
					this.Button_Skip_Donation.BackColor = Color.WhiteSmoke;
				}
				if (this.FRM_Blink > 10)
				{
					this.FRM_Blink = 0;
					this.Button_Skip_Donation.BackColor = Color.WhiteSmoke;
				}
				else
				{
					this.Timer_Blink.Enabled = true;
				}
			}
		}
		private void Button_PayPalWEB_Click(object sender, EventArgs e)
		{
			Process.Start("http://www.myportablesoftware.com/donateweb.aspx");
		}
	}
}
