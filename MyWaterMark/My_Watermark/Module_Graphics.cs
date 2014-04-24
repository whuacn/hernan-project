using Microsoft.VisualBasic.CompilerServices;
using My_Watermark.My;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace My_Watermark
{
	[StandardModule]
	internal sealed class Module_Graphics
	{
		public enum EXIF_Orientations : byte
		{
			Unknown,
			TopLeft,
			TopRight,
			BottomRight,
			BottomLeft,
			LeftTop,
			RightTop,
			RightBottom,
			LeftBottom
		}
		public static Form FORM_FullImage;
		private const int EXIF_OrientationId = 274;
		public static Module_Graphics.EXIF_Orientations ImageOrientation(Image my_Image)
		{
			Module_Graphics.EXIF_Orientations result;
			try
			{
				int num = Array.IndexOf<int>(my_Image.PropertyIdList, 274);
				if (num < 0)
				{
					result = Module_Graphics.EXIF_Orientations.Unknown;
				}
				else
				{
					result = (Module_Graphics.EXIF_Orientations)my_Image.GetPropertyItem(274).Value[0];
				}
			}
			catch (Exception expr_2E)
			{
				ProjectData.SetProjectError(expr_2E);
				result = Module_Graphics.EXIF_Orientations.Unknown;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static Image EXIF_RotateImage(Image my_Image)
		{
			Image result;
			try
			{
				Module_Graphics.EXIF_Orientations eXIF_Orientations = Module_Graphics.ImageOrientation(my_Image);
				Bitmap bitmap = new Bitmap(my_Image);
				switch (eXIF_Orientations)
				{
				case Module_Graphics.EXIF_Orientations.BottomRight:
					bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
					break;
				case Module_Graphics.EXIF_Orientations.LeftTop:
					bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
					break;
				case Module_Graphics.EXIF_Orientations.RightTop:
					bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
					break;
				case Module_Graphics.EXIF_Orientations.RightBottom:
					bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
					break;
				case Module_Graphics.EXIF_Orientations.LeftBottom:
					bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
					break;
				}
				result = bitmap;
			}
			catch (Exception expr_5F)
			{
				ProjectData.SetProjectError(expr_5F);
				result = my_Image;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static Image Read_Image(string my_ImageFile, bool my_AutoRotate = true)
		{
			FileStream fileStream = null;
			checked
			{
				Image result;
				try
				{
					int num = 0;
					while (true)
					{
						num++;
						try
						{
							fileStream = new FileStream(my_ImageFile, FileMode.Open, FileAccess.Read);
							break;
						}
						catch (Exception expr_13)
						{
							ProjectData.SetProjectError(expr_13);
							Exception ex = expr_13;
							Thread.Sleep(250);
							Application.DoEvents();
							if (num > 2)
							{
								Module_MyFunctions.Update_LOG("ERROR [Read_Image.FileStream] [" + ex.Message + "]", false, "", false);
								result = null;
								ProjectData.ClearProjectError();
								return result;
							}
							ProjectData.ClearProjectError();
						}
					}
					Image image = Image.FromStream(fileStream);
					if (my_AutoRotate)
					{
						image = Module_Graphics.EXIF_RotateImage(image);
					}
					fileStream.Close();
					fileStream.Dispose();
					result = image;
				}
				catch (Exception expr_86)
				{
					ProjectData.SetProjectError(expr_86);
					Exception ex2 = expr_86;
					Module_MyFunctions.Update_LOG("ERROR [Read_Image] [" + ex2.Message + "]", false, "", false);
					try
					{
						if (fileStream != null)
						{
							fileStream.Close();
						}
					}
					catch (Exception expr_BB)
					{
						ProjectData.SetProjectError(expr_BB);
						Exception ex3 = expr_BB;
						Module_MyFunctions.Update_LOG("ERROR [Read_Image.closeStream] [" + ex3.Message + "]", false, "", false);
						ProjectData.ClearProjectError();
					}
					result = null;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static bool ShowFull_Image(string my_ImageFile, string my_Title, bool my_ShowMenu = true, bool my_EnlargeImage = true, object my_AssociateObject = null)
		{
			if (Operators.CompareString(my_ImageFile, "", false) == 0 | !File.Exists(my_ImageFile))
			{
				MessageBox.Show("Image [" + my_ImageFile + "] not found!", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return false;
			}
			Image image = Module_Graphics.Read_Image(my_ImageFile, true);
			Module_Graphics.Show_Image_FS(image, my_ImageFile, my_Title, my_ShowMenu, my_EnlargeImage, my_AssociateObject);			
			return true;
		}
		public static bool ShowFull_Image(Image my_Image, string my_Title, bool my_ShowMenu = true, bool my_EnlargeImage = true)
		{
			string arg_0D_1 = "";
			object obj = null;
			Module_Graphics.Show_Image_FS(my_Image, arg_0D_1, my_Title, my_ShowMenu, my_EnlargeImage, obj);
			return true;
		}
		private static bool Show_Image_FS(Image my_Image, string my_ImageFile, string my_Title, bool my_ShowMenu = true, bool my_EnlargeImage = true, object my_AssociateObject = null)
		{
			checked
			{
				bool result;
				try
				{
					Module_Graphics.FORM_FullImage = new Form();
					Module_Graphics.FORM_FullImage.Tag = "";
					if (my_Image == null)
					{
						result = true;
					}
					else
					{
						PictureBox pictureBox = new PictureBox();
						Module_Graphics.FORM_FullImage.Visible = false;
						Module_Graphics.FORM_FullImage.ShowInTaskbar = false;
						Module_Graphics.FORM_FullImage.BackColor = Color.Black;
						Module_Graphics.FORM_FullImage.Text = my_Title;
						Module_Graphics.FORM_FullImage.Icon = MyProject.Forms.Form_Main.Icon;
						Module_Graphics.FORM_FullImage.FormBorderStyle = FormBorderStyle.None;
						Module_Graphics.FORM_FullImage.WindowState = FormWindowState.Maximized;
						if (my_ShowMenu)
						{
							Module_Graphics.FORM_FullImage.ContextMenuStrip = MyProject.Forms.Form_Main.CMS_FullImage;
						}
						else
						{
							Module_Graphics.FORM_FullImage.ContextMenuStrip = null;
						}
						Module_Graphics.FORM_FullImage.StartPosition = FormStartPosition.Manual;
						Application.DoEvents();
						PictureBox pictureBox2 = pictureBox;
						Control arg_D2_0 = pictureBox2;
						Point location = new Point(0, 0);
						arg_D2_0.Location = location;
						pictureBox2.Cursor = Cursors.Hand;
						pictureBox2.Image = my_Image;
						pictureBox2.Tag = my_ImageFile;
						Module_Graphics.FORM_FullImage.Text = string.Concat(new string[]
						{
							Conversions.ToString(pictureBox2.Width),
							"x",
							Conversions.ToString(pictureBox2.Height),
							" - ",
							Module_Graphics.FORM_FullImage.Text
						});
						pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
						int num;
						int num2;
						int x = 0;
						int y = 0;
						if (!my_EnlargeImage & pictureBox2.Image.Width <= Screen.PrimaryScreen.Bounds.Width & pictureBox2.Image.Height <= Screen.PrimaryScreen.Bounds.Height)
						{
							num = pictureBox2.Image.Width;
							num2 = pictureBox2.Image.Height;
							x = Convert.ToInt32((double)(Screen.PrimaryScreen.Bounds.Width - num) / 2.0);
							y = Convert.ToInt32((double)(Screen.PrimaryScreen.Bounds.Height - num2) / 2.0);
						}
						else
						{
							if (pictureBox2.Image.Width > pictureBox2.Image.Height)
							{
								num = Screen.PrimaryScreen.Bounds.Width;
								num2 = Convert.ToInt32(Math.Round((double)(num * pictureBox2.Image.Height) / (double)pictureBox2.Image.Width));
								if (num2 > Screen.PrimaryScreen.Bounds.Height)
								{
									num2 = Screen.PrimaryScreen.Bounds.Height;
									num = Convert.ToInt32(Math.Round((double)(num2 * pictureBox2.Image.Width) / (double)pictureBox2.Image.Height));
									x = Convert.ToInt32((double)(Screen.PrimaryScreen.Bounds.Width - num) / 2.0);
								}
								else
								{
									y = Convert.ToInt32((double)(Screen.PrimaryScreen.Bounds.Height - num2) / 2.0);
								}
							}
							else
							{
								num2 = Screen.PrimaryScreen.Bounds.Height;
								num = Convert.ToInt32(Math.Round((double)(num2 * pictureBox2.Image.Width) / (double)pictureBox2.Image.Height));
								x = Convert.ToInt32((double)(Screen.PrimaryScreen.Bounds.Width - num) / 2.0);
							}
						}
						pictureBox2.Width = num;
						pictureBox2.Height = num2;
						Control arg_395_0 = pictureBox2;
						location = new Point(x, y);
						arg_395_0.Location = location;
						Form arg_3CE_0 = Module_Graphics.FORM_FullImage;
						Size size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
						arg_3CE_0.Size = size;
						Application.DoEvents();
						Form arg_3E8_0 = Module_Graphics.FORM_FullImage;
						location = new Point(0, 0);
						arg_3E8_0.Location = location;
						Module_Graphics.FORM_FullImage.Controls.Add(pictureBox);
						Module_Graphics.FORM_FullImage.Cursor = Cursors.Hand;
						Module_Graphics.FORM_FullImage.Visible = true;
						pictureBox2.MouseClick += new MouseEventHandler(Module_Graphics.Full_Image_Click);
						Module_Graphics.FORM_FullImage.MouseClick += new MouseEventHandler(Module_Graphics.Full_Form_Click);
						Module_Graphics.FORM_FullImage.KeyUp += new KeyEventHandler(Module_Graphics.Full_Image_Keypress);
						Module_Graphics.FORM_FullImage.TopMost = true;
						Application.DoEvents();
						Module_Graphics.FORM_FullImage.Tag = my_ImageFile;
						try
						{
							if (my_AssociateObject != null)
							{
								my_AssociateObject = pictureBox;
							}
						}
						catch (Exception expr_47F)
						{
							ProjectData.SetProjectError(expr_47F);
							Exception ex = expr_47F;
							Module_MyFunctions.Update_LOG("ERROR [ShowFull_Image(" + my_ImageFile + ").associate] " + ex.Message, true, "", false);
							ProjectData.ClearProjectError();
						}
						result = true;
					}
				}
				catch (Exception expr_4B5)
				{
					ProjectData.SetProjectError(expr_4B5);
					Exception ex2 = expr_4B5;
					Module_MyFunctions.Update_LOG("ERROR [ShowFull_Image(" + my_ImageFile + ")] " + ex2.Message, true, "", false);
					result = false;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		private static void Full_Image_Click(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left)
				{
					NewLateBinding.LateCall(NewLateBinding.LateGet(sender, null, "parent", new object[0], null, null, null), null, "dispose", new object[0], null, null, null, true);
				}
				else
				{
					if (e.Button == MouseButtons.Right && Module_Graphics.FORM_FullImage.ContextMenuStrip == null)
					{
						NewLateBinding.LateCall(NewLateBinding.LateGet(sender, null, "parent", new object[0], null, null, null), null, "dispose", new object[0], null, null, null, true);
					}
				}
			}
			catch (Exception expr_80)
			{
				ProjectData.SetProjectError(expr_80);
				Exception ex = expr_80;
				Module_MyFunctions.Update_LOG("ERROR [Module_Graphics.Full_Image_Click()] " + ex.Message, true, "", false);
				ProjectData.ClearProjectError();
			}
		}
		private static void Full_Form_Click(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left | (e.Button == MouseButtons.Right & Module_Graphics.FORM_FullImage.ContextMenuStrip == null))
				{
					NewLateBinding.LateCall(sender, null, "dispose", new object[0], null, null, null, true);
				}
			}
			catch (Exception expr_44)
			{
				ProjectData.SetProjectError(expr_44);
				Exception ex = expr_44;
				Module_MyFunctions.Update_LOG("ERROR [Module_Graphics.Full_Form_Click()] " + ex.Message, true, "", false);
				ProjectData.ClearProjectError();
			}
		}
		private static void Full_Image_Keypress(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Escape | e.KeyCode == Keys.Return)
				{
					NewLateBinding.LateCall(sender, null, "dispose", new object[0], null, null, null, true);
				}
			}
			catch (Exception expr_30)
			{
				ProjectData.SetProjectError(expr_30);
				ProjectData.ClearProjectError();
			}
		}
		public static bool Copy_Image_to_Clipboard(string my_ImageFile, bool my_ShowError = true)
		{
			checked
			{
				bool result = true;
				try
				{
					Image image = null;
					if (File.Exists(my_ImageFile))
					{
						image = Module_Graphics.Read_Image(my_ImageFile, true);
						if (image == null)
						{
							if (my_ShowError)
							{
								Module_MyFunctions.my_MsgBox("Cannot read image:\r\n\r\n" + my_ImageFile + "\r\n\r\nverify the log!", MessageBoxIcon.Hand, false);
							}
						}
						else
						{
							int i = 0;
							while (i < 3)
							{
								try
								{
									Clipboard.Clear();
									Clipboard.SetImage(image);
									image.Dispose();
									image = null;
									return result;
								}
								catch (Exception expr_68)
								{
									ProjectData.SetProjectError(expr_68);
									Thread.Sleep(500);
									Application.DoEvents();
									i++;
									ProjectData.ClearProjectError();
								}
							}
							if (my_ShowError)
							{
								Module_MyFunctions.my_MsgBox("Cannot copy image to clipboard, please retry!", MessageBoxIcon.Hand, false);
							}
						}
					}
					else
					{
						result = false;
					}
				}
				catch (Exception expr_8B)
				{
					ProjectData.SetProjectError(expr_8B);
					Exception ex = expr_8B;
					Module_MyFunctions.Update_LOG("ERROR [Module_Graphics.Copy_Image_to_Clipboard()] " + ex.Message, false, "", false);
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
	}
}
