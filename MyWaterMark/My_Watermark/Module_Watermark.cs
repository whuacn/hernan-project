using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace My_Watermark
{
	[StandardModule]
	internal sealed class Module_Watermark
	{
		public static byte[] CFG_PwdBytes = new byte[]
		{
			52,
			124,
			77,
			94,
			206,
			221,
			89,
			36,
			25,
			10,
			48,
			155,
			128,
			172,
			15,
			94
		};
		public static string PRG_Name = "My Watermark";
		public static string PRG_Version = "2.21";
		public static string PRG_Title = "";
		public static bool PRG_PayPal_Logo = true;
		public static int PRG_Donate_Days = 1;
		public static string PRG_Donate_WEBHTML = "";
		public static string PRG_APIKEY = "";
		public static string PRG_KEY_DataFile = "mywatermark.txt";
		public static string PRG_KEY_Version = "#mywatermark.ver:|#";
		public static string PRG_KEY_MD5 = "#mywatermark.md5_";
		public static string PRG_KEY_UPD_URL = "#mywatermark.upd:|#";
		public static string PRG_KEY_UPD_DAT = "#mywatermark.upd.dat:|#";
		public static string PRG_KEY_UPD_MD5 = "#mywatermark.upd.md5:|#";
		public static string PRG_KEY_UPD_NEW = "#mywatermark.upd.new:|#";
		public static string PRG_KEY_LIC_URL = "#mywatermark.lic.url:|#";
		public static string PRG_KEY_LIC_BlackList = "#mywatermark.lic.blacklist:|#";
		public static string PRG_LIC_URL_Main = "";
		public static string PRG_LIC_URL_Mirror = "";
		public static string PRG_LIC_BlackList = "";
		public static string PRG_KEY_WebName = "mywatermark";
		public static string DAT_Icons = "Icons by: Visual Idiot - http://visualidiot.com";
		public static string DAT_Icons_URL = "http://visualidiot.com";
		public static string DAT_Icons2 = "Gnome Project - http://art.gnome.org/themes/icon";
		public static string DAT_Icons_URL2 = "http://art.gnome.org/themes/icon";
		public static string DAT_Watermark_Pos;
		public static string DAT_Watermark_Effect;
		public static bool DAT_Thumbnails;
		public static string DAT_ThumbnailsDB;
		public static int DAT_ThumbnailsDB_Size = 10000;
		public static Image Text_On_Image(Image my_Image, string my_Text, string my_FontFamily, int my_FontSize, FontStyle my_FontStyle, Color my_Color, int my_Transparency, string my_Effect, string my_Position, int my_Margin)
		{
			checked
			{
				Image result;
				try
				{
					Image image = my_Image;
					SolidBrush brush = new SolidBrush(Color.FromArgb(my_Transparency, (int)my_Color.R, (int)my_Color.G, (int)my_Color.B));
					bool flag = false;
					Size newSize = new Size();
					newSize.Height = image.Height;
					newSize.Width = image.Width;
					Bitmap bitmap = new Bitmap(image, newSize);
					Graphics graphics = Graphics.FromImage(bitmap);
					int num3;
					if (my_FontSize < 0)
					{
						my_FontSize *= -1;
						int num = (int)Math.Round((double)(newSize.Width * my_FontSize) / 100.0);
						int num2 = 10;
						while (!Module_MyFunctions.PRG_Exit)
						{
							Font font = null;
							flag = false;
							try
							{
								font = new Font(my_FontFamily, (float)num2, my_FontStyle, GraphicsUnit.Pixel);
							}
							catch (Exception expr_A7)
							{
								ProjectData.SetProjectError(expr_A7);
								flag = true;
								my_FontStyle = FontStyle.Regular;
								ProjectData.ClearProjectError();
							}
							try
							{
								if (flag)
								{
									font = new Font(my_FontFamily, (float)num2, my_FontStyle, GraphicsUnit.Pixel);
								}
							}
							catch (Exception expr_CE)
							{
								ProjectData.SetProjectError(expr_CE);
								ProjectData.ClearProjectError();
							}
							if (graphics.MeasureString(my_Text, font).Width > (float)num)
							{
								num3 = num2;
								goto IL_108;
							}
							num2++;
						}
						result = null;
						return result;
					}
					num3 = my_FontSize;
					IL_108:
					Font font2 = null;
					flag = false;
					try
					{
						font2 = new Font(my_FontFamily, (float)num3, my_FontStyle, GraphicsUnit.Pixel);
					}
					catch (Exception expr_11B)
					{
						ProjectData.SetProjectError(expr_11B);
						flag = true;
						my_FontStyle = FontStyle.Regular;
						ProjectData.ClearProjectError();
					}
					try
					{
						if (flag)
						{
							font2 = new Font(my_FontFamily, (float)num3, my_FontStyle, GraphicsUnit.Pixel);
						}
					}
					catch (Exception expr_141)
					{
						ProjectData.SetProjectError(expr_141);
						ProjectData.ClearProjectError();
					}
					SizeF sizeF = graphics.MeasureString(my_Text, font2);
					bool flag2 = false;
					int num4;
					int num5;
					if (Operators.CompareString(my_Position, "TL", false) == 0)
					{
						num4 = my_Margin * 2;
						num5 = my_Margin * 2;
					}
					else
					{
						if (Operators.CompareString(my_Position, "TC", false) == 0)
						{
							num4 = (int)Math.Round((double)unchecked(((float)image.Size.Width - sizeF.Width) / 2f + (float)my_Margin));
							num5 = my_Margin * 2;
						}
						else
						{
							if (Operators.CompareString(my_Position, "TR", false) == 0)
							{
								num4 = (int)Math.Round((double)unchecked((float)image.Size.Width - sizeF.Width));
								num5 = my_Margin * 2;
							}
							else
							{
								if (Operators.CompareString(my_Position, "ML", false) == 0)
								{
									num4 = my_Margin * 2;
									num5 = (int)Math.Round((double)unchecked(((float)image.Size.Height - sizeF.Height) / 2f + (float)my_Margin));
								}
								else
								{
									if (Operators.CompareString(my_Position, "MC", false) == 0)
									{
										num4 = (int)Math.Round((double)unchecked(((float)image.Size.Width - sizeF.Width) / 2f + (float)my_Margin));
										num5 = (int)Math.Round((double)unchecked(((float)image.Size.Height - sizeF.Height) / 2f + (float)my_Margin));
									}
									else
									{
										if (Operators.CompareString(my_Position, "MR", false) == 0)
										{
											num4 = (int)Math.Round((double)unchecked((float)image.Size.Width - sizeF.Width));
											num5 = (int)Math.Round((double)unchecked(((float)image.Size.Height - sizeF.Height) / 2f + (float)my_Margin));
										}
										else
										{
											if (Operators.CompareString(my_Position, "BL", false) == 0)
											{
												num4 = my_Margin * 2;
												num5 = (int)Math.Round((double)unchecked((float)image.Size.Height - sizeF.Height));
												flag2 = true;
											}
											else
											{
												if (Operators.CompareString(my_Position, "BC", false) == 0)
												{
													num4 = (int)Math.Round((double)unchecked(((float)image.Size.Width - sizeF.Width) / 2f + (float)my_Margin));
													num5 = (int)Math.Round((double)unchecked((float)image.Size.Height - sizeF.Height));
													flag2 = true;
												}
												else
												{
													num4 = (int)Math.Round((double)unchecked((float)image.Size.Width - sizeF.Width));
													num5 = (int)Math.Round((double)unchecked((float)image.Size.Height - sizeF.Height));
													flag2 = true;
												}
											}
										}
									}
								}
							}
						}
					}
					if (Operators.CompareString(my_Effect, "", false) != 0 & my_Transparency == 255)
					{
						graphics.DrawString(my_Text, font2, new SolidBrush(Color.Black), (float)(num4 - (my_Margin - 2)), (float)(num5 - (my_Margin - 2)));
						if (Operators.CompareString(my_Effect, "O", false) == 0)
						{
							graphics.DrawString(my_Text, font2, new SolidBrush(Color.Black), (float)(num4 - my_Margin - 2), (float)(num5 - my_Margin - 2));
							graphics.DrawString(my_Text, font2, new SolidBrush(Color.Black), (float)(num4 - my_Margin), (float)(num5 - my_Margin - 2));
							graphics.DrawString(my_Text, font2, new SolidBrush(Color.Black), (float)(num4 - my_Margin), (float)(num5 - (my_Margin - 2)));
						}
					}
					graphics.DrawString(my_Text, font2, brush, (float)(num4 - my_Margin), (float)(num5 - my_Margin));
					try
					{
						if (!Module_Donate.PRG_Donate)
						{
							string text = "My Watermark (unregistered version)";
							int num6 = 2;
							int num7;
							if (newSize.Width >= 3000)
							{
								num7 = 50;
							}
							else
							{
								if (newSize.Width >= 2500)
								{
									num7 = 40;
								}
								else
								{
									if (newSize.Width >= 1920)
									{
										num7 = 30;
									}
									else
									{
										if (newSize.Width >= 1680)
										{
											num7 = 26;
										}
										else
										{
											if (newSize.Width >= 1024)
											{
												num7 = 20;
											}
											else
											{
												num7 = 12;
												num6 = 1;
											}
										}
									}
								}
							}
							Font font3 = new Font(FontFamily.GenericSansSerif, (float)num7, FontStyle.Regular, GraphicsUnit.Pixel);
							sizeF = graphics.MeasureString(text, font3);
							if (flag2)
							{
								num5 = 2;
							}
							else
							{
								num5 = (int)Math.Round((double)unchecked((float)image.Size.Height - sizeF.Height));
							}
							graphics.DrawString(text, font3, new SolidBrush(Color.Black), (float)num6, (float)num5);
							graphics.DrawString(text, font3, new SolidBrush(Color.White), 0f, (float)(num5 - num6));
						}
					}
					catch (Exception expr_5ED)
					{
						ProjectData.SetProjectError(expr_5ED);
						ProjectData.ClearProjectError();
					}
					graphics.Dispose();
					result = bitmap;
				}
				catch (Exception expr_608)
				{
					ProjectData.SetProjectError(expr_608);
					Exception ex = expr_608;
					Module_MyFunctions.Update_LOG("ERROR [Text_On_Image] [" + ex.Message + "]", false, "", false);
					result = null;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static Image Resize_ImageBox(Image my_Image, int my_Width, int my_Height, bool my_Adapt = false)
		{
			checked
			{
				Image result;
				try
				{
					Bitmap bitmap = new Bitmap(my_Width, my_Height);
					Graphics graphics = Graphics.FromImage(bitmap);
					int x = 0;
					int y = 0;
					int num = my_Image.Width;
					int height = my_Image.Height;
					if (my_Adapt)
					{
						int num2 = (int)Math.Round(Math.Floor((double)(height * my_Width) / (double)my_Height));
						int num3 = (int)Math.Round(Math.Floor((double)(num2 - num) / 2.0));
						x = 0 - num3;
						num += num3 * 2;
					}
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					Graphics arg_8B_0 = graphics;
					Rectangle rectangle = new Rectangle(0, 0, my_Width, my_Height);
					Rectangle arg_8B_2 = rectangle;
					Rectangle srcRect = new Rectangle(x, y, num, height);
					arg_8B_0.DrawImage(my_Image, arg_8B_2, srcRect, GraphicsUnit.Pixel);
					graphics.Dispose();
					result = bitmap;
				}
				catch (Exception expr_9C)
				{
					ProjectData.SetProjectError(expr_9C);
					Exception ex = expr_9C;
					Module_MyFunctions.Update_LOG("ERROR [Resize_ImageBox] [" + ex.Message + "]", false, "", false);
					result = null;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
	}
}
