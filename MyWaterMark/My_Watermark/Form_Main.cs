using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using My_Watermark.My;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Windows.Forms;
namespace My_Watermark
{
	[DesignerGenerated]
	public class Form_Main : Form
	{
		private IContainer components;
		[AccessedThroughProperty("Panel_Images")]
		private Panel _Panel_Images;
		[AccessedThroughProperty("PictureBox_Preview")]
		private PictureBox _PictureBox_Preview;
		[AccessedThroughProperty("Label_Status")]
		private Label _Label_Status;
		[AccessedThroughProperty("TextBox_Watermark")]
		private TextBox _TextBox_Watermark;
		[AccessedThroughProperty("TextBox_FontSize")]
		private TextBox _TextBox_FontSize;
		[AccessedThroughProperty("Label2")]
		private Label _Label2;
		[AccessedThroughProperty("Label4")]
		private Label _Label4;
		[AccessedThroughProperty("RBP_TR")]
		private RadioButton _RBP_TR;
		[AccessedThroughProperty("RBP_TC")]
		private RadioButton _RBP_TC;
		[AccessedThroughProperty("RBP_TL")]
		private RadioButton _RBP_TL;
		[AccessedThroughProperty("ComboBox_Style")]
		private ComboBox _ComboBox_Style;
		[AccessedThroughProperty("Label5")]
		private Label _Label5;
		[AccessedThroughProperty("RBP_MR")]
		private RadioButton _RBP_MR;
		[AccessedThroughProperty("RBP_MC")]
		private RadioButton _RBP_MC;
		[AccessedThroughProperty("RBP_ML")]
		private RadioButton _RBP_ML;
		[AccessedThroughProperty("RBP_BR")]
		private RadioButton _RBP_BR;
		[AccessedThroughProperty("RBP_BC")]
		private RadioButton _RBP_BC;
		[AccessedThroughProperty("RBP_BL")]
		private RadioButton _RBP_BL;
		[AccessedThroughProperty("Label_Preview")]
		private Label _Label_Preview;
		[AccessedThroughProperty("CK_SaveFolder")]
		private CheckBox _CK_SaveFolder;
		[AccessedThroughProperty("CK_SaveSubfolder")]
		private CheckBox _CK_SaveSubfolder;
		[AccessedThroughProperty("Label7")]
		private Label _Label7;
		[AccessedThroughProperty("Button_Watermark_Image")]
		private Button _Button_Watermark_Image;
		[AccessedThroughProperty("Button_Watermark_Folder")]
		private Button _Button_Watermark_Folder;
		[AccessedThroughProperty("Button_About")]
		private Button _Button_About;
		[AccessedThroughProperty("Button_Donate")]
		private Button _Button_Donate;
		[AccessedThroughProperty("Button_Updates")]
		private Button _Button_Updates;
		[AccessedThroughProperty("FolderBrowserDialog_Img")]
		private FolderBrowserDialog _FolderBrowserDialog_Img;
		[AccessedThroughProperty("Label8")]
		private Label _Label8;
		[AccessedThroughProperty("TextBox_Directory")]
		private TextBox _TextBox_Directory;
		[AccessedThroughProperty("Button_Select_Dir")]
		private Button _Button_Select_Dir;
		[AccessedThroughProperty("Button_Refresh_Images")]
		private Button _Button_Refresh_Images;
		[AccessedThroughProperty("Label_SelDir")]
		private Label _Label_SelDir;
		[AccessedThroughProperty("Label_SelImg")]
		private Label _Label_SelImg;
		[AccessedThroughProperty("Label1")]
		private Label _Label1;
		[AccessedThroughProperty("ColorDialog_Wtm")]
		private ColorDialog _ColorDialog_Wtm;
		[AccessedThroughProperty("Label_Color")]
		private Label _Label_Color;
		[AccessedThroughProperty("CK_Shadow")]
		private CheckBox _CK_Shadow;
		[AccessedThroughProperty("Label6")]
		private Label _Label6;
		[AccessedThroughProperty("TextBox_Margin")]
		private TextBox _TextBox_Margin;
		[AccessedThroughProperty("CK_SizePixel")]
		private CheckBox _CK_SizePixel;
		[AccessedThroughProperty("CK_SizeRelative")]
		private CheckBox _CK_SizeRelative;
		[AccessedThroughProperty("ComboBox_Size")]
		private ComboBox _ComboBox_Size;
		[AccessedThroughProperty("Panel1")]
		private Panel _Panel1;
		[AccessedThroughProperty("Panel2")]
		private Panel _Panel2;
		[AccessedThroughProperty("WebBrowser_Stats")]
		private WebBrowser _WebBrowser_Stats;
		[AccessedThroughProperty("Button_BrowseFolder")]
		private Button _Button_BrowseFolder;
		[AccessedThroughProperty("Panel_Wait")]
		private Panel _Panel_Wait;
		[AccessedThroughProperty("Label_Wait")]
		private Label _Label_Wait;
		[AccessedThroughProperty("Label_Line")]
		private Label _Label_Line;
		[AccessedThroughProperty("ComboBox_FontFamily")]
		private ComboBox _ComboBox_FontFamily;
		[AccessedThroughProperty("Label3")]
		private Label _Label3;
		[AccessedThroughProperty("PictureBox_Loading")]
		private PictureBox _PictureBox_Loading;
		[AccessedThroughProperty("ComboBox_Transparency")]
		private ComboBox _ComboBox_Transparency;
		[AccessedThroughProperty("Label9")]
		private Label _Label9;
		[AccessedThroughProperty("Timer_AutoApply")]
		private System.Windows.Forms.Timer _Timer_AutoApply;
		[AccessedThroughProperty("Label_Donate")]
		private Label _Label_Donate;
		[AccessedThroughProperty("CMS_FullImage")]
		private ContextMenuStrip _CMS_FullImage;
		[AccessedThroughProperty("TSM_FU_SetWallpaper")]
		private ToolStripMenuItem _TSM_FU_SetWallpaper;
		[AccessedThroughProperty("TSM_FU_CopyImage")]
		private ToolStripMenuItem _TSM_FU_CopyImage;
		[AccessedThroughProperty("ToolStripSeparator5")]
		private ToolStripSeparator _ToolStripSeparator5;
		[AccessedThroughProperty("TSM_FU_BrowseDirectory")]
		private ToolStripMenuItem _TSM_FU_BrowseDirectory;
		[AccessedThroughProperty("CK_ThumbDB")]
		private CheckBox _CK_ThumbDB;
		[AccessedThroughProperty("Button_Clean_ThumbDB")]
		private Button _Button_Clean_ThumbDB;
		[AccessedThroughProperty("Label_Line_2")]
		private Label _Label_Line_2;
		[AccessedThroughProperty("Label_Effects")]
		private Label _Label_Effects;
		[AccessedThroughProperty("CK_Outline")]
		private CheckBox _CK_Outline;
		[AccessedThroughProperty("Label_Img_Resolution")]
		private Label _Label_Img_Resolution;
		[AccessedThroughProperty("PictureBox_Saving")]
		private PictureBox _PictureBox_Saving;
		[AccessedThroughProperty("PictureBox_Back")]
		private PictureBox _PictureBox_Back;
		[AccessedThroughProperty("PictureBox_Folder")]
		private PictureBox _PictureBox_Folder;
		[AccessedThroughProperty("ProgressBar_Wait")]
		private ProgressBar _ProgressBar_Wait;
		[AccessedThroughProperty("Button_Watermark_Block")]
		private Button _Button_Watermark_Block;
		[AccessedThroughProperty("Timer_Update")]
		private System.Windows.Forms.Timer _Timer_Update;
		[AccessedThroughProperty("Timer_Close")]
		private System.Windows.Forms.Timer _Timer_Close;
		private bool PRV_Img_Refresh;
		private Image PRV_Img_Watermark;
		private int PRV_Img_Margin;
		private bool PRV_SaveSubfolder;
		private bool PRV_SizePixel;
		private bool PRV_Form_Resizing;
		private Size FRM_Size;
		private int PRV_Timer_CKUPD;
		private ArrayList PRV_FileList;
		private string PRV_Message_Load;
		private string PRV_Message_Save;
		private string PRV_Message_Clean;
		private int PRV_AutoApply;
		private bool PRV_CK_Effects;
		private bool PRV_BLOCK_Watermark;
		private bool[] THREAD_Stop;
		private bool[] THREAD_Stopped;
		private int THREAD_Totals;
		internal virtual Panel Panel_Images
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Panel_Images;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Panel_Images = value;
			}
		}
		internal virtual PictureBox PictureBox_Preview
		{
			[DebuggerNonUserCode]
			get
			{
				return this._PictureBox_Preview;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.PictureBox_Preview_Click);
				if (this._PictureBox_Preview != null)
				{
					this._PictureBox_Preview.Click -= value2;
				}
				this._PictureBox_Preview = value;
				if (this._PictureBox_Preview != null)
				{
					this._PictureBox_Preview.Click += value2;
				}
			}
		}
		internal virtual Label Label_Status
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Status;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_Status = value;
			}
		}
		internal virtual TextBox TextBox_Watermark
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TextBox_Watermark;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.TextBox_Watermark_TextChanged);
				if (this._TextBox_Watermark != null)
				{
					this._TextBox_Watermark.TextChanged -= value2;
				}
				this._TextBox_Watermark = value;
				if (this._TextBox_Watermark != null)
				{
					this._TextBox_Watermark.TextChanged += value2;
				}
			}
		}
		internal virtual TextBox TextBox_FontSize
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TextBox_FontSize;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.TextBox_FontSize_TextChanged);
				if (this._TextBox_FontSize != null)
				{
					this._TextBox_FontSize.TextChanged -= value2;
				}
				this._TextBox_FontSize = value;
				if (this._TextBox_FontSize != null)
				{
					this._TextBox_FontSize.TextChanged += value2;
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
		internal virtual RadioButton RBP_TR
		{
			[DebuggerNonUserCode]
			get
			{
				return this._RBP_TR;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.RBP_POS_Click);
				if (this._RBP_TR != null)
				{
					this._RBP_TR.Click -= value2;
				}
				this._RBP_TR = value;
				if (this._RBP_TR != null)
				{
					this._RBP_TR.Click += value2;
				}
			}
		}
		internal virtual RadioButton RBP_TC
		{
			[DebuggerNonUserCode]
			get
			{
				return this._RBP_TC;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.RBP_POS_Click);
				if (this._RBP_TC != null)
				{
					this._RBP_TC.Click -= value2;
				}
				this._RBP_TC = value;
				if (this._RBP_TC != null)
				{
					this._RBP_TC.Click += value2;
				}
			}
		}
		internal virtual RadioButton RBP_TL
		{
			[DebuggerNonUserCode]
			get
			{
				return this._RBP_TL;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.RBP_POS_Click);
				if (this._RBP_TL != null)
				{
					this._RBP_TL.Click -= value2;
				}
				this._RBP_TL = value;
				if (this._RBP_TL != null)
				{
					this._RBP_TL.Click += value2;
				}
			}
		}
		internal virtual ComboBox ComboBox_Style
		{
			[DebuggerNonUserCode]
			get
			{
				return this._ComboBox_Style;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.ComboBox_Style_SelectedIndexChanged);
				if (this._ComboBox_Style != null)
				{
					this._ComboBox_Style.SelectedIndexChanged -= value2;
				}
				this._ComboBox_Style = value;
				if (this._ComboBox_Style != null)
				{
					this._ComboBox_Style.SelectedIndexChanged += value2;
				}
			}
		}
		internal virtual Label Label5
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label5;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label5 = value;
			}
		}
		internal virtual RadioButton RBP_MR
		{
			[DebuggerNonUserCode]
			get
			{
				return this._RBP_MR;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.RBP_POS_Click);
				if (this._RBP_MR != null)
				{
					this._RBP_MR.Click -= value2;
				}
				this._RBP_MR = value;
				if (this._RBP_MR != null)
				{
					this._RBP_MR.Click += value2;
				}
			}
		}
		internal virtual RadioButton RBP_MC
		{
			[DebuggerNonUserCode]
			get
			{
				return this._RBP_MC;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.RBP_POS_Click);
				if (this._RBP_MC != null)
				{
					this._RBP_MC.Click -= value2;
				}
				this._RBP_MC = value;
				if (this._RBP_MC != null)
				{
					this._RBP_MC.Click += value2;
				}
			}
		}
		internal virtual RadioButton RBP_ML
		{
			[DebuggerNonUserCode]
			get
			{
				return this._RBP_ML;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.RBP_POS_Click);
				if (this._RBP_ML != null)
				{
					this._RBP_ML.Click -= value2;
				}
				this._RBP_ML = value;
				if (this._RBP_ML != null)
				{
					this._RBP_ML.Click += value2;
				}
			}
		}
		internal virtual RadioButton RBP_BR
		{
			[DebuggerNonUserCode]
			get
			{
				return this._RBP_BR;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.RBP_POS_Click);
				if (this._RBP_BR != null)
				{
					this._RBP_BR.Click -= value2;
				}
				this._RBP_BR = value;
				if (this._RBP_BR != null)
				{
					this._RBP_BR.Click += value2;
				}
			}
		}
		internal virtual RadioButton RBP_BC
		{
			[DebuggerNonUserCode]
			get
			{
				return this._RBP_BC;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.RBP_POS_Click);
				if (this._RBP_BC != null)
				{
					this._RBP_BC.Click -= value2;
				}
				this._RBP_BC = value;
				if (this._RBP_BC != null)
				{
					this._RBP_BC.Click += value2;
				}
			}
		}
		internal virtual RadioButton RBP_BL
		{
			[DebuggerNonUserCode]
			get
			{
				return this._RBP_BL;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.RBP_POS_Click);
				if (this._RBP_BL != null)
				{
					this._RBP_BL.Click -= value2;
				}
				this._RBP_BL = value;
				if (this._RBP_BL != null)
				{
					this._RBP_BL.Click += value2;
				}
			}
		}
		internal virtual Label Label_Preview
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Preview;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.PictureBox_Preview_Click);
				if (this._Label_Preview != null)
				{
					this._Label_Preview.Click -= value2;
				}
				this._Label_Preview = value;
				if (this._Label_Preview != null)
				{
					this._Label_Preview.Click += value2;
				}
			}
		}
		internal virtual CheckBox CK_SaveFolder
		{
			[DebuggerNonUserCode]
			get
			{
				return this._CK_SaveFolder;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.CK_SaveFolder_Click);
				if (this._CK_SaveFolder != null)
				{
					this._CK_SaveFolder.Click -= value2;
				}
				this._CK_SaveFolder = value;
				if (this._CK_SaveFolder != null)
				{
					this._CK_SaveFolder.Click += value2;
				}
			}
		}
		internal virtual CheckBox CK_SaveSubfolder
		{
			[DebuggerNonUserCode]
			get
			{
				return this._CK_SaveSubfolder;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.CK_SaveSubfolder_Click);
				if (this._CK_SaveSubfolder != null)
				{
					this._CK_SaveSubfolder.Click -= value2;
				}
				this._CK_SaveSubfolder = value;
				if (this._CK_SaveSubfolder != null)
				{
					this._CK_SaveSubfolder.Click += value2;
				}
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
		internal virtual Button Button_Watermark_Image
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Watermark_Image;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Watermark_Image_Click);
				if (this._Button_Watermark_Image != null)
				{
					this._Button_Watermark_Image.Click -= value2;
				}
				this._Button_Watermark_Image = value;
				if (this._Button_Watermark_Image != null)
				{
					this._Button_Watermark_Image.Click += value2;
				}
			}
		}
		internal virtual Button Button_Watermark_Folder
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Watermark_Folder;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Watermark_Folder_Click);
				if (this._Button_Watermark_Folder != null)
				{
					this._Button_Watermark_Folder.Click -= value2;
				}
				this._Button_Watermark_Folder = value;
				if (this._Button_Watermark_Folder != null)
				{
					this._Button_Watermark_Folder.Click += value2;
				}
			}
		}
		internal virtual Button Button_About
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_About;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_About_Click);
				if (this._Button_About != null)
				{
					this._Button_About.Click -= value2;
				}
				this._Button_About = value;
				if (this._Button_About != null)
				{
					this._Button_About.Click += value2;
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
		internal virtual Button Button_Updates
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Updates;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Updates_Click);
				if (this._Button_Updates != null)
				{
					this._Button_Updates.Click -= value2;
				}
				this._Button_Updates = value;
				if (this._Button_Updates != null)
				{
					this._Button_Updates.Click += value2;
				}
			}
		}
		internal virtual FolderBrowserDialog FolderBrowserDialog_Img
		{
			[DebuggerNonUserCode]
			get
			{
				return this._FolderBrowserDialog_Img;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._FolderBrowserDialog_Img = value;
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
		internal virtual TextBox TextBox_Directory
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TextBox_Directory;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TextBox_Directory = value;
			}
		}
		internal virtual Button Button_Select_Dir
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Select_Dir;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Select_Dir_Click);
				if (this._Button_Select_Dir != null)
				{
					this._Button_Select_Dir.Click -= value2;
				}
				this._Button_Select_Dir = value;
				if (this._Button_Select_Dir != null)
				{
					this._Button_Select_Dir.Click += value2;
				}
			}
		}
		internal virtual Button Button_Refresh_Images
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Refresh_Images;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Refresh_Images_Click);
				if (this._Button_Refresh_Images != null)
				{
					this._Button_Refresh_Images.Click -= value2;
				}
				this._Button_Refresh_Images = value;
				if (this._Button_Refresh_Images != null)
				{
					this._Button_Refresh_Images.Click += value2;
				}
			}
		}
		internal virtual Label Label_SelDir
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_SelDir;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_SelDir = value;
			}
		}
		internal virtual Label Label_SelImg
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_SelImg;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_SelImg = value;
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
		internal virtual ColorDialog ColorDialog_Wtm
		{
			[DebuggerNonUserCode]
			get
			{
				return this._ColorDialog_Wtm;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ColorDialog_Wtm = value;
			}
		}
		internal virtual Label Label_Color
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Color;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Color_Click);
				if (this._Label_Color != null)
				{
					this._Label_Color.Click -= value2;
				}
				this._Label_Color = value;
				if (this._Label_Color != null)
				{
					this._Label_Color.Click += value2;
				}
			}
		}
		internal virtual CheckBox CK_Shadow
		{
			[DebuggerNonUserCode]
			get
			{
				return this._CK_Shadow;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.CK_Effects_CheckedChanged);
				if (this._CK_Shadow != null)
				{
					this._CK_Shadow.CheckedChanged -= value2;
				}
				this._CK_Shadow = value;
				if (this._CK_Shadow != null)
				{
					this._CK_Shadow.CheckedChanged += value2;
				}
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
		internal virtual TextBox TextBox_Margin
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TextBox_Margin;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.TextBox_Margin_TextChanged);
				if (this._TextBox_Margin != null)
				{
					this._TextBox_Margin.TextChanged -= value2;
				}
				this._TextBox_Margin = value;
				if (this._TextBox_Margin != null)
				{
					this._TextBox_Margin.TextChanged += value2;
				}
			}
		}
		internal virtual CheckBox CK_SizePixel
		{
			[DebuggerNonUserCode]
			get
			{
				return this._CK_SizePixel;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.CK_SizePixel_CheckedChanged);
				if (this._CK_SizePixel != null)
				{
					this._CK_SizePixel.Click -= value2;
				}
				this._CK_SizePixel = value;
				if (this._CK_SizePixel != null)
				{
					this._CK_SizePixel.Click += value2;
				}
			}
		}
		internal virtual CheckBox CK_SizeRelative
		{
			[DebuggerNonUserCode]
			get
			{
				return this._CK_SizeRelative;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.CK_SizeRelative_CheckedChanged);
				if (this._CK_SizeRelative != null)
				{
					this._CK_SizeRelative.Click -= value2;
				}
				this._CK_SizeRelative = value;
				if (this._CK_SizeRelative != null)
				{
					this._CK_SizeRelative.Click += value2;
				}
			}
		}
		internal virtual ComboBox ComboBox_Size
		{
			[DebuggerNonUserCode]
			get
			{
				return this._ComboBox_Size;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.ComboBox_Size_SelectedIndexChanged);
				if (this._ComboBox_Size != null)
				{
					this._ComboBox_Size.SelectedIndexChanged -= value2;
				}
				this._ComboBox_Size = value;
				if (this._ComboBox_Size != null)
				{
					this._ComboBox_Size.SelectedIndexChanged += value2;
				}
			}
		}
		internal virtual Panel Panel1
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Panel1;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Panel1 = value;
			}
		}
		internal virtual Panel Panel2
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Panel2;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Panel2 = value;
			}
		}
		internal virtual WebBrowser WebBrowser_Stats
		{
			[DebuggerNonUserCode]
			get
			{
				return this._WebBrowser_Stats;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._WebBrowser_Stats = value;
			}
		}
		internal virtual Button Button_BrowseFolder
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_BrowseFolder;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_BrowseFolder_Click);
				if (this._Button_BrowseFolder != null)
				{
					this._Button_BrowseFolder.Click -= value2;
				}
				this._Button_BrowseFolder = value;
				if (this._Button_BrowseFolder != null)
				{
					this._Button_BrowseFolder.Click += value2;
				}
			}
		}
		internal virtual Panel Panel_Wait
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Panel_Wait;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Panel_Wait = value;
			}
		}
		internal virtual Label Label_Wait
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Wait;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_Wait = value;
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
		internal virtual ComboBox ComboBox_FontFamily
		{
			[DebuggerNonUserCode]
			get
			{
				return this._ComboBox_FontFamily;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.ComboBox_FontFamily_SelectedIndexChanged);
				if (this._ComboBox_FontFamily != null)
				{
					this._ComboBox_FontFamily.SelectedIndexChanged -= value2;
				}
				this._ComboBox_FontFamily = value;
				if (this._ComboBox_FontFamily != null)
				{
					this._ComboBox_FontFamily.SelectedIndexChanged += value2;
				}
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
		internal virtual PictureBox PictureBox_Loading
		{
			[DebuggerNonUserCode]
			get
			{
				return this._PictureBox_Loading;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._PictureBox_Loading = value;
			}
		}
		internal virtual ComboBox ComboBox_Transparency
		{
			[DebuggerNonUserCode]
			get
			{
				return this._ComboBox_Transparency;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.ComboBox_Transparency_SelectedIndexChanged);
				if (this._ComboBox_Transparency != null)
				{
					this._ComboBox_Transparency.SelectedIndexChanged -= value2;
				}
				this._ComboBox_Transparency = value;
				if (this._ComboBox_Transparency != null)
				{
					this._ComboBox_Transparency.SelectedIndexChanged += value2;
				}
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
		internal virtual System.Windows.Forms.Timer Timer_AutoApply
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Timer_AutoApply;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Timer_AutoApply_Tick);
				if (this._Timer_AutoApply != null)
				{
					this._Timer_AutoApply.Tick -= value2;
				}
				this._Timer_AutoApply = value;
				if (this._Timer_AutoApply != null)
				{
					this._Timer_AutoApply.Tick += value2;
				}
			}
		}
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
		internal virtual ContextMenuStrip CMS_FullImage
		{
			[DebuggerNonUserCode]
			get
			{
				return this._CMS_FullImage;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._CMS_FullImage = value;
			}
		}
		internal virtual ToolStripMenuItem TSM_FU_SetWallpaper
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TSM_FU_SetWallpaper;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TSM_FU_SetWallpaper = value;
			}
		}
		internal virtual ToolStripMenuItem TSM_FU_CopyImage
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TSM_FU_CopyImage;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TSM_FU_CopyImage = value;
			}
		}
		internal virtual ToolStripSeparator ToolStripSeparator5
		{
			[DebuggerNonUserCode]
			get
			{
				return this._ToolStripSeparator5;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ToolStripSeparator5 = value;
			}
		}
		internal virtual ToolStripMenuItem TSM_FU_BrowseDirectory
		{
			[DebuggerNonUserCode]
			get
			{
				return this._TSM_FU_BrowseDirectory;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._TSM_FU_BrowseDirectory = value;
			}
		}
		internal virtual CheckBox CK_ThumbDB
		{
			[DebuggerNonUserCode]
			get
			{
				return this._CK_ThumbDB;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.CK_ThumbDB_Click);
				if (this._CK_ThumbDB != null)
				{
					this._CK_ThumbDB.Click -= value2;
				}
				this._CK_ThumbDB = value;
				if (this._CK_ThumbDB != null)
				{
					this._CK_ThumbDB.Click += value2;
				}
			}
		}
		internal virtual Button Button_Clean_ThumbDB
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Clean_ThumbDB;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Clean_ThumbDB_Click);
				if (this._Button_Clean_ThumbDB != null)
				{
					this._Button_Clean_ThumbDB.Click -= value2;
				}
				this._Button_Clean_ThumbDB = value;
				if (this._Button_Clean_ThumbDB != null)
				{
					this._Button_Clean_ThumbDB.Click += value2;
				}
			}
		}
		internal virtual Label Label_Line_2
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Line_2;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_Line_2 = value;
			}
		}
		internal virtual Label Label_Effects
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Effects;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_Effects = value;
			}
		}
		internal virtual CheckBox CK_Outline
		{
			[DebuggerNonUserCode]
			get
			{
				return this._CK_Outline;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.CK_Effects_CheckedChanged);
				if (this._CK_Outline != null)
				{
					this._CK_Outline.CheckedChanged -= value2;
				}
				this._CK_Outline = value;
				if (this._CK_Outline != null)
				{
					this._CK_Outline.CheckedChanged += value2;
				}
			}
		}
		internal virtual Label Label_Img_Resolution
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Label_Img_Resolution;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._Label_Img_Resolution = value;
			}
		}
		internal virtual PictureBox PictureBox_Saving
		{
			[DebuggerNonUserCode]
			get
			{
				return this._PictureBox_Saving;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._PictureBox_Saving = value;
			}
		}
		internal virtual PictureBox PictureBox_Back
		{
			[DebuggerNonUserCode]
			get
			{
				return this._PictureBox_Back;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._PictureBox_Back = value;
			}
		}
		internal virtual PictureBox PictureBox_Folder
		{
			[DebuggerNonUserCode]
			get
			{
				return this._PictureBox_Folder;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._PictureBox_Folder = value;
			}
		}
		internal virtual ProgressBar ProgressBar_Wait
		{
			[DebuggerNonUserCode]
			get
			{
				return this._ProgressBar_Wait;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ProgressBar_Wait = value;
			}
		}
		internal virtual Button Button_Watermark_Block
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Button_Watermark_Block;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Button_Watermark_Block_Click);
				if (this._Button_Watermark_Block != null)
				{
					this._Button_Watermark_Block.Click -= value2;
				}
				this._Button_Watermark_Block = value;
				if (this._Button_Watermark_Block != null)
				{
					this._Button_Watermark_Block.Click += value2;
				}
			}
		}
		internal virtual System.Windows.Forms.Timer Timer_Update
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Timer_Update;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Timer_Update_Tick);
				if (this._Timer_Update != null)
				{
					this._Timer_Update.Tick -= value2;
				}
				this._Timer_Update = value;
				if (this._Timer_Update != null)
				{
					this._Timer_Update.Tick += value2;
				}
			}
		}
		internal virtual System.Windows.Forms.Timer Timer_Close
		{
			[DebuggerNonUserCode]
			get
			{
				return this._Timer_Close;
			}
			[DebuggerNonUserCode]
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				EventHandler value2 = new EventHandler(this.Timer_Close_Tick);
				if (this._Timer_Close != null)
				{
					this._Timer_Close.Tick -= value2;
				}
				this._Timer_Close = value;
				if (this._Timer_Close != null)
				{
					this._Timer_Close.Tick += value2;
				}
			}
		}
		public Form_Main()
		{
			base.Load += new EventHandler(this.Form_Main_Load);
			base.Resize += new EventHandler(this.Form_Main_Resize);
			base.ResizeEnd += new EventHandler(this.Form_Main_ResizeEnd);
			base.ResizeBegin += new EventHandler(this.Form_Main_ResizeBegin);
			base.FormClosing += new FormClosingEventHandler(this.Form_Main_FormClosing);
			this.PRV_Timer_CKUPD = 0;
			this.PRV_FileList = new ArrayList();
			this.PRV_Message_Load = "Reading images data...\r\nplease wait!";
			this.PRV_Message_Save = "Saving images...\r\nplease wait!";
			this.PRV_Message_Clean = "Cleaning thumbnails DB\r\nplease wait!";
			this.PRV_AutoApply = 0;
			this.PRV_CK_Effects = false;
			this.THREAD_Totals = 2;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form_Main));
			this.Panel_Images = new Panel();
			this.Label_SelDir = new Label();
			this.PictureBox_Preview = new PictureBox();
			this.Label_Status = new Label();
			this.TextBox_Watermark = new TextBox();
			this.TextBox_FontSize = new TextBox();
			this.Label2 = new Label();
			this.Label4 = new Label();
			this.RBP_TR = new RadioButton();
			this.RBP_TC = new RadioButton();
			this.RBP_TL = new RadioButton();
			this.ComboBox_Style = new ComboBox();
			this.Label5 = new Label();
			this.RBP_MR = new RadioButton();
			this.RBP_MC = new RadioButton();
			this.RBP_ML = new RadioButton();
			this.RBP_BR = new RadioButton();
			this.RBP_BC = new RadioButton();
			this.RBP_BL = new RadioButton();
			this.Label_Preview = new Label();
			this.CK_SaveFolder = new CheckBox();
			this.CK_SaveSubfolder = new CheckBox();
			this.Label7 = new Label();
			this.Button_Watermark_Image = new Button();
			this.Button_Watermark_Folder = new Button();
			this.Button_About = new Button();
			this.Button_Donate = new Button();
			this.Button_Updates = new Button();
			this.FolderBrowserDialog_Img = new FolderBrowserDialog();
			this.Label8 = new Label();
			this.TextBox_Directory = new TextBox();
			this.Button_Select_Dir = new Button();
			this.Button_Refresh_Images = new Button();
			this.Label_SelImg = new Label();
			this.Label1 = new Label();
			this.ColorDialog_Wtm = new ColorDialog();
			this.Label_Color = new Label();
			this.CK_Shadow = new CheckBox();
			this.Label6 = new Label();
			this.TextBox_Margin = new TextBox();
			this.CK_SizePixel = new CheckBox();
			this.CK_SizeRelative = new CheckBox();
			this.ComboBox_Size = new ComboBox();
			this.Panel1 = new Panel();
			this.Label_Effects = new Label();
			this.CK_Outline = new CheckBox();
			this.ComboBox_FontFamily = new ComboBox();
			this.Label3 = new Label();
			this.Panel2 = new Panel();
			this.ComboBox_Transparency = new ComboBox();
			this.Label9 = new Label();
			this.WebBrowser_Stats = new WebBrowser();
			this.Button_BrowseFolder = new Button();
			this.Panel_Wait = new Panel();
			this.ProgressBar_Wait = new ProgressBar();
			this.Label_Wait = new Label();
			this.Label_Line = new Label();
			this.PictureBox_Loading = new PictureBox();
			this.Timer_AutoApply = new System.Windows.Forms.Timer(this.components);
			this.Label_Donate = new Label();
			this.CMS_FullImage = new ContextMenuStrip(this.components);
			this.TSM_FU_SetWallpaper = new ToolStripMenuItem();
			this.TSM_FU_CopyImage = new ToolStripMenuItem();
			this.ToolStripSeparator5 = new ToolStripSeparator();
			this.TSM_FU_BrowseDirectory = new ToolStripMenuItem();
			this.CK_ThumbDB = new CheckBox();
			this.Button_Clean_ThumbDB = new Button();
			this.Label_Line_2 = new Label();
			this.Label_Img_Resolution = new Label();
			this.PictureBox_Saving = new PictureBox();
			this.PictureBox_Back = new PictureBox();
			this.PictureBox_Folder = new PictureBox();
			this.Button_Watermark_Block = new Button();
			this.Timer_Update = new System.Windows.Forms.Timer(this.components);
			this.Timer_Close = new System.Windows.Forms.Timer(this.components);
			((ISupportInitialize)this.PictureBox_Preview).BeginInit();
			this.Panel1.SuspendLayout();
			this.Panel2.SuspendLayout();
			this.Panel_Wait.SuspendLayout();
			((ISupportInitialize)this.PictureBox_Loading).BeginInit();
			this.CMS_FullImage.SuspendLayout();
			((ISupportInitialize)this.PictureBox_Saving).BeginInit();
			((ISupportInitialize)this.PictureBox_Back).BeginInit();
			((ISupportInitialize)this.PictureBox_Folder).BeginInit();
			this.SuspendLayout();
			this.Panel_Images.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.Panel_Images.BorderStyle = BorderStyle.FixedSingle;
			Control arg_3FF_0 = this.Panel_Images;
			Point location = new Point(3, 27);
			arg_3FF_0.Location = location;
			this.Panel_Images.Name = "Panel_Images";
			Control arg_42C_0 = this.Panel_Images;
			Size size = new Size(777, 231);
			arg_42C_0.Size = size;
			this.Panel_Images.TabIndex = 2;
			this.Label_SelDir.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.Label_SelDir.BackColor = Color.Transparent;
			this.Label_SelDir.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_SelDir.ForeColor = Color.Black;
			Control arg_498_0 = this.Label_SelDir;
			location = new Point(8, 58);
			arg_498_0.Location = location;
			this.Label_SelDir.Name = "Label_SelDir";
			Control arg_4C5_0 = this.Label_SelDir;
			size = new Size(763, 155);
			arg_4C5_0.Size = size;
			this.Label_SelDir.TabIndex = 38;
			this.Label_SelDir.Text = "Select a directory";
			this.Label_SelDir.TextAlign = ContentAlignment.MiddleCenter;
			this.PictureBox_Preview.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.PictureBox_Preview.BorderStyle = BorderStyle.FixedSingle;
			this.PictureBox_Preview.Cursor = Cursors.Hand;
			Control arg_530_0 = this.PictureBox_Preview;
			location = new Point(3, 280);
			arg_530_0.Location = location;
			this.PictureBox_Preview.Name = "PictureBox_Preview";
			Control arg_55D_0 = this.PictureBox_Preview;
			size = new Size(240, 180);
			arg_55D_0.Size = size;
			this.PictureBox_Preview.SizeMode = PictureBoxSizeMode.StretchImage;
			this.PictureBox_Preview.TabIndex = 3;
			this.PictureBox_Preview.TabStop = false;
			this.Label_Status.BackColor = Color.YellowGreen;
			this.Label_Status.BorderStyle = BorderStyle.FixedSingle;
			this.Label_Status.ForeColor = Color.Black;
			Control arg_5C2_0 = this.Label_Status;
			location = new Point(3, 7);
			arg_5C2_0.Location = location;
			this.Label_Status.Name = "Label_Status";
			Control arg_5EC_0 = this.Label_Status;
			size = new Size(246, 21);
			arg_5EC_0.Size = size;
			this.Label_Status.TabIndex = 4;
			this.Label_Status.TextAlign = ContentAlignment.MiddleLeft;
			this.TextBox_Watermark.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.TextBox_Watermark.ForeColor = Color.Black;
			Control arg_648_0 = this.TextBox_Watermark;
			location = new Point(65, 3);
			arg_648_0.Location = location;
			this.TextBox_Watermark.Multiline = true;
			this.TextBox_Watermark.Name = "TextBox_Watermark";
			this.TextBox_Watermark.ScrollBars = ScrollBars.Vertical;
			Control arg_68A_0 = this.TextBox_Watermark;
			size = new Size(285, 51);
			arg_68A_0.Size = size;
			this.TextBox_Watermark.TabIndex = 5;
			this.TextBox_Watermark.Text = "your text here";
			this.TextBox_FontSize.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.TextBox_FontSize.ForeColor = Color.Black;
			Control arg_6ED_0 = this.TextBox_FontSize;
			location = new Point(146, 83);
			arg_6ED_0.Location = location;
			this.TextBox_FontSize.Name = "TextBox_FontSize";
			Control arg_714_0 = this.TextBox_FontSize;
			size = new Size(32, 20);
			arg_714_0.Size = size;
			this.TextBox_FontSize.TabIndex = 9;
			this.TextBox_FontSize.Text = "80";
			this.Label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label2.ForeColor = Color.Black;
			Control arg_774_0 = this.Label2;
			location = new Point(-11, 2);
			arg_774_0.Location = location;
			this.Label2.Name = "Label2";
			Control arg_79B_0 = this.Label2;
			size = new Size(73, 20);
			arg_79B_0.Size = size;
			this.Label2.TabIndex = 10;
			this.Label2.Text = "Watermark";
			this.Label2.TextAlign = ContentAlignment.MiddleRight;
			this.Label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label4.ForeColor = Color.Black;
			Control arg_80C_0 = this.Label4;
			location = new Point(220, 57);
			arg_80C_0.Location = location;
			this.Label4.Name = "Label4";
			Control arg_833_0 = this.Label4;
			size = new Size(42, 20);
			arg_833_0.Size = size;
			this.Label4.TabIndex = 12;
			this.Label4.Text = "Style";
			this.Label4.TextAlign = ContentAlignment.MiddleRight;
			this.RBP_TR.AutoSize = true;
			this.RBP_TR.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.RBP_TR.ForeColor = Color.Black;
			Control arg_8AF_0 = this.RBP_TR;
			location = new Point(145, 8);
			arg_8AF_0.Location = location;
			this.RBP_TR.Name = "RBP_TR";
			Control arg_8D6_0 = this.RBP_TR;
			size = new Size(14, 13);
			arg_8D6_0.Size = size;
			this.RBP_TR.TabIndex = 15;
			this.RBP_TR.TabStop = true;
			this.RBP_TR.Tag = "TR";
			this.RBP_TR.UseVisualStyleBackColor = true;
			this.RBP_TC.AutoSize = true;
			this.RBP_TC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.RBP_TC.ForeColor = Color.Black;
			Control arg_95A_0 = this.RBP_TC;
			location = new Point(104, 8);
			arg_95A_0.Location = location;
			this.RBP_TC.Name = "RBP_TC";
			Control arg_981_0 = this.RBP_TC;
			size = new Size(14, 13);
			arg_981_0.Size = size;
			this.RBP_TC.TabIndex = 14;
			this.RBP_TC.TabStop = true;
			this.RBP_TC.Tag = "TC";
			this.RBP_TC.UseVisualStyleBackColor = true;
			this.RBP_TL.AutoSize = true;
			this.RBP_TL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.RBP_TL.ForeColor = Color.Black;
			Control arg_A05_0 = this.RBP_TL;
			location = new Point(63, 8);
			arg_A05_0.Location = location;
			this.RBP_TL.Name = "RBP_TL";
			Control arg_A2C_0 = this.RBP_TL;
			size = new Size(14, 13);
			arg_A2C_0.Size = size;
			this.RBP_TL.TabIndex = 13;
			this.RBP_TL.TabStop = true;
			this.RBP_TL.Tag = "TL";
			this.RBP_TL.UseVisualStyleBackColor = true;
			this.ComboBox_Style.DropDownStyle = ComboBoxStyle.DropDownList;
			this.ComboBox_Style.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ComboBox_Style.ForeColor = Color.Black;
			this.ComboBox_Style.FormattingEnabled = true;
			Control arg_AC0_0 = this.ComboBox_Style;
			location = new Point(265, 58);
			arg_AC0_0.Location = location;
			this.ComboBox_Style.Name = "ComboBox_Style";
			Control arg_AE7_0 = this.ComboBox_Style;
			size = new Size(85, 21);
			arg_AE7_0.Size = size;
			this.ComboBox_Style.TabIndex = 16;
			this.Label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label5.ForeColor = Color.Black;
			Control arg_B36_0 = this.Label5;
			location = new Point(5, 4);
			arg_B36_0.Location = location;
			this.Label5.Name = "Label5";
			Control arg_B5D_0 = this.Label5;
			size = new Size(49, 20);
			arg_B5D_0.Size = size;
			this.Label5.TabIndex = 17;
			this.Label5.Text = "Position";
			this.Label5.TextAlign = ContentAlignment.MiddleRight;
			this.RBP_MR.AutoSize = true;
			this.RBP_MR.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.RBP_MR.ForeColor = Color.Black;
			Control arg_BDA_0 = this.RBP_MR;
			location = new Point(145, 31);
			arg_BDA_0.Location = location;
			this.RBP_MR.Name = "RBP_MR";
			Control arg_C01_0 = this.RBP_MR;
			size = new Size(14, 13);
			arg_C01_0.Size = size;
			this.RBP_MR.TabIndex = 20;
			this.RBP_MR.TabStop = true;
			this.RBP_MR.Tag = "MR";
			this.RBP_MR.UseVisualStyleBackColor = true;
			this.RBP_MC.AutoSize = true;
			this.RBP_MC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.RBP_MC.ForeColor = Color.Black;
			Control arg_C86_0 = this.RBP_MC;
			location = new Point(104, 31);
			arg_C86_0.Location = location;
			this.RBP_MC.Name = "RBP_MC";
			Control arg_CAD_0 = this.RBP_MC;
			size = new Size(14, 13);
			arg_CAD_0.Size = size;
			this.RBP_MC.TabIndex = 19;
			this.RBP_MC.TabStop = true;
			this.RBP_MC.Tag = "MC";
			this.RBP_MC.UseVisualStyleBackColor = true;
			this.RBP_ML.AutoSize = true;
			this.RBP_ML.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.RBP_ML.ForeColor = Color.Black;
			Control arg_D32_0 = this.RBP_ML;
			location = new Point(63, 31);
			arg_D32_0.Location = location;
			this.RBP_ML.Name = "RBP_ML";
			Control arg_D59_0 = this.RBP_ML;
			size = new Size(14, 13);
			arg_D59_0.Size = size;
			this.RBP_ML.TabIndex = 18;
			this.RBP_ML.TabStop = true;
			this.RBP_ML.Tag = "ML";
			this.RBP_ML.UseVisualStyleBackColor = true;
			this.RBP_BR.AutoSize = true;
			this.RBP_BR.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.RBP_BR.ForeColor = Color.Black;
			Control arg_DE1_0 = this.RBP_BR;
			location = new Point(145, 54);
			arg_DE1_0.Location = location;
			this.RBP_BR.Name = "RBP_BR";
			Control arg_E08_0 = this.RBP_BR;
			size = new Size(14, 13);
			arg_E08_0.Size = size;
			this.RBP_BR.TabIndex = 23;
			this.RBP_BR.TabStop = true;
			this.RBP_BR.Tag = "BR";
			this.RBP_BR.UseVisualStyleBackColor = true;
			this.RBP_BC.AutoSize = true;
			this.RBP_BC.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.RBP_BC.ForeColor = Color.Black;
			Control arg_E8D_0 = this.RBP_BC;
			location = new Point(104, 54);
			arg_E8D_0.Location = location;
			this.RBP_BC.Name = "RBP_BC";
			Control arg_EB4_0 = this.RBP_BC;
			size = new Size(14, 13);
			arg_EB4_0.Size = size;
			this.RBP_BC.TabIndex = 22;
			this.RBP_BC.TabStop = true;
			this.RBP_BC.Tag = "BC";
			this.RBP_BC.UseVisualStyleBackColor = true;
			this.RBP_BL.AutoSize = true;
			this.RBP_BL.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.RBP_BL.ForeColor = Color.Black;
			Control arg_F39_0 = this.RBP_BL;
			location = new Point(63, 54);
			arg_F39_0.Location = location;
			this.RBP_BL.Name = "RBP_BL";
			Control arg_F60_0 = this.RBP_BL;
			size = new Size(14, 13);
			arg_F60_0.Size = size;
			this.RBP_BL.TabIndex = 21;
			this.RBP_BL.TabStop = true;
			this.RBP_BL.Tag = "BL";
			this.RBP_BL.UseVisualStyleBackColor = true;
			this.Label_Preview.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Label_Preview.BackColor = Color.YellowGreen;
			this.Label_Preview.BorderStyle = BorderStyle.FixedSingle;
			this.Label_Preview.ForeColor = Color.Black;
			Control arg_FE6_0 = this.Label_Preview;
			location = new Point(3, 260);
			arg_FE6_0.Location = location;
			this.Label_Preview.Name = "Label_Preview";
			Control arg_1010_0 = this.Label_Preview;
			size = new Size(240, 21);
			arg_1010_0.Size = size;
			this.Label_Preview.TabIndex = 24;
			this.Label_Preview.Text = "Preview";
			this.Label_Preview.TextAlign = ContentAlignment.MiddleLeft;
			this.CK_SaveFolder.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.CK_SaveFolder.AutoSize = true;
			this.CK_SaveFolder.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.CK_SaveFolder.ForeColor = Color.Black;
			Control arg_109C_0 = this.CK_SaveFolder;
			location = new Point(382, 417);
			arg_109C_0.Location = location;
			this.CK_SaveFolder.Name = "CK_SaveFolder";
			Control arg_10C6_0 = this.CK_SaveFolder;
			size = new Size(165, 17);
			arg_10C6_0.Size = size;
			this.CK_SaveFolder.TabIndex = 25;
			this.CK_SaveFolder.Text = "same foder (filename_wm.jpg)";
			this.CK_SaveFolder.UseVisualStyleBackColor = true;
			this.CK_SaveSubfolder.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.CK_SaveSubfolder.AutoSize = true;
			this.CK_SaveSubfolder.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.CK_SaveSubfolder.ForeColor = Color.Black;
			Control arg_1151_0 = this.CK_SaveSubfolder;
			location = new Point(382, 442);
			arg_1151_0.Location = location;
			this.CK_SaveSubfolder.Name = "CK_SaveSubfolder";
			Control arg_117B_0 = this.CK_SaveSubfolder;
			size = new Size(182, 17);
			arg_117B_0.Size = size;
			this.CK_SaveSubfolder.TabIndex = 26;
			this.CK_SaveSubfolder.Text = "subfolder (original_dir\\watermark)";
			this.CK_SaveSubfolder.UseVisualStyleBackColor = true;
			this.Label7.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label7.ForeColor = Color.Black;
			Control arg_11FA_0 = this.Label7;
			location = new Point(260, 414);
			arg_11FA_0.Location = location;
			this.Label7.Name = "Label7";
			Control arg_1221_0 = this.Label7;
			size = new Size(118, 20);
			arg_1221_0.Size = size;
			this.Label7.TabIndex = 27;
			this.Label7.Text = "Save new image(s) in:";
			this.Label7.TextAlign = ContentAlignment.MiddleLeft;
			this.Button_Watermark_Image.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Button_Watermark_Image.Cursor = Cursors.Hand;
			this.Button_Watermark_Image.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Button_Watermark_Image.ForeColor = Color.Black;
			Control arg_12B1_0 = this.Button_Watermark_Image;
			location = new Point(607, 415);
			arg_12B1_0.Location = location;
			this.Button_Watermark_Image.Name = "Button_Watermark_Image";
			Control arg_12DB_0 = this.Button_Watermark_Image;
			size = new Size(171, 23);
			arg_12DB_0.Size = size;
			this.Button_Watermark_Image.TabIndex = 28;
			this.Button_Watermark_Image.Text = "Watermark selected image";
			this.Button_Watermark_Image.UseVisualStyleBackColor = true;
			this.Button_Watermark_Folder.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Button_Watermark_Folder.Cursor = Cursors.Hand;
			this.Button_Watermark_Folder.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Button_Watermark_Folder.ForeColor = Color.Black;
			Control arg_136A_0 = this.Button_Watermark_Folder;
			location = new Point(607, 440);
			arg_136A_0.Location = location;
			this.Button_Watermark_Folder.Name = "Button_Watermark_Folder";
			Control arg_1394_0 = this.Button_Watermark_Folder;
			size = new Size(171, 23);
			arg_1394_0.Size = size;
			this.Button_Watermark_Folder.TabIndex = 29;
			this.Button_Watermark_Folder.Text = "Watermark all images in folder";
			this.Button_Watermark_Folder.UseVisualStyleBackColor = true;
			this.Button_About.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Button_About.Cursor = Cursors.Hand;
			this.Button_About.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Button_About.ForeColor = Color.Black;
			Control arg_1423_0 = this.Button_About;
			location = new Point(715, 501);
			arg_1423_0.Location = location;
			this.Button_About.Name = "Button_About";
			Control arg_144A_0 = this.Button_About;
			size = new Size(63, 23);
			arg_144A_0.Size = size;
			this.Button_About.TabIndex = 30;
			this.Button_About.Text = "About";
			this.Button_About.UseVisualStyleBackColor = true;
			this.Button_Donate.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Button_Donate.Cursor = Cursors.Hand;
			this.Button_Donate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Button_Donate.ForeColor = Color.Black;
			Control arg_14D6_0 = this.Button_Donate;
			location = new Point(55, 499);
			arg_14D6_0.Location = location;
			this.Button_Donate.Name = "Button_Donate";
			Control arg_1500_0 = this.Button_Donate;
			size = new Size(138, 23);
			arg_1500_0.Size = size;
			this.Button_Donate.TabIndex = 31;
			this.Button_Donate.Text = "Donate and register";
			this.Button_Donate.UseVisualStyleBackColor = true;
			this.Button_Updates.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Button_Updates.Cursor = Cursors.Hand;
			this.Button_Updates.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Button_Updates.ForeColor = Color.Black;
			Control arg_158F_0 = this.Button_Updates;
			location = new Point(248, 501);
			arg_158F_0.Location = location;
			this.Button_Updates.Name = "Button_Updates";
			Control arg_15B9_0 = this.Button_Updates;
			size = new Size(195, 23);
			arg_15B9_0.Size = size;
			this.Button_Updates.TabIndex = 33;
			this.Button_Updates.Text = "Verify new software versions";
			this.Button_Updates.UseVisualStyleBackColor = true;
			this.FolderBrowserDialog_Img.RootFolder = Environment.SpecialFolder.MyComputer;
			this.Label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label8.ForeColor = Color.Black;
			Control arg_1635_0 = this.Label8;
			location = new Point(252, 4);
			arg_1635_0.Location = location;
			this.Label8.Name = "Label8";
			Control arg_165C_0 = this.Label8;
			size = new Size(93, 20);
			arg_165C_0.Size = size;
			this.Label8.TabIndex = 35;
			this.Label8.Text = "Images directory";
			this.Label8.TextAlign = ContentAlignment.MiddleRight;
			this.TextBox_Directory.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.TextBox_Directory.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.TextBox_Directory.ForeColor = Color.Black;
			Control arg_16D9_0 = this.TextBox_Directory;
			location = new Point(349, 5);
			arg_16D9_0.Location = location;
			this.TextBox_Directory.Name = "TextBox_Directory";
			Control arg_1703_0 = this.TextBox_Directory;
			size = new Size(306, 20);
			arg_1703_0.Size = size;
			this.TextBox_Directory.TabIndex = 34;
			this.TextBox_Directory.Text = "C:\\";
			this.Button_Select_Dir.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.Button_Select_Dir.Cursor = Cursors.Hand;
			this.Button_Select_Dir.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Button_Select_Dir.ForeColor = Color.Black;
			Control arg_1783_0 = this.Button_Select_Dir;
			location = new Point(661, 4);
			arg_1783_0.Location = location;
			this.Button_Select_Dir.Name = "Button_Select_Dir";
			Control arg_17AA_0 = this.Button_Select_Dir;
			size = new Size(57, 23);
			arg_17AA_0.Size = size;
			this.Button_Select_Dir.TabIndex = 36;
			this.Button_Select_Dir.Text = "Select";
			this.Button_Select_Dir.UseVisualStyleBackColor = true;
			this.Button_Refresh_Images.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.Button_Refresh_Images.Cursor = Cursors.Hand;
			this.Button_Refresh_Images.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Button_Refresh_Images.ForeColor = Color.Black;
			Control arg_1836_0 = this.Button_Refresh_Images;
			location = new Point(724, 4);
			arg_1836_0.Location = location;
			this.Button_Refresh_Images.Name = "Button_Refresh_Images";
			Control arg_185D_0 = this.Button_Refresh_Images;
			size = new Size(56, 23);
			arg_185D_0.Size = size;
			this.Button_Refresh_Images.TabIndex = 37;
			this.Button_Refresh_Images.Text = "Refresh";
			this.Button_Refresh_Images.UseVisualStyleBackColor = true;
			this.Label_SelImg.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Label_SelImg.BackColor = Color.Transparent;
			this.Label_SelImg.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_SelImg.ForeColor = Color.Black;
			Control arg_18E9_0 = this.Label_SelImg;
			location = new Point(10, 331);
			arg_18E9_0.Location = location;
			this.Label_SelImg.Name = "Label_SelImg";
			Control arg_1913_0 = this.Label_SelImg;
			size = new Size(226, 81);
			arg_1913_0.Size = size;
			this.Label_SelImg.TabIndex = 39;
			this.Label_SelImg.Text = "Select an image";
			this.Label_SelImg.TextAlign = ContentAlignment.MiddleCenter;
			this.Label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label1.ForeColor = Color.Black;
			Control arg_1980_0 = this.Label1;
			location = new Point(-1, 79);
			arg_1980_0.Location = location;
			this.Label1.Name = "Label1";
			Control arg_19A7_0 = this.Label1;
			size = new Size(38, 20);
			arg_19A7_0.Size = size;
			this.Label1.TabIndex = 41;
			this.Label1.Text = "Color";
			this.Label1.TextAlign = ContentAlignment.MiddleRight;
			this.Label_Color.BackColor = Color.White;
			this.Label_Color.BorderStyle = BorderStyle.FixedSingle;
			this.Label_Color.Cursor = Cursors.Hand;
			this.Label_Color.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_Color.ForeColor = Color.Black;
			Control arg_1A41_0 = this.Label_Color;
			location = new Point(40, 79);
			arg_1A41_0.Location = location;
			this.Label_Color.Name = "Label_Color";
			Control arg_1A68_0 = this.Label_Color;
			size = new Size(127, 20);
			arg_1A68_0.Size = size;
			this.Label_Color.TabIndex = 42;
			this.Label_Color.TextAlign = ContentAlignment.MiddleRight;
			this.CK_Shadow.AutoSize = true;
			this.CK_Shadow.BackColor = Color.Transparent;
			this.CK_Shadow.Checked = true;
			this.CK_Shadow.CheckState = CheckState.Checked;
			this.CK_Shadow.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.CK_Shadow.ForeColor = Color.Black;
			Control arg_1AFA_0 = this.CK_Shadow;
			location = new Point(66, 108);
			arg_1AFA_0.Location = location;
			this.CK_Shadow.Name = "CK_Shadow";
			Control arg_1B21_0 = this.CK_Shadow;
			size = new Size(63, 17);
			arg_1B21_0.Size = size;
			this.CK_Shadow.TabIndex = 43;
			this.CK_Shadow.Text = "shadow";
			this.CK_Shadow.UseVisualStyleBackColor = false;
			this.Label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label6.ForeColor = Color.Black;
			Control arg_1B91_0 = this.Label6;
			location = new Point(240, 105);
			arg_1B91_0.Location = location;
			this.Label6.Name = "Label6";
			Control arg_1BB8_0 = this.Label6;
			size = new Size(72, 20);
			arg_1BB8_0.Size = size;
			this.Label6.TabIndex = 45;
			this.Label6.Text = "Margin (pixel)";
			this.Label6.TextAlign = ContentAlignment.MiddleRight;
			this.TextBox_Margin.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.TextBox_Margin.ForeColor = Color.Black;
			Control arg_1C29_0 = this.TextBox_Margin;
			location = new Point(318, 106);
			arg_1C29_0.Location = location;
			this.TextBox_Margin.Name = "TextBox_Margin";
			Control arg_1C50_0 = this.TextBox_Margin;
			size = new Size(32, 20);
			arg_1C50_0.Size = size;
			this.TextBox_Margin.TabIndex = 44;
			this.TextBox_Margin.Text = "10";
			this.CK_SizePixel.AutoSize = true;
			this.CK_SizePixel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.CK_SizePixel.ForeColor = Color.Black;
			Control arg_1CBD_0 = this.CK_SizePixel;
			location = new Point(66, 85);
			arg_1CBD_0.Location = location;
			this.CK_SizePixel.Name = "CK_SizePixel";
			Control arg_1CE4_0 = this.CK_SizePixel;
			size = new Size(74, 17);
			arg_1CE4_0.Size = size;
			this.CK_SizePixel.TabIndex = 46;
			this.CK_SizePixel.Text = "size (pixel)";
			this.CK_SizePixel.UseVisualStyleBackColor = true;
			this.CK_SizeRelative.AutoSize = true;
			this.CK_SizeRelative.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.CK_SizeRelative.ForeColor = Color.Black;
			Control arg_1D60_0 = this.CK_SizeRelative;
			location = new Point(202, 84);
			arg_1D60_0.Location = location;
			this.CK_SizeRelative.Name = "CK_SizeRelative";
			Control arg_1D87_0 = this.CK_SizeRelative;
			size = new Size(87, 17);
			arg_1D87_0.Size = size;
			this.CK_SizeRelative.TabIndex = 47;
			this.CK_SizeRelative.Text = "size (relative)";
			this.CK_SizeRelative.UseVisualStyleBackColor = true;
			this.ComboBox_Size.DropDownStyle = ComboBoxStyle.DropDownList;
			this.ComboBox_Size.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ComboBox_Size.ForeColor = Color.Black;
			this.ComboBox_Size.FormattingEnabled = true;
			this.ComboBox_Size.Items.AddRange(new object[]
			{
				"30%"
			});
			Control arg_1E2F_0 = this.ComboBox_Size;
			location = new Point(295, 82);
			arg_1E2F_0.Location = location;
			this.ComboBox_Size.Name = "ComboBox_Size";
			Control arg_1E56_0 = this.ComboBox_Size;
			size = new Size(55, 21);
			arg_1E56_0.Size = size;
			this.ComboBox_Size.TabIndex = 48;
			this.Panel1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Panel1.BorderStyle = BorderStyle.FixedSingle;
			this.Panel1.Controls.Add(this.Label_Effects);
			this.Panel1.Controls.Add(this.CK_Outline);
			this.Panel1.Controls.Add(this.ComboBox_FontFamily);
			this.Panel1.Controls.Add(this.Label3);
			this.Panel1.Controls.Add(this.CK_Shadow);
			this.Panel1.Controls.Add(this.ComboBox_Style);
			this.Panel1.Controls.Add(this.ComboBox_Size);
			this.Panel1.Controls.Add(this.TextBox_Watermark);
			this.Panel1.Controls.Add(this.CK_SizeRelative);
			this.Panel1.Controls.Add(this.TextBox_FontSize);
			this.Panel1.Controls.Add(this.CK_SizePixel);
			this.Panel1.Controls.Add(this.Label2);
			this.Panel1.Controls.Add(this.Label6);
			this.Panel1.Controls.Add(this.Label4);
			this.Panel1.Controls.Add(this.TextBox_Margin);
			Control arg_1FE2_0 = this.Panel1;
			location = new Point(248, 260);
			arg_1FE2_0.Location = location;
			this.Panel1.Name = "Panel1";
			Control arg_200F_0 = this.Panel1;
			size = new Size(355, 152);
			arg_200F_0.Size = size;
			this.Panel1.TabIndex = 49;
			this.Label_Effects.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_Effects.ForeColor = Color.Black;
			Control arg_2063_0 = this.Label_Effects;
			location = new Point(63, 129);
			arg_2063_0.Location = location;
			this.Label_Effects.Name = "Label_Effects";
			Control arg_208D_0 = this.Label_Effects;
			size = new Size(287, 19);
			arg_208D_0.Size = size;
			this.Label_Effects.TabIndex = 52;
			this.Label_Effects.Text = "Effects enabled";
			this.CK_Outline.AutoSize = true;
			this.CK_Outline.BackColor = Color.Transparent;
			this.CK_Outline.Checked = true;
			this.CK_Outline.CheckState = CheckState.Checked;
			this.CK_Outline.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.CK_Outline.ForeColor = Color.Black;
			Control arg_2125_0 = this.CK_Outline;
			location = new Point(146, 108);
			arg_2125_0.Location = location;
			this.CK_Outline.Name = "CK_Outline";
			Control arg_214C_0 = this.CK_Outline;
			size = new Size(57, 17);
			arg_214C_0.Size = size;
			this.CK_Outline.TabIndex = 51;
			this.CK_Outline.Text = "outline";
			this.CK_Outline.UseVisualStyleBackColor = false;
			this.ComboBox_FontFamily.DropDownStyle = ComboBoxStyle.DropDownList;
			this.ComboBox_FontFamily.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ComboBox_FontFamily.ForeColor = Color.Black;
			this.ComboBox_FontFamily.FormattingEnabled = true;
			Control arg_21D1_0 = this.ComboBox_FontFamily;
			location = new Point(65, 57);
			arg_21D1_0.Location = location;
			this.ComboBox_FontFamily.Name = "ComboBox_FontFamily";
			Control arg_21FB_0 = this.ComboBox_FontFamily;
			size = new Size(164, 21);
			arg_21FB_0.Size = size;
			this.ComboBox_FontFamily.TabIndex = 50;
			this.Label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label3.ForeColor = Color.Black;
			Control arg_224C_0 = this.Label3;
			location = new Point(23, 57);
			arg_224C_0.Location = location;
			this.Label3.Name = "Label3";
			Control arg_2273_0 = this.Label3;
			size = new Size(39, 20);
			arg_2273_0.Size = size;
			this.Label3.TabIndex = 49;
			this.Label3.Text = "Font";
			this.Label3.TextAlign = ContentAlignment.MiddleRight;
			this.Panel2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Panel2.BorderStyle = BorderStyle.FixedSingle;
			this.Panel2.Controls.Add(this.ComboBox_Transparency);
			this.Panel2.Controls.Add(this.Label9);
			this.Panel2.Controls.Add(this.RBP_TL);
			this.Panel2.Controls.Add(this.RBP_TC);
			this.Panel2.Controls.Add(this.Label_Color);
			this.Panel2.Controls.Add(this.RBP_TR);
			this.Panel2.Controls.Add(this.Label1);
			this.Panel2.Controls.Add(this.Label5);
			this.Panel2.Controls.Add(this.RBP_ML);
			this.Panel2.Controls.Add(this.RBP_MC);
			this.Panel2.Controls.Add(this.RBP_MR);
			this.Panel2.Controls.Add(this.RBP_BL);
			this.Panel2.Controls.Add(this.RBP_BC);
			this.Panel2.Controls.Add(this.RBP_BR);
			Control arg_2406_0 = this.Panel2;
			location = new Point(605, 260);
			arg_2406_0.Location = location;
			this.Panel2.Name = "Panel2";
			Control arg_2433_0 = this.Panel2;
			size = new Size(174, 152);
			arg_2433_0.Size = size;
			this.Panel2.TabIndex = 39;
			this.ComboBox_Transparency.DropDownStyle = ComboBoxStyle.DropDownList;
			this.ComboBox_Transparency.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ComboBox_Transparency.ForeColor = Color.Black;
			this.ComboBox_Transparency.FormattingEnabled = true;
			Control arg_249C_0 = this.ComboBox_Transparency;
			location = new Point(82, 107);
			arg_249C_0.Location = location;
			this.ComboBox_Transparency.Name = "ComboBox_Transparency";
			Control arg_24C3_0 = this.ComboBox_Transparency;
			size = new Size(85, 21);
			arg_24C3_0.Size = size;
			this.ComboBox_Transparency.TabIndex = 45;
			this.Label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label9.ForeColor = Color.Black;
			Control arg_2514_0 = this.Label9;
			location = new Point(-4, 106);
			arg_2514_0.Location = location;
			this.Label9.Name = "Label9";
			Control arg_253B_0 = this.Label9;
			size = new Size(82, 20);
			arg_253B_0.Size = size;
			this.Label9.TabIndex = 44;
			this.Label9.Text = "Transparency";
			this.Label9.TextAlign = ContentAlignment.MiddleRight;
			this.WebBrowser_Stats.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			Control arg_258E_0 = this.WebBrowser_Stats;
			location = new Point(246, 387);
			arg_258E_0.Location = location;
			Control arg_25A3_0 = this.WebBrowser_Stats;
			Padding margin = new Padding(0);
			arg_25A3_0.Margin = margin;
			Control arg_25BA_0 = this.WebBrowser_Stats;
			size = new Size(20, 20);
			arg_25BA_0.MinimumSize = size;
			this.WebBrowser_Stats.Name = "WebBrowser_Stats";
			this.WebBrowser_Stats.ScrollBarsEnabled = false;
			Control arg_25ED_0 = this.WebBrowser_Stats;
			size = new Size(48, 21);
			arg_25ED_0.Size = size;
			this.WebBrowser_Stats.TabIndex = 119;
			this.WebBrowser_Stats.Visible = false;
			this.Button_BrowseFolder.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Button_BrowseFolder.Cursor = Cursors.Hand;
			this.Button_BrowseFolder.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Button_BrowseFolder.ForeColor = Color.Black;
			Control arg_266C_0 = this.Button_BrowseFolder;
			location = new Point(269, 436);
			arg_266C_0.Location = location;
			this.Button_BrowseFolder.Name = "Button_BrowseFolder";
			Control arg_2693_0 = this.Button_BrowseFolder;
			size = new Size(87, 23);
			arg_2693_0.Size = size;
			this.Button_BrowseFolder.TabIndex = 120;
			this.Button_BrowseFolder.Text = "Open folder";
			this.Button_BrowseFolder.UseVisualStyleBackColor = true;
			this.Panel_Wait.Anchor = AnchorStyles.Top;
			this.Panel_Wait.BackColor = Color.Gold;
			this.Panel_Wait.BorderStyle = BorderStyle.FixedSingle;
			this.Panel_Wait.Controls.Add(this.ProgressBar_Wait);
			this.Panel_Wait.Controls.Add(this.Label_Wait);
			Control arg_272D_0 = this.Panel_Wait;
			location = new Point(264, 150);
			arg_272D_0.Location = location;
			this.Panel_Wait.Name = "Panel_Wait";
			Control arg_2757_0 = this.Panel_Wait;
			size = new Size(252, 80);
			arg_2757_0.Size = size;
			this.Panel_Wait.TabIndex = 121;
			this.Panel_Wait.Visible = false;
			Control arg_2786_0 = this.ProgressBar_Wait;
			location = new Point(3, 66);
			arg_2786_0.Location = location;
			this.ProgressBar_Wait.Name = "ProgressBar_Wait";
			Control arg_27B0_0 = this.ProgressBar_Wait;
			size = new Size(244, 10);
			arg_27B0_0.Size = size;
			this.ProgressBar_Wait.TabIndex = 41;
			this.Label_Wait.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Label_Wait.BackColor = Color.Transparent;
			this.Label_Wait.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			Control arg_280B_0 = this.Label_Wait;
			location = new Point(-1, 3);
			arg_280B_0.Location = location;
			this.Label_Wait.Name = "Label_Wait";
			Control arg_2835_0 = this.Label_Wait;
			size = new Size(252, 56);
			arg_2835_0.Size = size;
			this.Label_Wait.TabIndex = 40;
			this.Label_Wait.Text = "Initializing images...\r\nplease wait!";
			this.Label_Wait.TextAlign = ContentAlignment.MiddleCenter;
			this.Label_Line.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Label_Line.BackColor = Color.Transparent;
			this.Label_Line.BorderStyle = BorderStyle.FixedSingle;
			this.Label_Line.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_Line.ForeColor = Color.Black;
			Control arg_28D1_0 = this.Label_Line;
			location = new Point(247, 465);
			arg_28D1_0.Location = location;
			this.Label_Line.Name = "Label_Line";
			Control arg_28FA_0 = this.Label_Line;
			size = new Size(533, 5);
			arg_28FA_0.Size = size;
			this.Label_Line.TabIndex = 122;
			this.Label_Line.TextAlign = ContentAlignment.MiddleLeft;
			this.PictureBox_Loading.BackColor = Color.Honeydew;
			this.PictureBox_Loading.Image = (Image)componentResourceManager.GetObject("PictureBox_Loading.Image");
			Control arg_2959_0 = this.PictureBox_Loading;
			location = new Point(120, 305);
			arg_2959_0.Location = location;
			this.PictureBox_Loading.Name = "PictureBox_Loading";
			Control arg_2980_0 = this.PictureBox_Loading;
			size = new Size(120, 90);
			arg_2980_0.Size = size;
			this.PictureBox_Loading.SizeMode = PictureBoxSizeMode.StretchImage;
			this.PictureBox_Loading.TabIndex = 123;
			this.PictureBox_Loading.TabStop = false;
			this.PictureBox_Loading.Visible = false;
			this.Timer_AutoApply.Interval = 1000;
			this.Label_Donate.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Label_Donate.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_Donate.ForeColor = Color.Black;
			Control arg_2A13_0 = this.Label_Donate;
			location = new Point(1, 477);
			arg_2A13_0.Location = location;
			this.Label_Donate.Name = "Label_Donate";
			Control arg_2A3D_0 = this.Label_Donate;
			size = new Size(247, 16);
			arg_2A3D_0.Size = size;
			this.Label_Donate.TabIndex = 124;
			this.Label_Donate.Text = "Donate to remove the text \"unregistered version\"";
			this.Label_Donate.TextAlign = ContentAlignment.MiddleCenter;
			this.CMS_FullImage.Items.AddRange(new ToolStripItem[]
			{
				this.TSM_FU_SetWallpaper,
				this.TSM_FU_CopyImage,
				this.ToolStripSeparator5,
				this.TSM_FU_BrowseDirectory
			});
			this.CMS_FullImage.Name = "CMS_FullImage";
			Control arg_2AD3_0 = this.CMS_FullImage;
			size = new Size(206, 76);
			arg_2AD3_0.Size = size;
			this.TSM_FU_SetWallpaper.Name = "TSM_FU_SetWallpaper";
			ToolStripItem arg_2AFD_0 = this.TSM_FU_SetWallpaper;
			size = new Size(205, 22);
			arg_2AFD_0.Size = size;
			this.TSM_FU_SetWallpaper.Text = "Set image as wallpaper";
			this.TSM_FU_SetWallpaper.Visible = false;
			this.TSM_FU_CopyImage.Name = "TSM_FU_CopyImage";
			ToolStripItem arg_2B43_0 = this.TSM_FU_CopyImage;
			size = new Size(205, 22);
			arg_2B43_0.Size = size;
			this.TSM_FU_CopyImage.Text = "Copy image to clipboard";
			this.ToolStripSeparator5.Name = "ToolStripSeparator5";
			ToolStripItem arg_2B7C_0 = this.ToolStripSeparator5;
			size = new Size(202, 6);
			arg_2B7C_0.Size = size;
			this.ToolStripSeparator5.Visible = false;
			this.TSM_FU_BrowseDirectory.Name = "TSM_FU_BrowseDirectory";
			ToolStripItem arg_2BB2_0 = this.TSM_FU_BrowseDirectory;
			size = new Size(205, 22);
			arg_2BB2_0.Size = size;
			this.TSM_FU_BrowseDirectory.Text = "Browse image directory";
			this.TSM_FU_BrowseDirectory.Visible = false;
			this.CK_ThumbDB.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.CK_ThumbDB.AutoSize = true;
			this.CK_ThumbDB.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.CK_ThumbDB.ForeColor = Color.Black;
			Control arg_2C30_0 = this.CK_ThumbDB;
			location = new Point(257, 474);
			arg_2C30_0.Location = location;
			this.CK_ThumbDB.Name = "CK_ThumbDB";
			Control arg_2C5A_0 = this.CK_ThumbDB;
			size = new Size(408, 17);
			arg_2C5A_0.Size = size;
			this.CK_ThumbDB.TabIndex = 125;
			this.CK_ThumbDB.Text = "store thumbnails to improve performance (maximum db size: 50 MB ~ 10.000 files)";
			this.CK_ThumbDB.UseVisualStyleBackColor = true;
			this.Button_Clean_ThumbDB.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Button_Clean_ThumbDB.Cursor = Cursors.Hand;
			this.Button_Clean_ThumbDB.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Button_Clean_ThumbDB.ForeColor = Color.Black;
			Control arg_2CE9_0 = this.Button_Clean_ThumbDB;
			location = new Point(677, 470);
			arg_2CE9_0.Location = location;
			this.Button_Clean_ThumbDB.Name = "Button_Clean_ThumbDB";
			Control arg_2D10_0 = this.Button_Clean_ThumbDB;
			size = new Size(88, 23);
			arg_2D10_0.Size = size;
			this.Button_Clean_ThumbDB.TabIndex = 126;
			this.Button_Clean_ThumbDB.Text = "Clean DB";
			this.Button_Clean_ThumbDB.UseVisualStyleBackColor = true;
			this.Label_Line_2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Label_Line_2.BackColor = Color.Transparent;
			this.Label_Line_2.BorderStyle = BorderStyle.FixedSingle;
			this.Label_Line_2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_Line_2.ForeColor = Color.Black;
			Control arg_2DAB_0 = this.Label_Line_2;
			location = new Point(246, 495);
			arg_2DAB_0.Location = location;
			this.Label_Line_2.Name = "Label_Line_2";
			Control arg_2DD4_0 = this.Label_Line_2;
			size = new Size(533, 5);
			arg_2DD4_0.Size = size;
			this.Label_Line_2.TabIndex = 127;
			this.Label_Line_2.TextAlign = ContentAlignment.MiddleLeft;
			this.Label_Img_Resolution.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Label_Img_Resolution.BorderStyle = BorderStyle.FixedSingle;
			this.Label_Img_Resolution.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Label_Img_Resolution.ForeColor = Color.Black;
			Control arg_2E4C_0 = this.Label_Img_Resolution;
			location = new Point(3, 459);
			arg_2E4C_0.Location = location;
			this.Label_Img_Resolution.Name = "Label_Img_Resolution";
			Control arg_2E76_0 = this.Label_Img_Resolution;
			size = new Size(240, 15);
			arg_2E76_0.Size = size;
			this.Label_Img_Resolution.TabIndex = 128;
			this.Label_Img_Resolution.Text = "Resolution: 2500x1300";
			this.Label_Img_Resolution.TextAlign = ContentAlignment.MiddleCenter;
			this.PictureBox_Saving.BackColor = Color.Honeydew;
			this.PictureBox_Saving.Image = (Image)componentResourceManager.GetObject("PictureBox_Saving.Image");
			Control arg_2EEB_0 = this.PictureBox_Saving;
			location = new Point(129, 348);
			arg_2EEB_0.Location = location;
			this.PictureBox_Saving.Name = "PictureBox_Saving";
			Control arg_2F12_0 = this.PictureBox_Saving;
			size = new Size(120, 90);
			arg_2F12_0.Size = size;
			this.PictureBox_Saving.SizeMode = PictureBoxSizeMode.StretchImage;
			this.PictureBox_Saving.TabIndex = 129;
			this.PictureBox_Saving.TabStop = false;
			this.PictureBox_Saving.Visible = false;
			this.PictureBox_Back.BackColor = Color.Honeydew;
			this.PictureBox_Back.Image = (Image)componentResourceManager.GetObject("PictureBox_Back.Image");
			Control arg_2F8E_0 = this.PictureBox_Back;
			location = new Point(142, 366);
			arg_2F8E_0.Location = location;
			this.PictureBox_Back.Name = "PictureBox_Back";
			Control arg_2FB5_0 = this.PictureBox_Back;
			size = new Size(120, 90);
			arg_2FB5_0.Size = size;
			this.PictureBox_Back.SizeMode = PictureBoxSizeMode.StretchImage;
			this.PictureBox_Back.TabIndex = 130;
			this.PictureBox_Back.TabStop = false;
			this.PictureBox_Back.Visible = false;
			this.PictureBox_Folder.BackColor = Color.Honeydew;
			this.PictureBox_Folder.Image = (Image)componentResourceManager.GetObject("PictureBox_Folder.Image");
			Control arg_302E_0 = this.PictureBox_Folder;
			location = new Point(73, 373);
			arg_302E_0.Location = location;
			this.PictureBox_Folder.Name = "PictureBox_Folder";
			Control arg_3055_0 = this.PictureBox_Folder;
			size = new Size(120, 90);
			arg_3055_0.Size = size;
			this.PictureBox_Folder.SizeMode = PictureBoxSizeMode.StretchImage;
			this.PictureBox_Folder.TabIndex = 131;
			this.PictureBox_Folder.TabStop = false;
			this.PictureBox_Folder.Visible = false;
			this.Button_Watermark_Block.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
			this.Button_Watermark_Block.Cursor = Cursors.Hand;
			this.Button_Watermark_Block.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.Button_Watermark_Block.ForeColor = Color.Black;
			Control arg_30EF_0 = this.Button_Watermark_Block;
			location = new Point(512, 451);
			arg_30EF_0.Location = location;
			this.Button_Watermark_Block.Name = "Button_Watermark_Block";
			Control arg_3119_0 = this.Button_Watermark_Block;
			size = new Size(171, 23);
			arg_3119_0.Size = size;
			this.Button_Watermark_Block.TabIndex = 132;
			this.Button_Watermark_Block.Text = "BLOCK watermark process";
			this.Button_Watermark_Block.UseVisualStyleBackColor = true;
			this.Button_Watermark_Block.Visible = false;
			this.Timer_Update.Interval = 30000;
			this.Timer_Close.Interval = 10000;
			SizeF autoScaleDimensions = new SizeF(6f, 13f);
			this.AutoScaleDimensions = autoScaleDimensions;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.GhostWhite;
			size = new Size(792, 531);
			this.ClientSize = size;
			this.Controls.Add(this.Button_Watermark_Block);
			this.Controls.Add(this.Button_Refresh_Images);
			this.Controls.Add(this.PictureBox_Folder);
			this.Controls.Add(this.PictureBox_Back);
			this.Controls.Add(this.PictureBox_Saving);
			this.Controls.Add(this.Label_Img_Resolution);
			this.Controls.Add(this.Label_Line_2);
			this.Controls.Add(this.Button_Clean_ThumbDB);
			this.Controls.Add(this.CK_ThumbDB);
			this.Controls.Add(this.Panel_Wait);
			this.Controls.Add(this.Label_SelDir);
			this.Controls.Add(this.PictureBox_Loading);
			this.Controls.Add(this.Label_Line);
			this.Controls.Add(this.Button_BrowseFolder);
			this.Controls.Add(this.Panel2);
			this.Controls.Add(this.WebBrowser_Stats);
			this.Controls.Add(this.Panel1);
			this.Controls.Add(this.Label_SelImg);
			this.Controls.Add(this.Button_Select_Dir);
			this.Controls.Add(this.Label8);
			this.Controls.Add(this.TextBox_Directory);
			this.Controls.Add(this.Button_Donate);
			this.Controls.Add(this.Button_About);
			this.Controls.Add(this.Button_Updates);
			this.Controls.Add(this.Button_Watermark_Folder);
			this.Controls.Add(this.Button_Watermark_Image);
			this.Controls.Add(this.Label7);
			this.Controls.Add(this.CK_SaveSubfolder);
			this.Controls.Add(this.CK_SaveFolder);
			this.Controls.Add(this.Label_Preview);
			this.Controls.Add(this.Label_Status);
			this.Controls.Add(this.PictureBox_Preview);
			this.Controls.Add(this.Panel_Images);
			this.Controls.Add(this.Label_Donate);
			this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.ForeColor = Color.Black;
			this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			size = new Size(800, 530);
			this.MinimumSize = size;
			this.Name = "Form_Main";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "My Watermark";
			((ISupportInitialize)this.PictureBox_Preview).EndInit();
			this.Panel1.ResumeLayout(false);
			this.Panel1.PerformLayout();
			this.Panel2.ResumeLayout(false);
			this.Panel2.PerformLayout();
			this.Panel_Wait.ResumeLayout(false);
			((ISupportInitialize)this.PictureBox_Loading).EndInit();
			this.CMS_FullImage.ResumeLayout(false);
			((ISupportInitialize)this.PictureBox_Saving).EndInit();
			((ISupportInitialize)this.PictureBox_Back).EndInit();
			((ISupportInitialize)this.PictureBox_Folder).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
		{
			Module_MyFunctions.PRG_Exit = true;
			int arg_10_0 = 0;
			int tHREAD_Totals = this.THREAD_Totals;
			checked
			{
				for (int i = arg_10_0; i <= tHREAD_Totals; i++)
				{
					this.THREAD_Stop[i] = true;
				}
			}
		}
		private void Form_Main_Load(object sender, EventArgs e)
		{
			Module_MyFunctions.Init_PRG_Data(Application.ExecutablePath);
			Module_MyFunctions.Admin_Rights_Check();
			if (Environment.ProcessorCount > 1)
			{
				this.THREAD_Totals = 2;
			}
			else
			{
				this.THREAD_Totals = 1;
			}
			checked
			{
				this.THREAD_Stop = new bool[this.THREAD_Totals + 1];
				this.THREAD_Stopped = new bool[this.THREAD_Totals + 1];
				int arg_5A_0 = 0;
				int tHREAD_Totals = this.THREAD_Totals;
				for (int i = arg_5A_0; i <= tHREAD_Totals; i++)
				{
					this.THREAD_Stopped[i] = true;
				}
				Module_Watermark.DAT_ThumbnailsDB = Module_MyFunctions.DAT_Path + "thumbnails\\";
				if (!Directory.Exists(Module_Watermark.DAT_ThumbnailsDB))
				{
					Directory.CreateDirectory(Module_Watermark.DAT_ThumbnailsDB);
				}
				this.TextBox_Directory.Text = Module_MyFunctions.Config_Read("lastDir", "", true);
				if (Operators.CompareString(Module_MyFunctions.Config_Read("thumbDB", "", true), "N", false) == 0)
				{
					this.CK_ThumbDB.Checked = false;
					Module_Watermark.DAT_Thumbnails = false;
				}
				else
				{
					this.CK_ThumbDB.Checked = true;
					Module_Watermark.DAT_Thumbnails = true;
				}
				if (Operators.CompareString(Module_MyFunctions.Config_Read("saveInFolder", "", true), "Y", false) == 0)
				{
					this.PRV_SaveSubfolder = false;
					this.CK_SaveSubfolder.Checked = false;
					this.CK_SaveFolder.Checked = true;
				}
				else
				{
					this.PRV_SaveSubfolder = true;
					this.CK_SaveSubfolder.Checked = true;
					this.CK_SaveFolder.Checked = false;
				}
				if (Operators.CompareString(Module_MyFunctions.Config_Read("watermarkText", "", true), "", false) != 0)
				{
					this.TextBox_Watermark.Text = Strings.Replace(Module_MyFunctions.Config_Read("watermarkText", "", true), "<br>", "\r\n", 1, -1, CompareMethod.Binary);
				}
				else
				{
					this.TextBox_Watermark.Text = "Your text here";
				}
				if (Operators.CompareString(Module_MyFunctions.Config_Read("watermarkSize", "", true), "", false) != 0)
				{
					this.TextBox_FontSize.Text = Module_MyFunctions.Config_Read("watermarkSize", "", true);
				}
				else
				{
					this.TextBox_FontSize.Text = "80";
				}
				if (Operators.CompareString(Module_MyFunctions.Config_Read("watermarkMargin", "", true), "", false) != 0)
				{
					this.TextBox_Margin.Text = Module_MyFunctions.Config_Read("watermarkMargin", "", true);
				}
				else
				{
					this.TextBox_Margin.Text = "10";
				}
				Module_Watermark.DAT_Watermark_Pos = Module_MyFunctions.Config_Read("watermarkPos", "", true);
				string left = Module_MyFunctions.Config_Read("watermarkPos", "", true);
				if (Operators.CompareString(left, "TL", false) == 0)
				{
					this.RBP_TL.Checked = true;
				}
				else
				{
					if (Operators.CompareString(left, "TC", false) == 0)
					{
						this.RBP_TC.Checked = true;
					}
					else
					{
						if (Operators.CompareString(left, "TR", false) == 0)
						{
							this.RBP_TR.Checked = true;
						}
						else
						{
							if (Operators.CompareString(left, "ML", false) == 0)
							{
								this.RBP_ML.Checked = true;
							}
							else
							{
								if (Operators.CompareString(left, "MC", false) == 0)
								{
									this.RBP_MC.Checked = true;
								}
								else
								{
									if (Operators.CompareString(left, "MR", false) == 0)
									{
										this.RBP_MR.Checked = true;
									}
									else
									{
										if (Operators.CompareString(left, "BL", false) == 0)
										{
											this.RBP_BL.Checked = true;
										}
										else
										{
											if (Operators.CompareString(left, "BC", false) == 0)
											{
												this.RBP_BC.Checked = true;
											}
											else
											{
												Module_Watermark.DAT_Watermark_Pos = "BR";
												this.RBP_BR.Checked = true;
											}
										}
									}
								}
							}
						}
					}
				}
				try
				{
					if (Operators.CompareString(Module_MyFunctions.Config_Read("textColor", "", true), "", false) != 0)
					{
						this.Label_Color.BackColor = Color.FromArgb(Conversions.ToInteger(Module_MyFunctions.Config_Read("textColor", "", true)));
					}
				}
				catch (Exception expr_3CD)
				{
					ProjectData.SetProjectError(expr_3CD);
					this.Label_Color.BackColor = Color.White;
					ProjectData.ClearProjectError();
				}
				if (Operators.CompareString(Module_MyFunctions.Config_Read("textShadow", "", true), "N", false) == 0)
				{
					this.CK_Shadow.Checked = false;
				}
				else
				{
					this.CK_Shadow.Checked = true;
				}
				if (Operators.CompareString(Module_MyFunctions.Config_Read("textOutline", "", true), "N", false) == 0)
				{
					this.CK_Outline.Checked = false;
				}
				else
				{
					this.CK_Outline.Checked = true;
				}
				if (this.CK_Shadow.Checked & this.CK_Outline.Checked)
				{
					this.CK_Outline.Checked = false;
				}
				if (this.CK_Shadow.Checked)
				{
					Module_Watermark.DAT_Watermark_Effect = "S";
				}
				else
				{
					Module_Watermark.DAT_Watermark_Effect = "O";
				}
				if (Operators.CompareString(Module_MyFunctions.Config_Read("fontSizePixel", "", true), "Y", false) == 0)
				{
					this.CK_SizePixel.Checked = true;
					this.CK_SizeRelative.Checked = false;
					this.PRV_SizePixel = true;
				}
				else
				{
					this.CK_SizePixel.Checked = false;
					this.CK_SizeRelative.Checked = true;
					this.PRV_SizePixel = false;
				}
				string right = Module_MyFunctions.Config_Read("fontFamily", "", true);
				this.ComboBox_FontFamily.Items.Clear();
				FontFamily[] families = FontFamily.Families;
				for (int j = 0; j < families.Length; j++)
				{
					FontFamily fontFamily = families[j];
					try
					{
						Font font = new Font(fontFamily.Name, 20f, FontStyle.Regular, GraphicsUnit.Pixel);
						this.ComboBox_FontFamily.Items.Add(new Class_AddDataItem(fontFamily.Name, fontFamily.Name));
					}
					catch (Exception expr_56D)
					{
						ProjectData.SetProjectError(expr_56D);
						ProjectData.ClearProjectError();
					}
				}
				int arg_59F_0 = 0;
				int num = this.ComboBox_FontFamily.Items.Count - 1;
				for (int k = arg_59F_0; k <= num; k++)
				{
					if (Operators.ConditionalCompareObjectEqual(NewLateBinding.LateGet(this.ComboBox_FontFamily.Items[k], null, "my_Value", new object[0], null, null, null), right, false))
					{
						this.ComboBox_FontFamily.SelectedIndex = k;
						break;
					}
				}
				if (this.ComboBox_FontFamily.SelectedIndex == -1)
				{
					this.ComboBox_FontFamily.SelectedValue = "Arial";
					if (this.ComboBox_FontFamily.SelectedIndex == -1)
					{
						this.ComboBox_FontFamily.SelectedIndex = 0;
					}
				}
				this.ComboBox_Style.Items.Clear();
				this.ComboBox_Style.Items.Add(new Class_AddDataItem(FontStyle.Regular, "Normal"));
				this.ComboBox_Style.Items.Add(new Class_AddDataItem(FontStyle.Bold, "Bold"));
				this.ComboBox_Style.Items.Add(new Class_AddDataItem(FontStyle.Italic, "Italic"));
				this.ComboBox_Style.Items.Add(new Class_AddDataItem(FontStyle.Underline, "Underline"));
				string left2 = Module_MyFunctions.Config_Read("watermarkStyle", "", true);
				if (Operators.CompareString(left2, "Bold", false) == 0)
				{
					this.ComboBox_Style.SelectedIndex = 1;
				}
				else
				{
					if (Operators.CompareString(left2, "Italic", false) == 0)
					{
						this.ComboBox_Style.SelectedIndex = 2;
					}
					else
					{
						if (Operators.CompareString(left2, "Underline", false) == 0)
						{
							this.ComboBox_Style.SelectedIndex = 3;
						}
						else
						{
							this.ComboBox_Style.SelectedIndex = 0;
						}
					}
				}
				this.ComboBox_Size.Items.Clear();
				this.ComboBox_Size.Items.Add(new Class_AddDataItem(-30, "30%"));
				this.ComboBox_Size.Items.Add(new Class_AddDataItem(-50, "50%"));
				this.ComboBox_Size.Items.Add(new Class_AddDataItem(-90, "90%"));
				if (this.ComboBox_Size.SelectedIndex == -1)
				{
					this.ComboBox_Size.SelectedIndex = 0;
				}
				string left3 = Module_MyFunctions.Config_Read("watermarkSizeRelative", "", true);
				if (Operators.CompareString(left3, "50", false) == 0)
				{
					this.ComboBox_Size.SelectedIndex = 1;
				}
				else
				{
					if (Operators.CompareString(left3, "90", false) == 0)
					{
						this.ComboBox_Size.SelectedIndex = 2;
					}
					else
					{
						this.ComboBox_Size.SelectedIndex = 0;
					}
				}
				this.ComboBox_Transparency.Items.Clear();
				this.ComboBox_Transparency.Items.Add(new Class_AddDataItem(255, "None"));
				this.ComboBox_Transparency.Items.Add(new Class_AddDataItem(200, "Lowest"));
				this.ComboBox_Transparency.Items.Add(new Class_AddDataItem(150, "Low"));
				this.ComboBox_Transparency.Items.Add(new Class_AddDataItem(100, "Medium"));
				this.ComboBox_Transparency.Items.Add(new Class_AddDataItem(50, "High"));
				this.ComboBox_Transparency.Items.Add(new Class_AddDataItem(25, "Highest"));
				if (this.ComboBox_Transparency.SelectedIndex == -1)
				{
					this.ComboBox_Transparency.SelectedIndex = 0;
				}
				string left4 = Module_MyFunctions.Config_Read("watermarkTransparency", "", true);
				if (Operators.CompareString(left4, "200", false) == 0)
				{
					this.ComboBox_Transparency.SelectedIndex = 1;
				}
				else
				{
					if (Operators.CompareString(left4, "150", false) == 0)
					{
						this.ComboBox_Transparency.SelectedIndex = 2;
					}
					else
					{
						if (Operators.CompareString(left4, "100", false) == 0)
						{
							this.ComboBox_Transparency.SelectedIndex = 3;
						}
						else
						{
							if (Operators.CompareString(left4, "50", false) == 0)
							{
								this.ComboBox_Transparency.SelectedIndex = 4;
							}
							else
							{
								if (Operators.CompareString(left4, "25", false) == 0)
								{
									this.ComboBox_Transparency.SelectedIndex = 5;
								}
								else
								{
									this.ComboBox_Transparency.SelectedIndex = 0;
								}
							}
						}
					}
				}
				int num2;
				int num3;
				int num4;
				int num5;
				try
				{
					num2 = Convert.ToInt32(Module_MyFunctions.Config_Read("formWidth", "", true));
					num3 = Convert.ToInt32(Module_MyFunctions.Config_Read("formHeight", "", true));
					num4 = Convert.ToInt32(Module_MyFunctions.Config_Read("formPosX", "", true));
					num5 = Convert.ToInt32(Module_MyFunctions.Config_Read("formPosY", "", true));
				}
				catch (Exception expr_A2D)
				{
					ProjectData.SetProjectError(expr_A2D);
					num2 = 0;
					num3 = 0;
					num4 = 0;
					num5 = 0;
					ProjectData.ClearProjectError();
				}
				if (num2 <= 0)
				{
					num2 = this.MinimumSize.Width;
				}
				if (num3 <= 0)
				{
					num3 = this.MinimumSize.Height;
				}
				if (num3 > Screen.PrimaryScreen.WorkingArea.Height)
				{
					num3 = Screen.PrimaryScreen.WorkingArea.Height - 10;
				}
				if (num2 > Screen.PrimaryScreen.WorkingArea.Width)
				{
					num2 = Screen.PrimaryScreen.WorkingArea.Width;
				}
				Point point = new Point(num2, num3);
				this.Size = (Size)point;
				if ((num4 == 0 & num5 == 0) | num4 > Screen.PrimaryScreen.WorkingArea.Width | num5 > Screen.PrimaryScreen.WorkingArea.Height)
				{
					Form form = this;
					Module_MyFunctions.show_myForm(form, "C", true);
				}
				else
				{
					point = new Point(num4, num5);
					this.Location = point;
				}
				this.PRV_FileList = new ArrayList();
				this.Text = Module_Watermark.PRG_Title;
				Control arg_B7E_0 = this.Label_Line;
				point = new Point(this.Label_Line.Size.Width, 1);
				arg_B7E_0.Size = (Size)point;
				Control arg_BAC_0 = this.Label_Line_2;
				point = new Point(this.Label_Line_2.Size.Width, 1);
				arg_BAC_0.Size = (Size)point;
				this.Label_Img_Resolution.Text = "";
				this.Show();
				Application.DoEvents();
				Module_MyFunctions.PRG_CheckUpdates(false);
				Module_Internet.WEB_Verify_MD5_EXE();
				Module_Donate.Donate_Check(Module_Watermark.PRG_Donate_Days);
				this.Donate_Set();
				if (this.CK_ThumbDB.Checked)
				{
					this.Clean_ThumbDB();
				}
				if (Operators.CompareString(this.TextBox_Directory.Text, "", false) != 0)
				{
					this.Refresh_Thumbnails(false);
				}
				if (Module_MyFunctions.DEBUG_DEV)
				{
					this.Text = string.Concat(new string[]
					{
						this.Text,
						" - ",
						Conversions.ToString(Environment.ProcessorCount),
						" CPU (",
						Conversions.ToString(this.THREAD_Totals + 1),
						" threads)"
					});
				}
				Module_MyFunctions.PRG_Loading = false;
				if (Module_MyFunctions.PRG_Exit)
				{
					Application.Exit();
				}
			}
		}
		public void Donate_Set()
		{
			if (Module_Donate.PRG_Donate)
			{
				this.Label_Donate.Text = "Registered version";
			}
		}
		private void Timer_Update_Tick(object sender, EventArgs e)
		{
			checked
			{
				try
				{
					this.Timer_Update.Enabled = false;
					this.Timer_Update.Interval = 30000;
					bool enabled = true;
					Module_MyFunctions.GLB_Timer_CK_count++;
					if (Module_MyFunctions.GLB_Timer_CK_onlyStats)
					{
						if (MyProject.Computer.Network.IsAvailable | Module_MyFunctions.GLB_Timer_CK_count > 5)
						{
							enabled = false;
							this.WebBrowser_Stats.Navigate(Module_Internet.WEB_Generate_Stats_URL());
							if (Operators.CompareString(Module_Donate.PRG_Donate_Last_State, "E", false) == 0 | Operators.CompareString(Module_Donate.PRG_Donate_Last_State, "", false) == 0)
							{
								bool pRG_Donate = Module_Donate.PRG_Donate;
								Module_Donate.Donate_Check(Module_Watermark.PRG_Donate_Days);
								if (pRG_Donate != Module_Donate.PRG_Donate)
								{
									this.Donate_Set();
								}
							}
						}
					}
					else
					{
						bool arg_AC_0 = true;
						NotifyIcon notifyIcon = null;
						if (Module_Internet.WEB_CheckUpdate(arg_AC_0, notifyIcon))
						{
							Module_MyFunctions.GLB_Timer_CK_onlyStats = true;
						}
						else
						{
							if (Module_MyFunctions.GLB_Timer_CK_count > 10)
							{
								Module_MyFunctions.GLB_Timer_CK_onlyStats = true;
							}
						}
					}
					this.Timer_Update.Enabled = enabled;
				}
				catch (Exception expr_D8)
				{
					ProjectData.SetProjectError(expr_D8);
					Exception ex = expr_D8;
					Module_MyFunctions.Update_LOG("ERROR [Timer_Update_Tick] [" + ex.Message + "]", false, "", false);
					ProjectData.ClearProjectError();
				}
			}
		}
		private void Button_Refresh_Images_Click(object sender, EventArgs e)
		{
			this.Refresh_Thumbnails(true);
		}
		private void Refresh_Thumbnails(bool my_ShowErrors = true)
		{
			if (this.PRV_Img_Refresh)
			{
				return;
			}
			string text = this.TextBox_Directory.Text;
			if (!text.EndsWith("\\"))
			{
				text += "\\";
			}
			checked
			{
				if (!Directory.Exists(text))
				{
					if (my_ShowErrors)
					{
						MessageBox.Show("The selected directory does not exists!", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				else
				{
					if (Operators.CompareString(text.ToUpper(), Strings.UCase(Module_Watermark.DAT_ThumbnailsDB), false) == 0)
					{
						MessageBox.Show("The selected directory is used by the software and cannot be browsed!", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					bool flag = false;
					while (!flag)
					{
						flag = true;
						int arg_8E_0 = 0;
						int tHREAD_Totals = this.THREAD_Totals;
						for (int i = arg_8E_0; i <= tHREAD_Totals; i++)
						{
							this.THREAD_Stop[i] = true;
							if (!this.THREAD_Stopped[i])
							{
								flag = false;
								break;
							}
						}
						Thread.Sleep(50);
					}
					int arg_C4_0 = 0;
					int tHREAD_Totals2 = this.THREAD_Totals;
					for (int i = arg_C4_0; i <= tHREAD_Totals2; i++)
					{
						this.THREAD_Stop[i] = false;
					}
					this.EnableDisableButtons(false);
					this.Show_Wait_Panel(this.PRV_Message_Load);
					try
					{
						this.Label_SelDir.Visible = false;
						this.PRV_Img_Refresh = true;
						string[] files = Directory.GetFiles(text, "*.*");
						string[] directories = Directory.GetDirectories(text, "*.*");
						string text2 = "";
						int num = 0;
						int num2 = 0;
						int num3 = 0;
						int num4 = 120;
						this.Panel_Images.AutoScroll = false;
						while (this.Panel_Images.Controls.Count != 0)
						{
							this.Panel_Images.Controls.Remove(this.Panel_Images.Controls[0]);
							Application.DoEvents();
						}
						PictureBox pictureBox = new PictureBox();
						Control arg_1A9_0 = pictureBox;
						Point point = new Point(num3 * 120 + 2 + num3 * 2, num2 * 90 + 2 + num2 * 2);
						arg_1A9_0.Location = point;
						Control arg_1C2_0 = pictureBox;
						point = new Point(120, 90);
						arg_1C2_0.Size = (Size)point;
						pictureBox.BorderStyle = BorderStyle.FixedSingle;
						pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
						pictureBox.Cursor = Cursors.Hand;
						pictureBox.SendToBack();
						pictureBox.Tag = "BCK|" + text;
						pictureBox.AccessibleDescription = "";
						pictureBox.Image = this.PictureBox_Back.Image;
						this.Panel_Images.Controls.Add(pictureBox);
						pictureBox.MouseClick += new MouseEventHandler(this.Select_ImageBox_Navigate);
						this.PRV_FileList = new ArrayList();
						this.Label_Status.BackColor = Color.Yellow;
						string[] array = directories;
						for (int j = 0; j < array.Length; j++)
						{
							text2 = array[j];
							bool flag2 = true;
							try
							{
								if (text2.ToUpper().Contains("$RECYCLE.BIN"))
								{
									flag2 = false;
								}
								else
								{
									string[] files2 = Directory.GetFiles(text2, "*.*");
								}
							}
							catch (Exception expr_299)
							{
								ProjectData.SetProjectError(expr_299);
								flag2 = false;
								ProjectData.ClearProjectError();
							}
							if (flag2)
							{
								num3++;
								if (num3 * num4 + 2 + num4 + num3 * 2 > this.Panel_Images.Width - 26)
								{
									num3 = 0;
									num2++;
								}
								pictureBox = new PictureBox();
								Control arg_303_0 = pictureBox;
								point = new Point(num3 * 120 + 2 + num3 * 2, num2 * 90 + 2 + num2 * 2);
								arg_303_0.Location = point;
								Control arg_31C_0 = pictureBox;
								point = new Point(120, 90);
								arg_31C_0.Size = (Size)point;
								pictureBox.BorderStyle = BorderStyle.FixedSingle;
								pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
								pictureBox.Cursor = Cursors.Hand;
								pictureBox.SendToBack();
								pictureBox.Tag = "DIR|" + text2;
								pictureBox.AccessibleDescription = "!0";
								pictureBox.Image = this.PictureBox_Folder.Image;
								try
								{
									ToolTip toolTip = new ToolTip();
									if (text2.EndsWith("\\"))
									{
										text2 = Strings.Left(text2, text2.Length - 1);
									}
									if (text2.Contains("\\"))
									{
										text2 = Strings.Mid(text2, Strings.InStrRev(text2, "\\", -1, CompareMethod.Binary) + 1);
									}
									toolTip.SetToolTip(pictureBox, text2);
								}
								catch (Exception expr_3D0)
								{
									ProjectData.SetProjectError(expr_3D0);
									ProjectData.ClearProjectError();
								}
								this.Panel_Images.Controls.Add(pictureBox);
								pictureBox.MouseClick += new MouseEventHandler(this.Select_ImageBox_Navigate);
							}
						}
						int num5 = 0;
						Random random = new Random();
						string[] array2 = files;
						for (int k = 0; k < array2.Length; k++)
						{
							string text3 = array2[k];
							num++;
							this.Label_Status.Text = string.Concat(new string[]
							{
								"Initializing thumbnails [",
								num.ToString().PadLeft(files.Count<string>().ToString().Length, '0'),
								"] / [",
								files.Count<string>().ToString().PadLeft(files.Count<string>().ToString().Length, '0'),
								"]"
							});
							string text4 = text3;
							text3 = text3.ToUpper();
							if (text3.EndsWith(".JPG") | text3.EndsWith(".PNG") | text3.EndsWith(".JPEG"))
							{
								num5++;
								if (num5 > this.THREAD_Totals)
								{
									num5 = 1;
								}
								this.PRV_FileList.Add(text4);
								num3++;
								if (num3 * num4 + 2 + num4 + num3 * 2 > this.Panel_Images.Width - 26)
								{
									num3 = 0;
									num2++;
								}
								pictureBox = new PictureBox();
								Control arg_572_0 = pictureBox;
								point = new Point(num3 * 120 + 2 + num3 * 2, num2 * 90 + 2 + num2 * 2);
								arg_572_0.Location = point;
								Control arg_58B_0 = pictureBox;
								point = new Point(120, 90);
								arg_58B_0.Size = (Size)point;
								pictureBox.BorderStyle = BorderStyle.FixedSingle;
								pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
								pictureBox.Cursor = Cursors.Hand;
								pictureBox.SendToBack();
								pictureBox.Tag = text4;
								pictureBox.AccessibleDescription = "*" + Conversions.ToString(num5);
								try
								{
									ToolTip toolTip2 = new ToolTip();
									toolTip2.SetToolTip(pictureBox, Path.GetFileNameWithoutExtension(text4));
								}
								catch (Exception expr_5ED)
								{
									ProjectData.SetProjectError(expr_5ED);
									ProjectData.ClearProjectError();
								}
								this.Panel_Images.Controls.Add(pictureBox);
								pictureBox.Image = this.PictureBox_Loading.Image;
								pictureBox.MouseClick += new MouseEventHandler(this.Select_ImageBox);
								if (random.Next(10) == 1)
								{
									try
									{
										this.ProgressBar_Wait.Value = (int)Math.Round(Math.Floor((double)(num * 100) / (double)files.Count<string>()));
									}
									catch (Exception expr_667)
									{
										ProjectData.SetProjectError(expr_667);
										ProjectData.ClearProjectError();
									}
									Application.DoEvents();
								}
							}
						}
						this.Show_Wait_Panel("");
						this.Label_Status.Text = "";
						this.Label_Status.BackColor = Color.YellowGreen;
						this.Panel_Images.AutoScroll = true;
						try
						{
							this.Panel_Images.Refresh();
						}
						catch (Exception expr_6D0)
						{
							ProjectData.SetProjectError(expr_6D0);
							Exception ex = expr_6D0;
							if (Module_MyFunctions.DEBUG_MODE)
							{
								Module_MyFunctions.Update_LOG("ERROR [Refresh_Thumbnails.Panel_Images.Refresh()] [" + ex.Message + "]", false, "", false);
							}
							ProjectData.ClearProjectError();
						}
						Thread[] array3 = new Thread[this.THREAD_Totals + 1];
						int arg_720_0 = 0;
						int tHREAD_Totals3 = this.THREAD_Totals;
						for (int i = arg_720_0; i <= tHREAD_Totals3; i++)
						{
							array3[i] = new Thread(delegate(object a0)
							{
								this.Thread_Refresh_Thumbnails(Conversions.ToInteger(a0));
							});
							array3[i].Priority = ThreadPriority.Normal;
							array3[i].Start(i);
						}
					}
					catch (Exception expr_75D)
					{
						ProjectData.SetProjectError(expr_75D);
						Exception ex2 = expr_75D;
						Module_MyFunctions.Update_LOG("ERROR [Refresh_Thumbnails] [" + ex2.Message + "]", true, "", false);
						ProjectData.ClearProjectError();
					}
					this.PRV_Img_Refresh = false;
					this.EnableDisableButtons(true);
				}
			}
		}
		private void Thread_Refresh_Thumbnails(int my_ThreadNumber)
		{
			this.THREAD_Stopped[my_ThreadNumber] = false;
			checked
			{
				try
				{
					MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
					string text = "";
					bool flag = true;
					while (flag)
					{
						flag = false;
						try
						{
							IEnumerator enumerator = this.Panel_Images.Controls.GetEnumerator();
							while (enumerator.MoveNext())
							{
								object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
								if (this.THREAD_Stop[my_ThreadNumber] | Module_MyFunctions.PRG_Exit)
								{
									this.THREAD_Stopped[my_ThreadNumber] = true;
									this.THREAD_Stop[my_ThreadNumber] = false;
									return;
								}
								text = "";
								string text2 = "-";
								try
								{
									if (Conversions.ToBoolean(Operators.AndObject(Operators.CompareObjectNotEqual(NewLateBinding.LateGet(objectValue, null, "tag", new object[0], null, null, null), "", false), NewLateBinding.LateGet(objectValue, null, "AccessibleDescription", new object[0], null, null, null).ToString().Contains(Conversions.ToString(my_ThreadNumber)))))
									{
										text2 = Conversions.ToString(NewLateBinding.LateGet(objectValue, null, "AccessibleDescription", new object[0], null, null, null));
										flag = true;
										if (NewLateBinding.LateGet(objectValue, null, "AccessibleDescription", new object[0], null, null, null).ToString().StartsWith("*"))
										{
											NewLateBinding.LateSet(objectValue, null, "AccessibleDescription", new object[]
											{
												""
											}, null, null);
											if (File.Exists(Conversions.ToString(NewLateBinding.LateGet(objectValue, null, "tag", new object[0], null, null, null))))
											{
												Image image = null;
												string text3 = this.Get_FileName_MD5(Conversions.ToString(NewLateBinding.LateGet(objectValue, null, "tag", new object[0], null, null, null)));
												if (Operators.CompareString(text3, "", false) != 0)
												{
													text = Module_Watermark.DAT_ThumbnailsDB + text3 + ".jpg";
												}
												if (File.Exists(text))
												{
													image = Module_Graphics.Read_Image(text, true);
												}
												else
												{
													image = Module_Graphics.Read_Image(Conversions.ToString(NewLateBinding.LateGet(objectValue, null, "tag", new object[0], null, null, null)), true);
													image = Module_Watermark.Resize_ImageBox(image, 120, 90, image.Height > image.Width);
													NewLateBinding.LateSet(objectValue, null, "image", new object[]
													{
														this.PictureBox_Saving.Image
													}, null, null);
													try
													{
														Application.DoEvents();
													}
													catch (Exception expr_238)
													{
														ProjectData.SetProjectError(expr_238);
														ProjectData.ClearProjectError();
													}
													this.Save_Thumbnail(image, text);
												}
												NewLateBinding.LateSet(objectValue, null, "image", new object[]
												{
													image
												}, null, null);
											}
										}
										else
										{
											if (NewLateBinding.LateGet(objectValue, null, "AccessibleDescription", new object[0], null, null, null).ToString().StartsWith("!"))
											{
												NewLateBinding.LateSet(objectValue, null, "AccessibleDescription", new object[]
												{
													""
												}, null, null);
												try
												{
													if (Directory.Exists(Strings.Mid(Conversions.ToString(NewLateBinding.LateGet(objectValue, null, "tag", new object[0], null, null, null)), 5)))
													{
														string[] files = Directory.GetFiles(Strings.Mid(Conversions.ToString(NewLateBinding.LateGet(objectValue, null, "tag", new object[0], null, null, null)), 5), "*.*");
														Rectangle[] array = new Rectangle[5];
														array[1] = new Rectangle(17, 19, 40, 30);
														array[2] = new Rectangle(64, 19, 40, 30);
														array[3] = new Rectangle(17, 52, 40, 30);
														array[4] = new Rectangle(64, 52, 40, 30);
														if (files.Length > 0)
														{
															int num = 0;
															Image image2 = (Image)NewLateBinding.LateGet(objectValue, null, "image", new object[0], null, null, null);
															string[] array2 = files;
															for (int i = 0; i < array2.Length; i++)
															{
																string text4 = array2[i];
																if (text4.ToUpper().EndsWith(".JPG") | text4.ToUpper().EndsWith(".PNG"))
																{
																	num++;
																	if (num > 4)
																	{
																		break;
																	}
																	Image image3 = null;
																	string text3 = this.Get_FileName_MD5(text4);
																	if (Operators.CompareString(text3, "", false) != 0)
																	{
																		text = Module_Watermark.DAT_ThumbnailsDB + text3 + ".jpg";
																	}
																	if (File.Exists(text))
																	{
																		image3 = Module_Graphics.Read_Image(text, true);
																	}
																	else
																	{
																		image3 = Module_Graphics.Read_Image(text4, true);
																		image3 = Module_Watermark.Resize_ImageBox(image3, 120, 90, image3.Height > image3.Width);
																		this.Save_Thumbnail(image3, text);
																	}
																	image3 = Module_Watermark.Resize_ImageBox(image3, 40, 30, image3.Height > image3.Width);
																	Bitmap image4 = new Bitmap(image3);
																	Bitmap bitmap = new Bitmap(image2);
																	Graphics graphics = Graphics.FromImage(bitmap);
																	Rectangle srcRect = new Rectangle(0, 0, 40, 30);
																	graphics.DrawImage(image4, array[num], srcRect, GraphicsUnit.Pixel);
																	image2 = bitmap;
																	try
																	{
																		NewLateBinding.LateSet(objectValue, null, "image", new object[]
																		{
																			image2
																		}, null, null);
																	}
																	catch (Exception expr_4E1)
																	{
																		ProjectData.SetProjectError(expr_4E1);
																		ProjectData.ClearProjectError();
																	}
																}
															}
														}
													}
												}
												catch (Exception expr_506)
												{
													ProjectData.SetProjectError(expr_506);
													Thread.Sleep(250);
													try
													{
														NewLateBinding.LateSet(objectValue, null, "AccessibleDescription", new object[]
														{
															text2
														}, null, null);
													}
													catch (Exception expr_539)
													{
														ProjectData.SetProjectError(expr_539);
														Exception ex = expr_539;
														if (Module_MyFunctions.DEBUG_MODE)
														{
															Module_MyFunctions.Update_LOG(string.Concat(new string[]
															{
																"ERROR [Thread_Refresh_Thumbnails.Folder.AccessibleDescription(",
																text2,
																")] [",
																ex.Message,
																"]"
															}), true, "", false);
														}
														ProjectData.ClearProjectError();
													}
													ProjectData.ClearProjectError();
												}
											}
										}
									}
								}
								catch (Exception expr_5A2)
								{
									ProjectData.SetProjectError(expr_5A2);
									Thread.Sleep(250);
									try
									{
										NewLateBinding.LateSet(objectValue, null, "AccessibleDescription", new object[]
										{
											text2
										}, null, null);
									}
									catch (Exception expr_5D5)
									{
										ProjectData.SetProjectError(expr_5D5);
										Exception ex2 = expr_5D5;
										if (Module_MyFunctions.DEBUG_MODE)
										{
											Module_MyFunctions.Update_LOG(string.Concat(new string[]
											{
												"ERROR [Thread_Refresh_Thumbnails.AccessibleDescription(",
												text2,
												")] [",
												ex2.Message,
												"]"
											}), true, "", false);
										}
										ProjectData.ClearProjectError();
									}
									ProjectData.ClearProjectError();
								}
								Thread.Sleep(5);
							}
						}
						finally
						{
						}
					}
				}
				catch (Exception expr_66B)
				{
					ProjectData.SetProjectError(expr_66B);
					Exception ex3 = expr_66B;
					if (Module_MyFunctions.DEBUG_MODE)
					{
						Module_MyFunctions.Update_LOG(string.Concat(new string[]
						{
							"ERROR [Thread_Refresh_Thumbnails] [",
							ex3.Message,
							" / ",
							ex3.StackTrace,
							"]"
						}), true, "", false);
					}
					ProjectData.ClearProjectError();
				}
				this.THREAD_Stopped[my_ThreadNumber] = true;
			}
		}
		private string Get_FileName_MD5(string my_File)
		{
			if (!File.Exists(my_File))
			{
				return "";
			}
			string text = "";
			try
			{
				FileInfo fileInfo = new FileInfo(my_File);
				string str = fileInfo.LastWriteTime.ToString();
				text = Convert.ToBase64String(Encoding.UTF8.GetBytes(my_File + str));
				text = HttpUtility.UrlEncode(text);
			}
			catch (Exception expr_4B)
			{
				ProjectData.SetProjectError(expr_4B);
				text = "";
				ProjectData.ClearProjectError();
			}
			return text;
		}
		private void Save_Thumbnail(Image my_Thumbnail, string my_File)
		{
			if (File.Exists(my_File) | Operators.CompareString(my_File, "", false) == 0 | !Module_Watermark.DAT_Thumbnails)
			{
				return;
			}
			if (!Directory.Exists(Module_Watermark.DAT_ThumbnailsDB))
			{
				try
				{
					Directory.CreateDirectory(Module_Watermark.DAT_ThumbnailsDB);
				}
				catch (Exception expr_3B)
				{
					ProjectData.SetProjectError(expr_3B);
					Exception ex = expr_3B;
					if (Module_MyFunctions.DEBUG_MODE)
					{
						Module_MyFunctions.Update_LOG("ERROR [Save_Thumbnail.CreateDirectory] [" + ex.Message + "]", true, "", false);
					}
					ProjectData.ClearProjectError();
					return;
				}
			}
			try
			{
				ImageCodecInfo encoder = this.GetEncoder(ImageFormat.Jpeg);
				EncoderParameters encoderParameters = new EncoderParameters(1);
				encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
				my_Thumbnail.Save(my_File, encoder, encoderParameters);
				Application.DoEvents();
			}
			catch (Exception expr_AA)
			{
				ProjectData.SetProjectError(expr_AA);
				Exception ex2 = expr_AA;
				if (Module_MyFunctions.DEBUG_MODE)
				{
					Module_MyFunctions.Update_LOG("ERROR [Thread_Refresh_Thumbnails.SaveThumbnail] [" + ex2.Message + "]", true, "", false);
				}
				ProjectData.ClearProjectError();
			}
		}
		private void Relocate_Thumbnails()
		{
			checked
			{
				try
				{
					if (!this.PRV_Img_Refresh)
					{
						if (!this.Button_Watermark_Block.Visible)
						{
							this.EnableDisableButtons(false);
						}
						this.PRV_Img_Refresh = true;
						int num = 0;
						int num2 = 0;
						int num3 = -1;
						int num4 = 120;
						this.Panel_Images.AutoScroll = false;
						this.Label_Status.BackColor = Color.Yellow;
						try
						{
							IEnumerator enumerator = this.Panel_Images.Controls.GetEnumerator();
							while (enumerator.MoveNext())
							{
								PictureBox pictureBox = (PictureBox)enumerator.Current;
								try
								{
									num++;
									this.Label_Status.Text = string.Concat(new string[]
									{
										"Loading thumbnails [",
										num.ToString().PadLeft(this.Panel_Images.Controls.Count.ToString().Length, '0'),
										"] / [",
										this.Panel_Images.Controls.Count.ToString().PadLeft(this.Panel_Images.Controls.Count.ToString().Length, '0'),
										"]"
									});
									num3++;
									if (num3 * num4 + 2 + num4 + num3 * 2 > this.Panel_Images.Width - 26)
									{
										num3 = 0;
										num2++;
									}
									Control arg_169_0 = pictureBox;
									Point location = new Point(num3 * 120 + 2 + num3 * 2, num2 * 90 + 2 + num2 * 2);
									arg_169_0.Location = location;
									if (pictureBox.Height > pictureBox.Width)
									{
										Control arg_1AC_0 = pictureBox;
										location = pictureBox.Location;
										Point location2 = new Point(location.X + 30, pictureBox.Location.Y);
										arg_1AC_0.Location = location2;
									}
									this.Check_Thread_Running();
								}
								catch (Exception expr_1BA)
								{
									ProjectData.SetProjectError(expr_1BA);
									Exception ex = expr_1BA;
									Module_MyFunctions.Update_LOG("ERROR [Relocate_Thumbnails] [" + ex.Message + "]", false, "", false);
									ProjectData.ClearProjectError();
								}
							}
						}
						finally
						{

						}
						this.Label_Status.Text = "";
						this.Label_Status.BackColor = Color.YellowGreen;
						this.Panel_Images.AutoScroll = true;
						this.Panel_Images.Refresh();
						this.PRV_Img_Refresh = false;
						if (!this.Button_Watermark_Block.Visible)
						{
							this.EnableDisableButtons(true);
						}
					}
				}
				catch (Exception expr_263)
				{
					ProjectData.SetProjectError(expr_263);
					Exception ex2 = expr_263;
					Module_MyFunctions.Update_LOG("ERROR [Relocate_Thumbnails] [" + ex2.Message + "]", false, "", false);
					ProjectData.ClearProjectError();
				}
			}
		}
		private void Button_Select_Dir_Click(object sender, EventArgs e)
		{
			Module_MyFunctions.DAT_Dialog = true;
			if (Directory.Exists(this.TextBox_Directory.Text))
			{
				this.FolderBrowserDialog_Img.SelectedPath = this.TextBox_Directory.Text;
			}
			if (this.FolderBrowserDialog_Img.ShowDialog() == DialogResult.OK)
			{
				this.TextBox_Directory.Text = this.FolderBrowserDialog_Img.SelectedPath;
				if (!this.TextBox_Directory.Text.EndsWith("\\"))
				{
					TextBox textBox_Directory = this.TextBox_Directory;
					textBox_Directory.Text += "\\";
				}
				Module_MyFunctions.Config_Save("lastDir", this.TextBox_Directory.Text, "", false, true);
				this.Refresh_Thumbnails(true);
			}
			Module_MyFunctions.DAT_Dialog = false;
		}
		private void Select_ImageBox_Navigate(object sender, MouseEventArgs e)
		{
			try
			{
				string text = Strings.Left(Conversions.ToString(NewLateBinding.LateGet(sender, null, "tag", new object[0], null, null, null)), 3);
				string text2 = Strings.Mid(Conversions.ToString(NewLateBinding.LateGet(sender, null, "tag", new object[0], null, null, null)), 5);
				string left = text;
				if (Operators.CompareString(left, "BCK", false) == 0)
				{
					string text3 = text2;
					if (text3.EndsWith("\\"))
					{
						text3 = Strings.Left(text3, checked(text3.Length - 1));
					}
					if (text3.Contains("\\"))
					{
						text3 = Strings.Left(text3, Strings.InStrRev(text3, "\\", -1, CompareMethod.Binary));
						this.TextBox_Directory.Text = text3;
						Module_MyFunctions.Config_Save("lastDir", text3, "", false, true);
						this.Refresh_Thumbnails(true);
					}
				}
				else
				{
					if (Operators.CompareString(left, "DIR", false) == 0)
					{
						this.TextBox_Directory.Text = text2;
						Module_MyFunctions.Config_Save("lastDir", text2, "", false, true);
						this.Refresh_Thumbnails(true);
					}
				}
			}
			catch (Exception expr_F7)
			{
				ProjectData.SetProjectError(expr_F7);
				Exception ex = expr_F7;
				Module_MyFunctions.Update_LOG(Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject("ERROR [Select_ImageBox_Navigate(", NewLateBinding.LateGet(sender, null, "tag", new object[0], null, null, null)), ")] ["), ex.Message), "]")), true, "", false);
				ProjectData.ClearProjectError();
			}
		}
		private void Select_ImageBox(object sender, MouseEventArgs e)
		{
			this.PictureBox_Preview.Tag = RuntimeHelpers.GetObjectValue(NewLateBinding.LateGet(sender, null, "tag", new object[0], null, null, null));
			this.Preview_Watermark(Conversions.ToString(NewLateBinding.LateGet(sender, null, "tag", new object[0], null, null, null)));
		}
		private Image Preview_Watermark(string my_ImageFile)
		{
			Image result;
			try
			{
				if (Operators.CompareString(my_ImageFile, "", false) == 0 | !File.Exists(my_ImageFile))
				{
					this.Label_SelImg.Visible = true;
					this.Label_Img_Resolution.Text = "";
					this.PRV_Img_Watermark = null;
					this.PictureBox_Preview.Image = null;
					result = null;
				}
				else
				{
					this.Label_SelImg.Visible = false;
					this.Label_Preview.Text = "Processing...";
					this.Label_Preview.BackColor = Color.Yellow;
					this.Check_Thread_Running();
					try
					{
						this.TextBox_Margin.Text = Conversions.ToString((int)Convert.ToInt16(this.TextBox_Margin.Text));
						if (Convert.ToInt16(this.TextBox_Margin.Text) < 0)
						{
							this.TextBox_Margin.Text = Conversions.ToString(0);
						}
					}
					catch (Exception expr_CA)
					{
						ProjectData.SetProjectError(expr_CA);
						this.TextBox_Margin.Text = "10";
						ProjectData.ClearProjectError();
					}
					try
					{
						this.TextBox_FontSize.Text = Conversions.ToString((int)Convert.ToInt16(this.TextBox_FontSize.Text));
						if (Convert.ToInt16(this.TextBox_FontSize.Text) < 10)
						{
							this.TextBox_FontSize.Text = Conversions.ToString(10);
						}
					}
					catch (Exception expr_131)
					{
						ProjectData.SetProjectError(expr_131);
						this.TextBox_FontSize.Text = "80";
						ProjectData.ClearProjectError();
					}
					Module_MyFunctions.Config_Save("watermarkText", this.TextBox_Watermark.Text.Replace("\r\n", "<br>"), "", false, true);
					Module_MyFunctions.Config_Save("watermarkSize", this.TextBox_FontSize.Text, "", false, true);
					Module_MyFunctions.Config_Save("watermarkMargin", this.TextBox_Margin.Text, "", false, true);
					Image image = Module_Graphics.Read_Image(my_ImageFile, true);
					try
					{
						this.Label_Img_Resolution.Text = "Image resolution: " + Conversions.ToString(image.Width) + "x" + Conversions.ToString(image.Height);
					}
					catch (Exception expr_1F0)
					{
						ProjectData.SetProjectError(expr_1F0);
						this.Label_Img_Resolution.Text = "Image resolution: unknow";
						ProjectData.ClearProjectError();
					}
					int my_FontSize;
					if (this.PRV_SizePixel)
					{
						my_FontSize = (int)Convert.ToInt16(this.TextBox_FontSize.Text);
					}
					else
					{
						my_FontSize = Conversions.ToInteger(NewLateBinding.LateGet(this.ComboBox_Size.SelectedItem, null, "my_Value", new object[0], null, null, null));
					}
					int my_Transparency = Conversions.ToInteger(NewLateBinding.LateGet(this.ComboBox_Transparency.SelectedItem, null, "my_Value", new object[0], null, null, null));
					Image image2 = Module_Watermark.Text_On_Image(image, this.TextBox_Watermark.Text, Conversions.ToString(NewLateBinding.LateGet(this.ComboBox_FontFamily.SelectedItem, null, "my_Value", new object[0], null, null, null)), my_FontSize, (FontStyle)Conversions.ToInteger(NewLateBinding.LateGet(this.ComboBox_Style.SelectedItem, null, "my_Value", new object[0], null, null, null)), this.Label_Color.BackColor, my_Transparency, Module_Watermark.DAT_Watermark_Effect, Module_Watermark.DAT_Watermark_Pos, (int)Convert.ToInt16(this.TextBox_Margin.Text));
					if (image2 != null)
					{
						this.PRV_Img_Watermark = image2;
						image = image2;
						if (image.Width > image.Height)
						{
							image = Module_Watermark.Resize_ImageBox(image, 240, 180, false);
							Control arg_345_0 = this.PictureBox_Preview;
							Point location = new Point(240, 180);
							arg_345_0.Size = (Size)location;
							Control arg_36E_0 = this.PictureBox_Preview;
							int arg_367_1 = 3;
							location = this.PictureBox_Preview.Location;
							Point location2 = new Point(arg_367_1, location.Y);
							arg_36E_0.Location = location2;
						}
						else
						{
							image = Module_Watermark.Resize_ImageBox(image, 120, 180, false);
							Control arg_39F_0 = this.PictureBox_Preview;
							Point location2 = new Point(120, 180);
							arg_39F_0.Size = (Size)location2;
							Control arg_3C9_0 = this.PictureBox_Preview;
							int arg_3C2_1 = 63;
							location2 = this.PictureBox_Preview.Location;
							Point location = new Point(arg_3C2_1, location2.Y);
							arg_3C9_0.Location = location;
						}
						this.PictureBox_Preview.Image = image;
						this.Label_Preview.Text = "Preview (click to zoom)";
						this.Label_Preview.BackColor = Color.YellowGreen;
					}
					else
					{
						this.Label_Preview.Text = "ERROR - verify the log";
						this.Label_Preview.BackColor = Color.Red;
					}
					result = image2;
				}
			}
			catch (Exception expr_423)
			{
				ProjectData.SetProjectError(expr_423);
				Exception ex = expr_423;
				Module_MyFunctions.Update_LOG(string.Concat(new string[]
				{
					"ERROR [Preview_Watermark(",
					my_ImageFile,
					")] [",
					ex.Message,
					"]"
				}), false, "", false);
				this.Label_Preview.Text = "ERROR - verify the log";
				this.Label_Preview.BackColor = Color.Red;
				this.Label_SelImg.Visible = true;
				this.PictureBox_Preview.Image = null;
				this.PRV_Img_Watermark = null;
				result = null;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		private void RBP_POS_Click(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				Module_Watermark.DAT_Watermark_Pos = Conversions.ToString(NewLateBinding.LateGet(sender, null, "tag", new object[0], null, null, null));
				Module_MyFunctions.Config_Save("watermarkPos", Conversions.ToString(NewLateBinding.LateGet(sender, null, "tag", new object[0], null, null, null)), "", false, true);
				this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
			}
		}
		private void Button_Color_Click(object sender, EventArgs e)
		{
			this.ColorDialog_Wtm.Color = this.Label_Color.BackColor;
			Module_MyFunctions.DAT_Dialog = true;
			if (this.ColorDialog_Wtm.ShowDialog() == DialogResult.OK)
			{
				Module_MyFunctions.Config_Save("textColor", Conversions.ToString(this.ColorDialog_Wtm.Color.ToArgb()), "", false, true);
				this.Label_Color.BackColor = this.ColorDialog_Wtm.Color;
				this.Check_Thread_Running();
				this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
			}
			Module_MyFunctions.DAT_Dialog = false;
		}
		private void Button_Preview_Click(object sender, EventArgs e)
		{
			if (Operators.ConditionalCompareObjectEqual(this.PictureBox_Preview.Tag, "", false))
			{
				MessageBox.Show("Select an image!", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
			}
		}
		private void PictureBox_Preview_Click(object sender, EventArgs e)
		{
			try
			{
				if (!Operators.ConditionalCompareObjectEqual(this.PictureBox_Preview.Tag, "", false))
				{
					Module_Graphics.ShowFull_Image(this.PRV_Img_Watermark, Conversions.ToString(this.PictureBox_Preview.Tag), false, false);
				}
			}
			catch (Exception expr_3A)
			{
				ProjectData.SetProjectError(expr_3A);
				Exception ex = expr_3A;
				Module_MyFunctions.my_MsgBox(ex.Message, MessageBoxIcon.Hand, false);
				ProjectData.ClearProjectError();
			}
		}
		private void CK_Effects_CheckedChanged(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				if (this.PRV_CK_Effects)
				{
					return;
				}
				this.PRV_CK_Effects = true;
				if (sender == this.CK_Shadow)
				{
					if (this.CK_Shadow.Checked)
					{
						Module_MyFunctions.Config_Save("textShadow", "Y", "", false, true);
						Module_MyFunctions.Config_Save("textOutline", "N", "", false, true);
						Module_Watermark.DAT_Watermark_Effect = "S";
						this.CK_Outline.Checked = false;
					}
					else
					{
						Module_MyFunctions.Config_Save("textShadow", "N", "", false, true);
						Module_Watermark.DAT_Watermark_Effect = "";
					}
				}
				else
				{
					if (this.CK_Outline.Checked)
					{
						Module_MyFunctions.Config_Save("textOutline", "Y", "", false, true);
						Module_MyFunctions.Config_Save("textShadow", "N", "", false, true);
						Module_Watermark.DAT_Watermark_Effect = "O";
						this.CK_Shadow.Checked = false;
					}
					else
					{
						Module_MyFunctions.Config_Save("textOutline", "N", "", false, true);
						Module_Watermark.DAT_Watermark_Effect = "";
					}
				}
				this.PRV_CK_Effects = false;
				this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
			}
		}
		private void Form_Main_Resize(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading && !this.PRV_Form_Resizing)
			{
				this.Form_Main_ResizeEnd(null, null);
			}
		}
		private void Form_Main_ResizeBegin(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				this.PRV_Form_Resizing = true;
				this.FRM_Size = this.Size;
			}
		}
		private void Form_Main_ResizeEnd(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				this.PRV_Form_Resizing = false;
				if (this.Size != this.FRM_Size)
				{
					this.Relocate_Thumbnails();
				}
				Module_MyFunctions.Config_Save("formHeight", Conversions.ToString(this.Size.Height), "", false, true);
				Module_MyFunctions.Config_Save("formWidth", Conversions.ToString(this.Size.Width), "", false, true);
				Module_MyFunctions.Config_Save("formPosX", Conversions.ToString(this.Location.X), "", false, true);
				Module_MyFunctions.Config_Save("formPosY", Conversions.ToString(this.Location.Y), "", false, true);
			}
		}
		private void CK_SaveFolder_Click(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				if (this.CK_SaveFolder.Checked)
				{
					this.PRV_SaveSubfolder = false;
					Module_MyFunctions.Config_Save("saveInFolder", "Y", "", false, true);
					this.CK_SaveSubfolder.Checked = false;
				}
				else
				{
					this.PRV_SaveSubfolder = true;
					Module_MyFunctions.Config_Save("saveInFolder", "N", "", false, true);
					this.CK_SaveSubfolder.Checked = true;
				}
			}
		}
		private void CK_SaveSubfolder_Click(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				if (this.CK_SaveSubfolder.Checked)
				{
					this.PRV_SaveSubfolder = true;
					Module_MyFunctions.Config_Save("saveInFolder", "N", "", false, true);
					this.CK_SaveFolder.Checked = false;
				}
				else
				{
					this.PRV_SaveSubfolder = false;
					Module_MyFunctions.Config_Save("saveInFolder", "Y", "", false, true);
					this.CK_SaveFolder.Checked = true;
				}
			}
		}
		private void EnableDisableButtons(bool my_Enable)
		{
			this.Button_Watermark_Image.Enabled = my_Enable;
			this.Button_Watermark_Folder.Enabled = my_Enable;
			this.Button_Select_Dir.Enabled = my_Enable;
			this.Button_Refresh_Images.Enabled = my_Enable;
			this.Button_Clean_ThumbDB.Enabled = my_Enable;
			this.TextBox_Directory.Enabled = my_Enable;
			this.TextBox_Watermark.Enabled = my_Enable;
			this.TextBox_FontSize.Enabled = my_Enable;
			this.TextBox_Margin.Enabled = my_Enable;
			this.CK_SizePixel.Enabled = my_Enable;
			this.CK_SizeRelative.Enabled = my_Enable;
			this.ComboBox_FontFamily.Enabled = my_Enable;
			this.ComboBox_Size.Enabled = my_Enable;
			this.ComboBox_Style.Enabled = my_Enable;
			this.ComboBox_Transparency.Enabled = my_Enable;
			if (this.ComboBox_Transparency.SelectedIndex == 0)
			{
				this.CK_Shadow.Enabled = my_Enable;
				this.CK_Outline.Enabled = my_Enable;
			}
			this.Label_Color.Enabled = my_Enable;
			this.RBP_BC.Enabled = my_Enable;
			this.RBP_BL.Enabled = my_Enable;
			this.RBP_BR.Enabled = my_Enable;
			this.RBP_MC.Enabled = my_Enable;
			this.RBP_ML.Enabled = my_Enable;
			this.RBP_MR.Enabled = my_Enable;
			this.RBP_TC.Enabled = my_Enable;
			this.RBP_TL.Enabled = my_Enable;
			this.RBP_TR.Enabled = my_Enable;
			this.CK_SaveFolder.Enabled = my_Enable;
			this.CK_SaveSubfolder.Enabled = my_Enable;
		}
		private void Button_Watermark_Folder_Click(object sender, EventArgs e)
		{
			if (this.PRV_FileList.Count == 0)
			{
				MessageBox.Show("No images in this directory or no directory selected!", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.EnableDisableButtons(false);
			this.Button_Watermark_Folder.Visible = false;
			this.Button_Watermark_Block.Location = this.Button_Watermark_Folder.Location;
			this.Button_Watermark_Block.Enabled = true;
			this.Button_Watermark_Block.Visible = true;
			this.PRV_BLOCK_Watermark = false;
			this.Show_Wait_Panel(this.PRV_Message_Save);
			this.Watermark_Image(this.PRV_FileList);
			this.Label_Preview.Text = "Images watermarked!";
			this.Show_Wait_Panel("");
			this.Label_Status.Text = "";
			this.EnableDisableButtons(true);
			this.Button_Watermark_Folder.Visible = true;
			this.Button_Watermark_Block.Visible = false;
		}
		private void Button_Watermark_Image_Click(object sender, EventArgs e)
		{
			if (Operators.ConditionalCompareObjectEqual(this.PictureBox_Preview.Tag, "", false))
			{
				MessageBox.Show("Select an image!", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			this.EnableDisableButtons(false);
			this.PRV_BLOCK_Watermark = false;
			this.Watermark_Image(new ArrayList
			{
				RuntimeHelpers.GetObjectValue(this.PictureBox_Preview.Tag)
			});
			this.Label_Preview.Text = "Image watermarked!";
			this.EnableDisableButtons(true);
		}
		private bool Watermark_Image(ArrayList my_Files)
		{
			checked
			{
				bool result;
				try
				{
					string text = "";
					int num = 0;
					try
					{
						IEnumerator enumerator = my_Files.GetEnumerator();
						while (enumerator.MoveNext())
						{
							text = Conversions.ToString(enumerator.Current);
							if (Module_MyFunctions.PRG_Exit | this.PRV_BLOCK_Watermark)
							{
								result = true;
								return result;
							}
							try
							{
								num++;
								this.Label_Status.Text = string.Concat(new string[]
								{
									"Watermarking images [",
									num.ToString().PadLeft(my_Files.Count.ToString().Length, '0'),
									"] / [",
									my_Files.Count.ToString().PadLeft(my_Files.Count.ToString().Length, '0'),
									"]"
								});
								if (File.Exists(text))
								{
									string text2 = Path.GetDirectoryName(text);
									if (this.CK_SaveSubfolder.Checked)
									{
										text2 += "\\watermark\\";
										if (!Directory.Exists(text2))
										{
											Directory.CreateDirectory(text2);
										}
									}
									else
									{
										text2 += "\\";
									}
									text2 = text2 + Path.GetFileNameWithoutExtension(text) + "_wm.jpg";
									Image image = this.Preview_Watermark(text);
									if (image != null)
									{
										ImageCodecInfo encoder = this.GetEncoder(ImageFormat.Jpeg);
										EncoderParameters encoderParameters = new EncoderParameters(1);
										encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
										image.Save(text2, encoder, encoderParameters);
									}
									try
									{
										this.ProgressBar_Wait.Value = (int)Math.Round(Math.Floor((double)(num * 100) / (double)my_Files.Count));
									}
									catch (Exception expr_187)
									{
										ProjectData.SetProjectError(expr_187);
										ProjectData.ClearProjectError();
									}
									this.Check_Thread_Running();
								}
							}
							catch (Exception expr_19F)
							{
								ProjectData.SetProjectError(expr_19F);
								Exception ex = expr_19F;
								Module_MyFunctions.Update_LOG(string.Concat(new string[]
								{
									"ERROR [Watermark_Image(",
									text,
									")] [",
									ex.Message,
									"]"
								}), true, "", false);
								ProjectData.ClearProjectError();
							}
						}
					}
					finally
					{

					}
					this.Label_Status.Text = "";
					result = true;
				}
				catch (Exception expr_22C)
				{
					ProjectData.SetProjectError(expr_22C);
					Exception ex2 = expr_22C;
					Module_MyFunctions.Update_LOG("ERROR [Watermark_Image] [" + ex2.Message + "]", true, "", false);
					this.Label_Status.Text = "";
					result = false;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		private ImageCodecInfo GetEncoder(ImageFormat format)
		{
			ImageCodecInfo[] imageDecoders = ImageCodecInfo.GetImageDecoders();
			ImageCodecInfo[] array = imageDecoders;
			checked
			{
				for (int i = 0; i < array.Length; i++)
				{
					ImageCodecInfo imageCodecInfo = array[i];
					if (imageCodecInfo.FormatID == format.Guid)
					{
						return imageCodecInfo;
					}
				}
				return null;
			}
		}
		private void CK_SizePixel_CheckedChanged(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				if (this.CK_SizePixel.Checked)
				{
					this.PRV_SizePixel = true;
					Module_MyFunctions.Config_Save("fontSizePixel", "Y", "", false, true);
					this.CK_SizeRelative.Checked = false;
				}
				else
				{
					this.PRV_SizePixel = false;
					Module_MyFunctions.Config_Save("fontSizePixel", "N", "", false, true);
					this.CK_SizeRelative.Checked = true;
				}
				this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
			}
		}
		private void CK_SizeRelative_CheckedChanged(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				if (this.CK_SizeRelative.Checked)
				{
					this.PRV_SizePixel = false;
					Module_MyFunctions.Config_Save("fontSizePixel", "N", "", false, true);
					this.CK_SizePixel.Checked = false;
				}
				else
				{
					this.PRV_SizePixel = true;
					Module_MyFunctions.Config_Save("fontSizePixel", "Y", "", false, true);
					this.CK_SizePixel.Checked = true;
				}
				this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
			}
		}
		private void ComboBox_Size_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				Module_MyFunctions.Config_Save("watermarkSizeRelative", Conversions.ToString(Operators.MultiplyObject(NewLateBinding.LateGet(this.ComboBox_Size.SelectedItem, null, "my_Value", new object[0], null, null, null), -1)), "", false, true);
				if (this.CK_SizeRelative.Checked)
				{
					this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
				}
			}
		}
		private void ComboBox_FontFamily_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				Module_MyFunctions.Config_Save("fontFamily", Conversions.ToString(NewLateBinding.LateGet(this.ComboBox_FontFamily.SelectedItem, null, "my_Value", new object[0], null, null, null)), "", false, true);
				this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
			}
		}
		private void ComboBox_Style_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				Module_MyFunctions.Config_Save("watermarkStyle", NewLateBinding.LateGet(this.ComboBox_Style.SelectedItem, null, "my_Value", new object[0], null, null, null).ToString(), "", false, true);
				this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
			}
		}
		private void ComboBox_Transparency_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.ComboBox_Transparency.SelectedIndex == 0)
			{
				this.CK_Shadow.Enabled = true;
				this.CK_Outline.Enabled = true;
				this.Label_Effects.Text = "";
			}
			else
			{
				this.CK_Shadow.Enabled = false;
				this.CK_Outline.Enabled = false;
				this.Label_Effects.Text = "Effects disabled (transparency enabled)";
			}
			if (!Module_MyFunctions.PRG_Loading)
			{
				Module_MyFunctions.Config_Save("watermarkTransparency", NewLateBinding.LateGet(this.ComboBox_Transparency.SelectedItem, null, "my_Value", new object[0], null, null, null).ToString(), "", false, true);
				this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
			}
		}
		private void Button_Updates_Click(object sender, EventArgs e)
		{
			bool arg_05_0 = false;
			NotifyIcon notifyIcon = null;
			Module_Internet.WEB_CheckUpdate(arg_05_0, notifyIcon);
		}
		private void Button_About_Click(object sender, EventArgs e)
		{
			MyProject.Forms.Form_About.Show();
		}
		private void Button_Donate_Click(object sender, EventArgs e)
		{
			MyProject.Forms.Form_Donate.Show();
		}
		private void Button_BrowseFolder_Click(object sender, EventArgs e)
		{
			string text = this.TextBox_Directory.Text;
			if (!text.EndsWith("\\"))
			{
				text += "\\";
			}
			if (this.CK_SaveSubfolder.Checked & Directory.Exists(text + "watermark\\"))
			{
				Process.Start(text + "watermark\\");
			}
			else
			{
				if (Directory.Exists(text))
				{
					Process.Start(text);
				}
				else
				{
					Module_MyFunctions.my_MsgBox("The selected directory does not exists!", MessageBoxIcon.Exclamation, false);
				}
			}
		}
		private void TextBox_FontSize_TextChanged(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading & this.CK_SizePixel.Checked)
			{
				this.PRV_AutoApply = 3;
				this.Timer_AutoApply.Enabled = true;
				this.Timer_AutoApply_Tick(null, null);
			}
		}
		private void TextBox_Margin_TextChanged(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				this.PRV_AutoApply = 3;
				this.Timer_AutoApply.Enabled = true;
				this.Timer_AutoApply_Tick(null, null);
			}
		}
		private void TextBox_Watermark_TextChanged(object sender, EventArgs e)
		{
			if (!Module_MyFunctions.PRG_Loading)
			{
				this.PRV_AutoApply = 3;
				this.Timer_AutoApply.Enabled = true;
				this.Timer_AutoApply_Tick(null, null);
			}
		}
		private void Timer_AutoApply_Tick(object sender, EventArgs e)
		{
			this.Timer_AutoApply.Enabled = false;
			checked
			{
				this.PRV_AutoApply--;
				if (this.PRV_AutoApply < 1)
				{
					this.Label_Preview.Text = "Preview (click to zoom)";
					this.Preview_Watermark(Conversions.ToString(this.PictureBox_Preview.Tag));
				}
				else
				{
					this.Label_Preview.Text = "Auto preview in " + Conversions.ToString(this.PRV_AutoApply);
					this.Timer_AutoApply.Enabled = true;
				}
			}
		}
		private void Button_Clean_ThumbDB_Click(object sender, EventArgs e)
		{
			if (Module_MyFunctions.my_MsgBox("Delete all stored thumbnails?", MessageBoxIcon.Question, true) == DialogResult.No)
			{
				return;
			}
			this.Show_Wait_Panel(this.PRV_Message_Clean);
			try
			{
				Directory.Delete(Module_Watermark.DAT_ThumbnailsDB, true);
			}
			catch (Exception expr_2A)
			{
				ProjectData.SetProjectError(expr_2A);
				Exception ex = expr_2A;
				Module_MyFunctions.Update_LOG("ERROR [Button_Clean_ThumbDB_Click.Delete] [" + ex.Message + "]", false, "", false);
				ProjectData.ClearProjectError();
			}
			try
			{
				if (!Directory.Exists(Module_Watermark.DAT_ThumbnailsDB))
				{
					Directory.CreateDirectory(Module_Watermark.DAT_ThumbnailsDB);
				}
			}
			catch (Exception expr_72)
			{
				ProjectData.SetProjectError(expr_72);
				Exception ex2 = expr_72;
				Module_MyFunctions.Update_LOG("ERROR [Button_Clean_ThumbDB_Click.Create] [" + ex2.Message + "]", true, "", false);
				ProjectData.ClearProjectError();
			}
			this.Show_Wait_Panel("");
		}
		private void Clean_ThumbDB()
		{
			this.Show_Wait_Panel(this.PRV_Message_Clean);
			checked
			{
				try
				{
					string[] files = Directory.GetFiles(Module_Watermark.DAT_ThumbnailsDB);
					ArrayList arrayList = new ArrayList();
					int i = 0;
					if (files.Length > Module_Watermark.DAT_ThumbnailsDB_Size)
					{
						string[] array = files;
						for (int j = 0; j < array.Length; j++)
						{
							string text = array[j];
							try
							{
								i++;
								DateTime creationTime = new FileInfo(text).CreationTime;
								string str = string.Concat(new string[]
								{
									Conversions.ToString(creationTime.Year),
									creationTime.Month.ToString().PadLeft(2, '0'),
									creationTime.Day.ToString().PadLeft(2, '0'),
									creationTime.Hour.ToString().PadLeft(2, '0'),
									creationTime.Minute.ToString().PadLeft(2, '0'),
									creationTime.Second.ToString().PadLeft(2, '0')
								});
								arrayList.Add(str + "|" + Path.GetFileName(text));
								try
								{
									this.ProgressBar_Wait.Value = Convert.ToInt32(decimal.Divide(Math.Floor(new decimal(i * 100)), new decimal(files.Length)));
								}
								catch (Exception expr_14C)
								{
									ProjectData.SetProjectError(expr_14C);
									ProjectData.ClearProjectError();
								}
							}
							catch (Exception expr_15B)
							{
								ProjectData.SetProjectError(expr_15B);
								Exception ex = expr_15B;
								Module_MyFunctions.Update_LOG("ERROR [Clean_ThumbDB.ArrayAdd] [" + ex.Message + "]", false, "", false);
								ProjectData.ClearProjectError();
							}
						}
						arrayList.Sort();
						int arg_1B4_0 = 0;
						int num = arrayList.Count - 1 - Module_Watermark.DAT_ThumbnailsDB_Size;
						for (i = arg_1B4_0; i <= num; i++)
						{
							try
							{
								string[] array2 = arrayList[i].ToString().Split(new char[]
								{
									'|'
								});
								File.Delete(Module_Watermark.DAT_ThumbnailsDB + array2[1]);
								this.Label_Status.Text = "Cleaning [" + Conversions.ToString(arrayList.Count - Module_Watermark.DAT_ThumbnailsDB_Size - i) + "]";
								try
								{
									this.ProgressBar_Wait.Value = Convert.ToInt32(decimal.Divide(Math.Floor(new decimal(i * 100)), new decimal(arrayList.Count - 1 - Module_Watermark.DAT_ThumbnailsDB_Size)));
								}
								catch (Exception expr_259)
								{
									ProjectData.SetProjectError(expr_259);
									ProjectData.ClearProjectError();
								}
								Application.DoEvents();
							}
							catch (Exception expr_26F)
							{
								ProjectData.SetProjectError(expr_26F);
								Exception ex2 = expr_26F;
								Module_MyFunctions.Update_LOG("ERROR [Clean_ThumbDB.Delete] [" + ex2.Message + "]", false, "", false);
								ProjectData.ClearProjectError();
							}
						}
					}
				}
				catch (Exception expr_2B1)
				{
					ProjectData.SetProjectError(expr_2B1);
					Exception ex3 = expr_2B1;
					Module_MyFunctions.Update_LOG("ERROR [Clean_ThumbDB] [" + ex3.Message + "]", false, "", false);
					ProjectData.ClearProjectError();
				}
				this.Show_Wait_Panel("");
			}
		}
		private void CK_ThumbDB_Click(object sender, EventArgs e)
		{
			if (this.CK_ThumbDB.Checked)
			{
				Module_MyFunctions.Config_Save("thumbDB", "Y", "", false, true);
				Module_Watermark.DAT_Thumbnails = true;
			}
			else
			{
				Module_MyFunctions.Config_Save("thumbDB", "N", "", false, true);
				Module_Watermark.DAT_Thumbnails = false;
			}
		}
		private void Show_Wait_Panel(string my_Msg = "")
		{
			this.ProgressBar_Wait.Value = 0;
			if (Operators.CompareString(my_Msg, "", false) == 0)
			{
				this.Panel_Wait.Visible = false;
			}
			else
			{
				this.Panel_Wait.Visible = true;
				Control arg_58_0 = this.Panel_Wait;
				Point location = new Point(this.Panel_Wait.Location.X, 99);
				arg_58_0.Location = location;
				this.Label_Wait.Text = my_Msg;
			}
			this.Check_Thread_Running();
		}
		private void Button_Watermark_Block_Click(object sender, EventArgs e)
		{
			this.Button_Watermark_Block.Enabled = false;
			this.PRV_BLOCK_Watermark = true;
		}
		private bool Check_Thread_Running()
		{
			checked
			{
				bool result;
				try
				{
					bool flag = false;
					int arg_0B_0 = 0;
					int tHREAD_Totals = this.THREAD_Totals;
					for (int i = arg_0B_0; i <= tHREAD_Totals; i++)
					{
						if (!this.THREAD_Stopped[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Application.DoEvents();
					}
					result = flag;
				}
				catch (Exception expr_31)
				{
					ProjectData.SetProjectError(expr_31);
					result = true;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		private void Timer_Close_Tick(object sender, EventArgs e)
		{
			try
			{
				Module_MyFunctions.PRG_Exit = true;
				Application.Exit();
			}
			catch (Exception expr_0D)
			{
				ProjectData.SetProjectError(expr_0D);
				ProjectData.ClearProjectError();
			}
		}
		[DebuggerStepThrough, CompilerGenerated]
		private void _Lambda__1(object a0)
		{
			this.Thread_Refresh_Thumbnails(Conversions.ToInteger(a0));
		}
	}
}
