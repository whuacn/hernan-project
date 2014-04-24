using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using My_Watermark.My;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace My_Watermark
{
	[StandardModule]
	internal sealed class Module_MyFunctions
	{
		public static bool PRG_Loading = true;
		public static bool PRG_Loaded = false;
		public static string PRG_Copyright = "(C) 2011 / 2014 My Portable Software";
		public static bool PRG_Exit = false;
		public static bool PRG_AdminRights = true;
		public static string DAT_Path = "";
		public static string DAT_PathTemp = "";
		public static string DAT_Config = "";
		public static string DAT_MPS_Config = "";
		public static string DAT_LOG = "";
		public static string DAT_VLOG = "";
		public static bool DAT_Autostart = false;
		public static bool DAT_Command = false;
		public static string DAT_Autostart_EXE = "";
		public static bool DAT_Dialog = false;
		public static bool GLB_FRM_Main_Visible = true;
		public static int GLB_Timer_CK_count = 0;
		public static bool GLB_Timer_CK_onlyStats;
		public static bool DEBUG_DEV = false;
		public static bool DEBUG_MODE = false;
		private static bool PRV_ConfigReadError;
		private static Hashtable HT_Forms = new Hashtable();
		public static int ICON_SysTray_Recreate = 0;
		public static void Init_PRG_Data(string my_Autostart_EXE)
		{
			Module_Watermark.PRG_Title = Module_Watermark.PRG_Name + " v" + Module_Watermark.PRG_Version;
			Module_MyFunctions.DAT_Path = Path.GetDirectoryName(Application.ExecutablePath);
			if (!Module_MyFunctions.DAT_Path.EndsWith("\\"))
			{
				Module_MyFunctions.DAT_Path += "\\";
			}
			Module_MyFunctions.DAT_PathTemp = Module_MyFunctions.DAT_Path + "temp\\";
			Module_MyFunctions.DAT_Config = Module_MyFunctions.DAT_Path + Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".ini";
			Module_MyFunctions.DAT_LOG = Module_MyFunctions.DAT_Path + Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".log";
			Module_MyFunctions.DAT_Autostart_EXE = my_Autostart_EXE;
			if (Operators.CompareString(Module_MyFunctions.Config_Read("DEBUG", "", true), "Y", false) == 0)
			{
				Module_MyFunctions.DEBUG_MODE = true;
				Module_Watermark.PRG_Title += " ** DEBUG **";
			}
			if (File.Exists(Module_MyFunctions.DAT_Path + Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".debugDev"))
			{
				Module_MyFunctions.DEBUG_DEV = true;
			}
			try
			{
				if (File.Exists(Module_MyFunctions.DAT_Path + "updater.bat"))
				{
					File.Delete(Module_MyFunctions.DAT_Path + "updater.bat");
				}
			}
			catch (Exception expr_12D)
			{
				ProjectData.SetProjectError(expr_12D);
				Exception ex = expr_12D;
				if (Module_MyFunctions.DEBUG_MODE)
				{
					Module_MyFunctions.my_MsgBox("updater.delete: " + ex.Message, MessageBoxIcon.Exclamation, false);
				}
				ProjectData.ClearProjectError();
			}
			try
			{
				if (File.Exists(Module_MyFunctions.DAT_Path + Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".upd"))
				{
					File.Delete(Module_MyFunctions.DAT_Path + Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".upd");
				}
			}
			catch (Exception expr_19B)
			{
				ProjectData.SetProjectError(expr_19B);
				Exception ex2 = expr_19B;
				if (Module_MyFunctions.DEBUG_MODE)
				{
					Module_MyFunctions.my_MsgBox("upd.delete: " + ex2.Message, MessageBoxIcon.Exclamation, false);
				}
				ProjectData.ClearProjectError();
			}
		}
		public static bool Admin_Rights_Check()
		{
			bool result;
			try
			{
				if (Directory.Exists(Module_MyFunctions.DAT_Path + "tmpTestAdmRht"))
				{
					Directory.Delete(Module_MyFunctions.DAT_Path + "tmpTestAdmRht");
				}
				Directory.CreateDirectory(Module_MyFunctions.DAT_Path + "tmpTestAdmRht");
				if (Directory.Exists(Module_MyFunctions.DAT_Path + "tmpTestAdmRht"))
				{
					Directory.Delete(Module_MyFunctions.DAT_Path + "tmpTestAdmRht");
				}
				Module_MyFunctions.PRG_AdminRights = true;
				result = true;
			}
			catch (Exception expr_73)
			{
				ProjectData.SetProjectError(expr_73);
				MessageBox.Show(string.Concat(new string[]
				{
					"ERROR: to run ",
					Module_Watermark.PRG_Name,
					" in this directory, your user must have administrator rights.\r\n\r\nTo use ",
					Module_Watermark.PRG_Name,
					" with your account, you can copy it to another directory.\r\n\r\nOtherwise if you want to use it in this directory, you should start ",
					Module_Watermark.PRG_Name,
					" using 'run as administrator'."
				}), Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Module_MyFunctions.PRG_AdminRights = false;
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static void PRG_CheckUpdates(bool my_OnlyStats = false)
		{
			try
			{
				Module_MyFunctions.GLB_Timer_CK_onlyStats = my_OnlyStats;
				if (!my_OnlyStats)
				{
					bool arg_0E_0 = true;
					NotifyIcon notifyIcon = null;
					if (Module_Internet.WEB_CheckUpdate(arg_0E_0, notifyIcon))
					{
						Module_MyFunctions.GLB_Timer_CK_onlyStats = true;
					}
				}
				MyProject.Forms.Form_Main.Timer_Update.Interval = 2000;
				MyProject.Forms.Form_Main.Timer_Update.Enabled = true;
			}
			catch (Exception expr_4B)
			{
				ProjectData.SetProjectError(expr_4B);
				Exception ex = expr_4B;
				Module_MyFunctions.Update_LOG("ERROR [PRG_CheckUpdates] [" + ex.Message + "]", false, "", false);
				ProjectData.ClearProjectError();
			}
		}
		public static DialogResult my_MsgBox(string my_Msg, MessageBoxIcon my_MBIcon, bool my_Question = false)
		{
			DialogResult result;
			try
			{
				Application.DoEvents();
				if (my_Question)
				{
					result = MessageBox.Show(my_Msg, Module_Watermark.PRG_Name, MessageBoxButtons.YesNo, my_MBIcon, MessageBoxDefaultButton.Button2);
				}
				else
				{
					MessageBox.Show(my_Msg, Module_Watermark.PRG_Name, MessageBoxButtons.OK, my_MBIcon, MessageBoxDefaultButton.Button2);
					result = DialogResult.OK;
				}
			}
			catch (Exception expr_34)
			{
				ProjectData.SetProjectError(expr_34);
				result = DialogResult.OK;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static void my_Pause(int my_Seconds)
		{
			while (DateTime.Compare(DateTime.Now.AddSeconds((double)my_Seconds), DateTime.Now) >= 0)
			{
				if (Module_MyFunctions.PRG_Exit)
				{
					break;
				}
				Thread.Sleep(50);
				Application.DoEvents();
			}
		}
		public static string my_Get_Path(string my_Path)
		{
			string text = my_Path;
			checked
			{
				string result;
				try
				{
					if (text.StartsWith("\""))
					{
						text = Strings.Mid(text, 2);
					}
					if (text.EndsWith("\""))
					{
						text = Strings.Left(text, text.Length - 1);
					}
					if (text.Contains("\""))
					{
						text = Strings.Left(text, Strings.InStr(text, "\"", CompareMethod.Binary) - 1);
					}
					if (text.Contains(" /"))
					{
						text = Strings.Left(text, Strings.InStr(text, " /", CompareMethod.Binary) - 1);
					}
					else
					{
						if (text.Contains(" -"))
						{
							text = Strings.Left(text, Strings.InStr(text, " -", CompareMethod.Binary) - 1);
						}
					}
					result = text;
				}
				catch (Exception expr_9F)
				{
					ProjectData.SetProjectError(expr_9F);
					Exception ex = expr_9F;
					Module_MyFunctions.Update_LOG(string.Concat(new string[]
					{
						"ERROR [my_Get_Path(",
						my_Path,
						")] [",
						ex.Message,
						"]"
					}), true, "", false);
					result = my_Path;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static string get_Temp_Dir()
		{
			string result;
			try
			{
				Random random = new Random();
				string text = string.Concat(new string[]
				{
					Conversions.ToString(DateAndTime.Now.Year),
					Conversions.ToString(DateAndTime.Now.Month),
					Conversions.ToString(DateAndTime.Now.Day),
					Conversions.ToString(DateAndTime.Now.Hour),
					Conversions.ToString(DateAndTime.Now.Minute),
					Conversions.ToString(DateAndTime.Now.Second),
					Conversions.ToString(DateAndTime.Now.Millisecond),
					Conversions.ToString(random.Next(9999999)),
					"\\"
				});
				try
				{
					if (Directory.Exists(Module_MyFunctions.DAT_PathTemp))
					{
						Directory.Delete(Module_MyFunctions.DAT_PathTemp, true);
					}
				}
				catch (Exception expr_EE)
				{
					ProjectData.SetProjectError(expr_EE);
					Exception ex = expr_EE;
					Module_MyFunctions.Update_LOG("ERROR [get_Temp_Dir.Delete] [" + ex.Message + "]", false, "", false);
					ProjectData.ClearProjectError();
				}
				try
				{
					if (!Directory.Exists(Module_MyFunctions.DAT_PathTemp))
					{
						Directory.CreateDirectory(Module_MyFunctions.DAT_PathTemp);
					}
				}
				catch (Exception expr_136)
				{
					ProjectData.SetProjectError(expr_136);
					Exception ex2 = expr_136;
					throw new Exception("Cannot create temp directory [" + Module_MyFunctions.DAT_PathTemp + "]: " + ex2.Message);
				}
				try
				{
					Directory.CreateDirectory(Module_MyFunctions.DAT_PathTemp + text);
				}
				catch (Exception expr_172)
				{
					ProjectData.SetProjectError(expr_172);
					Exception ex3 = expr_172;
					throw new Exception(string.Concat(new string[]
					{
						"Cannot create temp directory [",
						Module_MyFunctions.DAT_PathTemp,
						text,
						"]: ",
						ex3.Message
					}));
				}
				result = Module_MyFunctions.DAT_PathTemp + text;
			}
			catch (Exception expr_1C8)
			{
				ProjectData.SetProjectError(expr_1C8);
				Exception ex4 = expr_1C8;
				Module_MyFunctions.Update_LOG("ERROR [get_Temp_Dir()] [" + ex4.Message + "]", true, "", false);
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static string Get_Unique_Filename(string my_Path, string my_Extension)
		{
			string text;
			while (true)
			{
				text = string.Concat(new string[]
				{
					my_Path,
					Module_MyFunctions.my_Get_Date(DateTime.MinValue).Replace("-", ""),
					"_",
					Module_MyFunctions.my_Get_Time(DateTime.MinValue).Replace(":", ""),
					".",
					my_Extension
				});
				if (!File.Exists(text))
				{
					break;
				}
				Thread.Sleep(500);
			}
			return text;
		}
		public static string my_Get_Time([DateTimeConstant(0L)] DateTime my_Date = default(DateTime))
		{
			if (DateTime.Compare(my_Date, DateTime.MinValue) == 0)
			{
				my_Date = DateAndTime.Now;
			}
			return string.Concat(new string[]
			{
				Strings.Right("0" + Conversions.ToString(my_Date.Hour), 2),
				":",
				Strings.Right("0" + Conversions.ToString(my_Date.Minute), 2),
				":",
				Strings.Right("0" + Conversions.ToString(my_Date.Second), 2)
			});
		}
		public static string my_Get_Date([DateTimeConstant(0L)] DateTime my_NotYearDate = default(DateTime))
		{
			if (DateTime.Compare(my_NotYearDate, DateTime.MinValue) == 0)
			{
				return string.Concat(new string[]
				{
					Conversions.ToString(DateAndTime.Now.Year),
					"-",
					Strings.Right("0" + Conversions.ToString(DateAndTime.Now.Month), 2),
					"-",
					Strings.Right("0" + Conversions.ToString(DateAndTime.Now.Day), 2)
				});
			}
			return Strings.Right("0" + Conversions.ToString(my_NotYearDate.Month), 2) + "/" + Strings.Right("0" + Conversions.ToString(my_NotYearDate.Day), 2);
		}
		public static void Update_LOG(string my_Msg, bool my_ShowError = false, string my_LOGFile = "", bool my_AlsoDate = false)
		{
			try
			{
				string str = Module_MyFunctions.my_Get_Date(DateTime.MinValue) + " " + Module_MyFunctions.my_Get_Time(DateTime.MinValue) + " ";
				if (Operators.CompareString(my_LOGFile, "", false) == 0)
				{
					my_LOGFile = Module_MyFunctions.DAT_LOG;
				}
				else
				{
					if (!my_AlsoDate)
					{
						str = "";
					}
				}
				if (my_ShowError)
				{
					MessageBox.Show(my_Msg, Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				StreamWriter streamWriter = new StreamWriter(my_LOGFile, true);
				streamWriter.WriteLine(str + my_Msg);
				streamWriter.Close();
			}
			catch (Exception expr_74)
			{
				ProjectData.SetProjectError(expr_74);
				ProjectData.ClearProjectError();
			}
		}
		public static void Update_VLOG(TextBox my_TextBox, string my_Msg = "", bool my_InsertDate = true)
		{
			if (Operators.CompareString(my_Msg, "", false) != 0 & my_InsertDate)
			{
				TextBox textBox = my_TextBox;
				textBox.Text = string.Concat(new string[]
				{
					textBox.Text,
					Module_MyFunctions.my_Get_Date(DateTime.MinValue),
					" ",
					Module_MyFunctions.my_Get_Time(DateTime.MinValue),
					" "
				});
			}
			else
			{
				if (Operators.CompareString(my_Msg, "-", false) == 0)
				{
					my_Msg = Strings.StrDup(80, "-");
				}
			}
			checked
			{
				while (my_TextBox.Text.Length + my_Msg.Length + 2 > my_TextBox.MaxLength)
				{
					my_TextBox.Text = Strings.Mid(my_TextBox.Text, Strings.InStr(my_TextBox.Text, "\r", CompareMethod.Binary) + 2);
					Application.DoEvents();
				}
				TextBox textBox = my_TextBox;
				textBox.Text = textBox.Text + my_Msg + "\r\n";
				my_TextBox.SelectionStart = my_TextBox.Text.Length;
				my_TextBox.ScrollToCaret();
				Module_MyFunctions.Update_LOG(my_Msg, false, Module_MyFunctions.DAT_VLOG, true);
			}
		}
		public static void AutoStartup_Verify(ToolStripMenuItem my_TSM)
		{
			string str = "";
			string text = Module_MyFunctions.Registry_ReadKey("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", Module_Watermark.PRG_Name);
			if (text == null)
			{
				text = Module_MyFunctions.Registry_ReadKey("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\\AutorunsDelayed", Module_Watermark.PRG_Name);
				str = "\\AutorunsDelayed";
			}
			bool flag = true;
			if (text == null)
			{
				my_TSM.Checked = false;
			}
			else
			{
				if (Operators.CompareString(text, Application.ExecutablePath, false) != 0 & !Module_MyFunctions.DEBUG_DEV)
				{
					flag = true;
				}
				my_TSM.Checked = true;
			}
			if (flag)
			{
				Module_MyFunctions.Registry_CreateKey("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run" + str, Module_Watermark.PRG_Name, Module_MyFunctions.DAT_Autostart_EXE, false);
			}
		}
		public static void AutoStartup_AddRemove(ToolStripMenuItem my_TSM)
		{
			string text = Module_MyFunctions.Registry_ReadKey("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", Module_Watermark.PRG_Name);
			string str = "";
			if (text == null)
			{
				text = Module_MyFunctions.Registry_ReadKey("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\\AutorunsDelayed", Module_Watermark.PRG_Name);
				str = "\\AutorunsDelayed";
			}
			if (text == null)
			{
				if (Module_MyFunctions.Registry_CreateKey("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", Module_Watermark.PRG_Name, Module_MyFunctions.DAT_Autostart_EXE, false))
				{
					my_TSM.Checked = true;
				}
			}
			else
			{
				if (Module_MyFunctions.Registry_DeleteKey(MyProject.Computer.Registry.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Run" + str, Module_Watermark.PRG_Name, false))
				{
					my_TSM.Checked = false;
				}
			}
		}
		public static void Hide_Form(Form my_Form)
		{
			if (!Module_MyFunctions.HT_Forms.ContainsKey(my_Form.Name))
			{
				Module_MyFunctions.HT_Forms.Add(my_Form.Name, my_Form.Location);
			}
			else
			{
				Module_MyFunctions.HT_Forms[my_Form.Name] = my_Form.Location;
			}
			Form arg_71_0 = my_Form;
			Point location = checked(new Point(0 - my_Form.Width - 100, 0 - my_Form.Height - 100));
			arg_71_0.Location = location;
			my_Form.ShowInTaskbar = false;
			my_Form.Visible = false;
			if (Operators.CompareString(my_Form.Name.ToUpper(), "FORM_MAIN", false) == 0)
			{
				Module_MyFunctions.GLB_FRM_Main_Visible = false;
			}
		}
		public static void show_myForm(Form my_Form, string my_FormPos = "C", bool my_ShowInTaskBar = false)
		{
			Point location = default(Point);
			checked
			{
				if (!Module_MyFunctions.HT_Forms.ContainsKey(my_Form.Name))
				{
					if (Operators.CompareString(my_FormPos, "BR", false) == 0)
					{
						location = new Point(Screen.PrimaryScreen.WorkingArea.Width - my_Form.Size.Width - 1, Screen.PrimaryScreen.WorkingArea.Height - my_Form.Size.Height - 1);
					}
					else
					{
						location = new Point(Convert.ToInt32((double)(Screen.PrimaryScreen.WorkingArea.Width - my_Form.Size.Width) / 2.0), Convert.ToInt32((double)(Screen.PrimaryScreen.WorkingArea.Height - my_Form.Size.Height) / 2.0));
					}
				}
				else
				{
					object expr_105 = Module_MyFunctions.HT_Forms[my_Form.Name];
					Point point = new Point();
					location = ((expr_105 != null) ? ((Point)expr_105) : point);
				}
				my_Form.Location = location;
				my_Form.ShowInTaskbar = my_ShowInTaskBar;
				my_Form.WindowState = FormWindowState.Normal;
				my_Form.Visible = true;
				if (Operators.CompareString(my_Form.Name.ToUpper(), "FORM_MAIN", false) == 0)
				{
					Module_MyFunctions.GLB_FRM_Main_Visible = true;
				}
			}
		}
		public static string Calculate_MD5(string my_StringData)
		{
			string result;
			try
			{
				MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
				if (Operators.CompareString(my_StringData, "", false) != 0)
				{
					result = Convert.ToBase64String(mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(my_StringData)));
				}
				else
				{
					result = "";
				}
			}
			catch (Exception expr_36)
			{
				ProjectData.SetProjectError(expr_36);
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static string Calculate_File_MD5(string my_File)
		{
			string result;
			try
			{
				if (File.Exists(my_File))
				{
					MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
					FileStream fileStream = new FileStream(my_File, FileMode.Open, FileAccess.Read);
					string text = Convert.ToBase64String(mD5CryptoServiceProvider.ComputeHash(fileStream));
					fileStream.Close();
					result = text;
				}
				else
				{
					result = "-";
				}
			}
			catch (Exception expr_36)
			{
				ProjectData.SetProjectError(expr_36);
				Exception ex = expr_36;
				Module_MyFunctions.Update_LOG("ERROR [Calculate_File_MD5] [" + ex.Message + "]", true, "", false);
				result = "-";
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static string Convert_To_Hex(string my_StringData)
		{
			checked
			{
				string result;
				try
				{
					string text = "";
					int arg_11_0 = 0;
					int num = my_StringData.Length - 1;
					for (int i = arg_11_0; i <= num; i++)
					{
						text += Strings.Asc(my_StringData.Substring(i, 1)).ToString("x").ToUpper();
					}
					result = text;
				}
				catch (Exception expr_48)
				{
					ProjectData.SetProjectError(expr_48);
					Exception ex = expr_48;
					Module_MyFunctions.Update_LOG("ERROR [Convert_To_Hex] [" + ex.Message + "]", true, "", false);
					result = "";
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static string Parse_Page_Data(string my_Data, string my_Tag, bool my_StripData = false, bool my_UpdateLog = true, bool my_RightData = false, bool my_CaseSensitive = true)
		{
			checked
			{
				string result;
				try
				{
					my_Tag = my_Tag.Replace("&numbersign;", "#");
					string[] array = Strings.Split(my_Tag, "|", -1, CompareMethod.Binary);
					string text = my_Data;
					int arg_2B_0 = 0;
					int num = array.Length - 1;
					for (int i = arg_2B_0; i <= num; i++)
					{
						if (!(text.Contains(array[i]) | (!my_CaseSensitive & text.ToUpper().Contains(array[i].ToUpper()))))
						{
							if (my_UpdateLog)
							{
								Module_MyFunctions.Update_LOG(string.Concat(new string[]
								{
									"ERROR [Parse_Page_Data (",
									my_Tag,
									")] [(",
									array[i],
									") tag not found]"
								}), true, "", false);
							}
							result = "";
							return result;
						}
						int num2;
						int num3;
						if (my_CaseSensitive)
						{
							num2 = Strings.InStr(text, array[i], CompareMethod.Binary);
							num3 = Strings.InStr(my_Data, array[i], CompareMethod.Binary);
						}
						else
						{
							num2 = Strings.InStr(text.ToUpper(), array[i].ToUpper(), CompareMethod.Binary);
							num3 = Strings.InStr(my_Data.ToUpper(), array[i].ToUpper(), CompareMethod.Binary);
						}
						if (i == array.Length - 1)
						{
							if (num2 > 1)
							{
								if (array.Length == 1 & my_RightData)
								{
									text = Strings.Mid(text, num2 + array[i].Length);
								}
								else
								{
									text = Strings.Left(text, num2 - 1);
								}
							}
							else
							{
								text = "";
							}
							if (my_StripData & num3 > 0)
							{
								my_Data = Strings.Mid(my_Data, num3 + array[i].Length);
							}
						}
						else
						{
							text = Strings.Mid(text, num2 + array[i].Length);
							if (my_StripData)
							{
								my_Data = text;
							}
						}
					}
					result = text;
				}
				catch (Exception expr_180)
				{
					ProjectData.SetProjectError(expr_180);
					Exception ex = expr_180;
					Module_MyFunctions.Update_LOG(string.Concat(new string[]
					{
						"ERROR [Parse_Page_Data (",
						my_Tag,
						")] [",
						ex.Message,
						"]"
					}), true, "", false);
					result = "";
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static string Config_Read(string my_Key, string my_ConfigFile = "", bool my_ShowError = true)
		{
			int num = 0;
			checked
			{
				string result;
				while (true)
				{
					num++;
					result = Module_MyFunctions.My_Config_Read(my_Key, my_ConfigFile, num == 5);
					if (!Module_MyFunctions.PRV_ConfigReadError)
					{
						break;
					}
					if (num > 4)
					{
						goto Block_2;
					}
					Thread.Sleep(250);
				}
				return result;
				Block_2:
				if (my_ShowError)
				{
					Module_MyFunctions.my_MsgBox(string.Concat(new string[]
					{
						"Fatal error: cannot read config key [",
						my_Key,
						"] in [",
						Path.GetFileName(Module_MyFunctions.DAT_Config),
						"]\r\n\r\nVerify the log."
					}), MessageBoxIcon.Hand, false);
				}
				return "";
			}
		}
		public static bool Config_Save(string my_Key, string my_Value, string my_ConfigFile = "", bool my_RemoveKey = false, bool my_ShowError = true)
		{
			int num = 0;
			checked
			{
				while (true)
				{
					num++;
					if (Module_MyFunctions.My_Config_Save(my_Key, my_Value, my_ConfigFile, my_RemoveKey, num == 5))
					{
						break;
					}
					if (num > 4)
					{
						goto Block_2;
					}
					Thread.Sleep(500);
				}
				return true;
				Block_2:
				if (my_ShowError)
				{
					Module_MyFunctions.my_MsgBox(string.Concat(new string[]
					{
						"Fatal error: cannot save config key [",
						my_Key,
						"] to [",
						Path.GetFileName(Module_MyFunctions.DAT_Config),
						"]\r\n\r\nProbably your user doesn't have administrator rights."
					}), MessageBoxIcon.Hand, false);
				}
				return false;
			}
		}
		private static bool My_Config_Save(string my_Key, string my_Value, string my_ConfigFile = "", bool my_RemoveKey = false, bool my_UpdateLOG = true)
		{
			checked
			{
				bool result;
				try
				{
					if (Operators.CompareString(my_ConfigFile, "", false) == 0)
					{
						my_ConfigFile = Module_MyFunctions.DAT_Config;
					}
					string text = "";
					bool flag = false;
					if (my_RemoveKey)
					{
						flag = true;
					}
					if (File.Exists(my_ConfigFile))
					{
						StreamReader streamReader = new StreamReader(my_ConfigFile, false);
						string[] array = Strings.Split(streamReader.ReadToEnd(), "\r\n", -1, CompareMethod.Binary);
						streamReader.Close();
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text2 = array2[i];
							if (Operators.CompareString(Strings.Trim(text2), "", false) != 0)
							{
								text2 = text2.Replace("\r", "").Replace("\n", "");
								if (Operators.CompareString(Strings.Trim(text2), "", false) != 0)
								{
									if (Operators.CompareString(Strings.Left(text2, my_Key.Length + 1).ToUpper(), Strings.UCase(my_Key + "="), false) == 0)
									{
										if (Operators.CompareString(text2, my_Key + "=" + my_Value, false) == 0 & !my_RemoveKey)
										{
											result = true;
											return result;
										}
										if (!flag)
										{
											flag = true;
											text = string.Concat(new string[]
											{
												text,
												my_Key,
												"=",
												my_Value,
												"\r\n"
											});
										}
									}
									else
									{
										text = text + text2 + "\r\n";
									}
								}
							}
						}
					}
					if (!flag)
					{
						text = string.Concat(new string[]
						{
							text,
							my_Key,
							"=",
							my_Value,
							"\r\n"
						});
					}
					StreamWriter streamWriter = new StreamWriter(my_ConfigFile, false);
					streamWriter.Write(text);
					streamWriter.Close();
					result = true;
				}
				catch (Exception expr_1AE)
				{
					ProjectData.SetProjectError(expr_1AE);
					Exception ex = expr_1AE;
					if (my_UpdateLOG)
					{
						Module_MyFunctions.Update_LOG(string.Concat(new string[]
						{
							"ERROR [Config_Save] [",
							my_Key,
							"] [",
							my_Value,
							"] ",
							ex.Message
						}), false, "", false);
					}
					result = false;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static string My_Config_Read(string my_Key, string my_ConfigFile = "", bool my_UpdateLOG = true)
		{
			checked
			{
				string result;
				try
				{
					Module_MyFunctions.PRV_ConfigReadError = false;
					if (Operators.CompareString(my_ConfigFile, "", false) == 0)
					{
						my_ConfigFile = Module_MyFunctions.DAT_Config;
					}
					if (!File.Exists(my_ConfigFile))
					{
						result = "";
					}
					else
					{
						StreamReader streamReader = new StreamReader(my_ConfigFile, false);
						string[] array = Strings.Split(streamReader.ReadToEnd(), "\r\n", -1, CompareMethod.Binary);
						streamReader.Close();
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text = array2[i];
							if (Operators.CompareString(Strings.Trim(text), "", false) != 0)
							{
								text = text.Replace("\r", "").Replace("\n", "");
								if (Operators.CompareString(Strings.Trim(text), "", false) != 0 && Operators.CompareString(Strings.Left(text, my_Key.Length + 1).ToUpper(), Strings.UCase(my_Key + "="), false) == 0)
								{
									result = Strings.Mid(text, my_Key.Length + 2);
									return result;
								}
							}
						}
						result = "";
					}
				}
				catch (Exception expr_FF)
				{
					ProjectData.SetProjectError(expr_FF);
					Exception ex = expr_FF;
					Module_MyFunctions.PRV_ConfigReadError = true;
					if (my_UpdateLOG)
					{
						Module_MyFunctions.Update_LOG("ERROR [Config_Read] [" + my_Key + "] " + ex.Message, false, "", false);
					}
					result = "[cfg.readerror]";
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static string Config_MPS_Read(string my_Key = "")
		{
			checked
			{
				string result;
				try
				{
					if (!File.Exists(Module_MyFunctions.DAT_MPS_Config))
					{
						result = "";
					}
					else
					{
						StreamReader streamReader = new StreamReader(Module_MyFunctions.DAT_MPS_Config);
						string text = streamReader.ReadToEnd().Replace("\r", "");
						streamReader.Close();
						if (Operators.CompareString(my_Key, "", false) == 0)
						{
							result = text;
						}
						else
						{
							string[] array = Strings.Split(text, "\n", -1, CompareMethod.Binary);
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string text2 = array2[i];
								if (text2.ToUpper().Contains(my_Key.ToUpper()))
								{
									result = text2;
									return result;
								}
							}
							result = "";
						}
					}
				}
				catch (Exception expr_A7)
				{
					ProjectData.SetProjectError(expr_A7);
					Exception ex = expr_A7;
					Module_MyFunctions.Update_LOG("ERROR [Config_MPS_Read] [" + my_Key + "] " + ex.Message, false, "", false);
					result = "";
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static bool Config_MPS_Save(string my_Data, string my_Key, bool my_DeleteKey = false, bool my_AddKeyUnique = false)
		{
			checked
			{
				bool result;
				try
				{
					string text = "";
					if (File.Exists(Module_MyFunctions.DAT_MPS_Config))
					{
						StreamReader streamReader = new StreamReader(Module_MyFunctions.DAT_MPS_Config);
						string[] array = streamReader.ReadToEnd().Replace("\r", "").Split(new char[]
						{
							'\n'
						});
						streamReader.Close();
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text2 = array2[i];
							if (Operators.CompareString(Strings.Trim(text2), "", false) != 0)
							{
								if (!text2.ToUpper().Contains(my_Key.ToUpper()))
								{
									text = text + text2 + "\r\n";
								}
								else
								{
									if (text2.ToUpper().Contains(my_Key.ToUpper()) & my_AddKeyUnique)
									{
										result = false;
										return result;
									}
								}
							}
						}
					}
					StreamWriter streamWriter = new StreamWriter(Module_MyFunctions.DAT_MPS_Config);
					streamWriter.Write(text);
					if (!my_DeleteKey)
					{
						streamWriter.WriteLine(my_Data);
					}
					streamWriter.Close();
					result = true;
				}
				catch (Exception expr_F0)
				{
					ProjectData.SetProjectError(expr_F0);
					Exception ex = expr_F0;
					Module_MyFunctions.Update_LOG(string.Concat(new string[]
					{
						"ERROR [Config_MPS_Save] [",
						my_Data,
						"] [",
						my_Key,
						"] [",
						Conversions.ToString(my_DeleteKey),
						"]",
						ex.Message
					}), false, "", false);
					result = false;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static string Registry_ReadKey(string my_Tree, string my_Key)
		{
			string result;
			try
			{
				result = Conversions.ToString(MyProject.Computer.Registry.GetValue(my_Tree, my_Key, null));
			}
			catch (Exception expr_1A)
			{
				ProjectData.SetProjectError(expr_1A);
				result = null;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static bool Registry_CreateKey(string my_Tree, string my_Key, string my_Value, bool my_Silence = false)
		{
			bool result;
			try
			{
				MyProject.Computer.Registry.SetValue(my_Tree, my_Key, my_Value);
				result = true;
			}
			catch (Exception expr_19)
			{
				ProjectData.SetProjectError(expr_19);
				Exception ex = expr_19;
				Module_MyFunctions.Update_LOG(string.Concat(new string[]
				{
					"ERROR [Registry_CreateKey (",
					my_Tree,
					", ",
					my_Key,
					", ",
					my_Value,
					")] [",
					ex.Message,
					"]"
				}), true, "", false);
				if (!my_Silence)
				{
					MessageBox.Show("Fatal error: cannot create registry key. Probably your user does not have administrator rights.\r\n\r\nError details:\r\n" + ex.Message, Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static bool Registry_DeleteKey(RegistryKey my_Registry, string my_Tree, string my_Key, bool my_Silence = false)
		{
			bool result;
			try
			{
				object obj = null;
				try
				{
					RegistryKey registryKey = my_Registry.OpenSubKey(my_Tree, false);
					obj = RuntimeHelpers.GetObjectValue(registryKey.GetValue(my_Key));
				}
				catch (Exception expr_1A)
				{
					ProjectData.SetProjectError(expr_1A);
					obj = null;
					ProjectData.ClearProjectError();
				}
				if (obj != null)
				{
					my_Registry.OpenSubKey(my_Tree, true).DeleteValue(my_Key);
				}
				result = true;
			}
			catch (Exception expr_44)
			{
				ProjectData.SetProjectError(expr_44);
				Exception ex = expr_44;
				Module_MyFunctions.Update_LOG(string.Concat(new string[]
				{
					"ERROR [Registry_DeleteKey (",
					my_Tree,
					", ",
					my_Key,
					")] [",
					ex.Message,
					"]"
				}), true, "", false);
				if (!my_Silence)
				{
					MessageBox.Show("Fatal error: cannot delete registry key. Probably your user does not have administrator rights.\r\n\r\nError details:\r\n" + ex.Message, Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}
}
