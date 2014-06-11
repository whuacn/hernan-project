using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using My_Watermark.My;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;
namespace My_Watermark
{
	[StandardModule]
	internal sealed class Module_Internet
	{
		public struct STR_Proxy
		{
			public string ProxyAddress;
			public string ProxyUser;
			public string ProxyPassword;
		}
		public struct STR_SMTP
		{
			public string SMTP_Address;
			public int SMTP_Port;
			public bool SMTP_SSL;
			public string SMTP_User;
			public string SMTP_Password;
		}
		public static Module_Internet.STR_Proxy WEB_Proxy_Config;
		public static Module_Internet.STR_SMTP WEB_SMTP_Config;
		public static int WEB_Timeout = 10;
		public static bool WEB_Error_404 = false;
		public static bool WEB_Error_Proxy = false;
		public static string WEB_Error_MSG = "";
		public static string NET_Error_Send_EMail = "";
		public static string WEB_Site_URL = "";
		public static string WEB_Report_Errors_URL = "";
		public static string WEB_Site_DATA = "";
		public static string WEB_Site_Updates = "";
		public static string WEB_Updater_DATA = "";
		public static string WEB_Updater_URL = "";
		public static string WEB_BundleAPP_URL = "";
		public static string WEB_EXEUPD_URL = "";
		public static string WEB_EXEUPD_DAT = "";
		public static string WEB_EXEUPD_MD5 = "";
		public static string WEB_EXEUPD_NEW = "";
		public static string WEB_Site_Stats = "";
		public static string WEB_MD5_URL = Module_Internet.WEB_Site_Updates;
		private static WebProxy WEB_Proxy;
		public static string WEB_Generate_Stats_URL()
		{
			string str = "";
			if (Module_MyFunctions.DAT_Command | Module_MyFunctions.DAT_Autostart)
			{
				str = "CL";
			}
			string text = Module_Watermark.PRG_Name.Replace(" ", "") + str + "_" + Module_Watermark.PRG_Version;
			if (Module_Donate.PRG_Donate_Bundle)
			{
				text += ".R";
			}
			else
			{
				if (Module_Donate.PRG_Donate)
				{
					text += ".D";
				}
			}
			return Module_Internet.WEB_Site_Stats + "?source=" + text;
		}
		public static void WEB_InitProxy(string my_ProxyURL, string my_ProxyUser, string my_ProxyPassword)
		{
			if (Operators.CompareString(my_ProxyURL, "", false) != 0 && !(my_ProxyURL.ToUpper().StartsWith("HTTP://") | my_ProxyURL.ToUpper().StartsWith("HTTPS://")))
			{
				my_ProxyURL = "http://" + my_ProxyURL;
			}
			Module_Internet.WEB_Proxy_Config.ProxyAddress = my_ProxyURL;
			Module_Internet.WEB_Proxy_Config.ProxyUser = my_ProxyUser;
			Module_Internet.WEB_Proxy_Config.ProxyPassword = my_ProxyPassword;
		}
		public static bool WEB_SetProxy(WebClient my_WebClient)
		{
			Module_Internet.WEB_Error_Proxy = false;
			bool result;
			try
			{
				try
				{
					Module_Internet.WEB_Proxy = new WebProxy();
					Module_Internet.WEB_Proxy.Address = new Uri(Module_Internet.WEB_Proxy_Config.ProxyAddress);
				}
				catch (Exception expr_2B)
				{
					ProjectData.SetProjectError(expr_2B);
					Exception ex = expr_2B;
					MessageBox.Show(string.Concat(new string[]
					{
						"Error connecting to proxy [",
						Module_Internet.WEB_Proxy_Config.ProxyAddress,
						"]: ",
						ex.Message,
						"\r\n\r\nCheck your firewall / proxy. If you are behind a proxy, configure it in the options window."
					}), Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					result = false;
					ProjectData.ClearProjectError();
					return result;
				}
				Module_Internet.WEB_Proxy.Credentials = new NetworkCredential(Module_Internet.WEB_Proxy_Config.ProxyUser, Module_Internet.WEB_Proxy_Config.ProxyPassword);
				my_WebClient.Proxy = Module_Internet.WEB_Proxy;
				Application.DoEvents();
				result = true;
			}
			catch (Exception expr_BF)
			{
				ProjectData.SetProjectError(expr_BF);
				Exception ex2 = expr_BF;
				Module_MyFunctions.Update_LOG("ERROR [WEB_SetProxy.WebClient] [" + ex2.Message + "]", false, "", false);
				MessageBox.Show("Error connecting to proxy [" + Module_Internet.WEB_Proxy_Config.ProxyAddress + "]: " + ex2.Message, Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		private static bool WEB_SetProxy(WebRequest my_WebClient)
		{
			Module_Internet.WEB_Error_Proxy = false;
			bool result;
			try
			{
				try
				{
					Module_Internet.WEB_Proxy = new WebProxy();
					Module_Internet.WEB_Proxy.Address = new Uri(Module_Internet.WEB_Proxy_Config.ProxyAddress);
				}
				catch (Exception expr_2B)
				{
					ProjectData.SetProjectError(expr_2B);
					Exception ex = expr_2B;
					MessageBox.Show(string.Concat(new string[]
					{
						"Error connecting to proxy [",
						Module_Internet.WEB_Proxy_Config.ProxyAddress,
						"]: ",
						ex.Message,
						"\r\n\r\nCheck your firewall / proxy. If you are behind a proxy, configure it in the options window."
					}), Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					result = false;
					ProjectData.ClearProjectError();
					return result;
				}
				Module_Internet.WEB_Proxy.Credentials = new NetworkCredential(Module_Internet.WEB_Proxy_Config.ProxyUser, Module_Internet.WEB_Proxy_Config.ProxyPassword);
				my_WebClient.Proxy = Module_Internet.WEB_Proxy;
				Application.DoEvents();
				result = true;
			}
			catch (Exception expr_BF)
			{
				ProjectData.SetProjectError(expr_BF);
				Exception ex2 = expr_BF;
				Module_MyFunctions.Update_LOG("ERROR [WEB_SetProxy.WebRequest] [" + ex2.Message + "]", false, "", false);
				MessageBox.Show("Error connecting to proxy [" + Module_Internet.WEB_Proxy_Config.ProxyAddress + "]: " + ex2.Message, Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static string WEB_CheckURL(string my_URL, string my_ResponseLength = null)
		{
			try
			{
				WebRequest webRequest = null;
				WebResponse webResponse = null;
				webRequest = WebRequest.Create(my_URL);
				webRequest.Timeout = 5000;
				if (Operators.CompareString(Module_Internet.WEB_Proxy_Config.ProxyAddress, "", false) != 0 && !Module_Internet.WEB_SetProxy(webRequest))
				{
					string result = "P";
					return result;
				}
				webResponse = webRequest.GetResponse();
				if (my_ResponseLength != null)
				{
					try
					{
						long contentLength = webResponse.ContentLength;
						my_ResponseLength = Conversions.ToString(contentLength);
					}
					catch (Exception expr_5E)
					{
						ProjectData.SetProjectError(expr_5E);
						my_ResponseLength = Conversions.ToString(0);
						string result = "C";
						ProjectData.ClearProjectError();
						return result;
					}
				}
				try
				{
					webResponse.Close();
					webRequest = null;
				}
				catch (Exception expr_85)
				{
					ProjectData.SetProjectError(expr_85);
					ProjectData.ClearProjectError();
				}
			}
			catch (Exception expr_94)
			{
				ProjectData.SetProjectError(expr_94);
				Exception ex = expr_94;
				string result;
				if (ex.Message.Contains("(407)"))
				{
					result = "P";
					ProjectData.ClearProjectError();
					return result;
				}
				result = "E" + ex.Message;
				ProjectData.ClearProjectError();
				return result;
			}
			return "";
		}
		public static byte[] WEB_Download(string my_URL, string my_SavePATH = "", bool my_UseFilenameUrl = false, ProgressBar my_ProgressBar = null, bool my_ShowError = true, bool my_StandardTimeout = false, bool my_Error404 = false, bool my_ErrorTimeout = false, bool my_NoCache = false)
		{
			BinaryWriter binaryWriter = null;
			checked
			{
				byte[] result;
				try
				{
					byte[] array = null;
					byte[] array2 = new byte[]
					{
						0
					};
					Application.DoEvents();
					Module_Internet.WEB_Error_404 = false;
					if (Operators.CompareString(my_SavePATH, "", false) != 0)
					{
						if (my_UseFilenameUrl)
						{
							if (!my_SavePATH.EndsWith("\\"))
							{
								my_SavePATH += "\\";
							}
							my_SavePATH += my_URL.Substring(my_URL.LastIndexOf("/") + 1);
						}
						if (my_ProgressBar != null)
						{
							my_ProgressBar.Value = 0;
							my_ProgressBar.Visible = true;
							my_ProgressBar.BringToFront();
						}
						WebRequest webRequest = null;
						WebResponse webResponse = null;
						try
						{
							webResponse = null;
							webRequest = null;
							webRequest = WebRequest.Create(Module_Internet.WEB_GetSID(my_URL, my_NoCache));
							if (my_StandardTimeout)
							{
								webRequest.Timeout = 5000;
							}
							else
							{
								webRequest.Timeout = Module_Internet.WEB_Timeout * 1000;
							}
							if (Operators.CompareString(Module_Internet.WEB_Proxy_Config.ProxyAddress, "", false) != 0 && !Module_Internet.WEB_SetProxy(webRequest))
							{
								result = null;
								return result;
							}
							webResponse = webRequest.GetResponse();
						}
						catch (Exception expr_F6)
						{
							ProjectData.SetProjectError(expr_F6);
							Exception ex = expr_F6;
							throw new Exception("[TIMEOUT] " + ex.Message);
						}
						long contentLength = webResponse.ContentLength;
						long num = 0L;
						Application.DoEvents();
						Stream responseStream = webResponse.GetResponseStream();
						BinaryReader binaryReader = new BinaryReader(responseStream);
						Application.DoEvents();
						binaryWriter = new BinaryWriter(File.Create(my_SavePATH));
						array = array2;
						while (array.Length != 0)
						{
							array = binaryReader.ReadBytes(5000);
							if (array.Length == 0)
							{
								break;
							}
							if (my_ProgressBar != null)
							{
								try
								{
									num += unchecked((long)array.Length);
									double num2 = Math.Floor((double)(num * 100L) / (double)contentLength);
									if (num2 > 100.0)
									{
										num2 = 100.0;
									}
									my_ProgressBar.Value = (int)Math.Round(num2);
								}
								catch (Exception expr_1B4)
								{
									ProjectData.SetProjectError(expr_1B4);
									my_ProgressBar.Value = 100;
									ProjectData.ClearProjectError();
								}
							}
							binaryWriter.Write(array);
							Application.DoEvents();
						}
						if (my_ProgressBar != null)
						{
							my_ProgressBar.Value = 100;
							Application.DoEvents();
						}
						responseStream.Close();
						binaryWriter.Close();
						if (my_ProgressBar != null)
						{
							my_ProgressBar.Visible = false;
						}
						result = array2;
					}
					else
					{
						WebClient webClient = new WebClient();
						if (Operators.CompareString(Module_Internet.WEB_Proxy_Config.ProxyAddress, "", false) != 0 && !Module_Internet.WEB_SetProxy(webClient))
						{
							result = null;
						}
						else
						{
							array = webClient.DownloadData(Module_Internet.WEB_GetSID(my_URL, my_NoCache));
							result = array;
						}
					}
				}
				catch (Exception expr_259)
				{
					ProjectData.SetProjectError(expr_259);
					Exception ex2 = expr_259;
					try
					{
						if (binaryWriter != null)
						{
							binaryWriter.Close();
						}
					}
					catch (Exception expr_26C)
					{
						ProjectData.SetProjectError(expr_26C);
						ProjectData.ClearProjectError();
					}
					string text = Strings.Replace(ex2.Message, Module_Watermark.PRG_APIKEY, "<APIKEY>", 1, -1, CompareMethod.Binary);
					my_URL = Strings.Replace(my_URL, Module_Watermark.PRG_APIKEY, "<APIKEY>", 1, -1, CompareMethod.Binary);
					Module_Internet.verify_WEB_Error(ex2.Message, my_Error404, my_ErrorTimeout);
					if (!my_ErrorTimeout)
					{
						Module_MyFunctions.Update_LOG(string.Concat(new string[]
						{
							"ERROR [WEB_Download] [",
							my_URL,
							"] [",
							text,
							"]"
						}), false, "", false);
					}
					if (my_ShowError)
					{
						if (my_Error404)
						{
							MessageBox.Show("Error [" + my_URL + "] not found (404)", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						else
						{
							MessageBox.Show(string.Concat(new string[]
							{
								"Error connecting to [",
								my_URL,
								"] - error details:\r\n\r\n",
								text,
								"\r\n\r\nCheck your firewall / proxy."
							}), Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
					if (my_ProgressBar != null)
					{
						my_ProgressBar.Visible = false;
					}
					result = null;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static string WEB_Get(string my_URL, bool my_ShowError = true, bool my_ShowError404 = true, bool my_Error404 = false, bool my_ErrorTimeout = false, bool my_NoCache = false)
		{
			checked
			{
				string result;
				try
				{
					Module_Internet.WEB_Error_404 = false;
					WebClient webClient = new WebClient();
					Application.DoEvents();
					if (Operators.CompareString(Module_Internet.WEB_Proxy_Config.ProxyAddress, "", false) != 0 && !Module_Internet.WEB_SetProxy(webClient))
					{
						result = null;
					}
					else
					{
						string @string = Encoding.UTF8.GetString(webClient.DownloadData(Module_Internet.WEB_GetSID(my_URL, my_NoCache)));
						result = @string;
					}
				}
				catch (Exception expr_5B)
				{
					ProjectData.SetProjectError(expr_5B);
					Exception ex = expr_5B;
					string text = Strings.Replace(ex.Message, Module_Watermark.PRG_APIKEY, "<APIKEY>", 1, -1, CompareMethod.Binary);
					my_URL = Strings.Replace(my_URL, Module_Watermark.PRG_APIKEY, "<APIKEY>", 1, -1, CompareMethod.Binary);
					if (text.Contains("donateck.aspx"))
					{
						text = Strings.Left(text, Strings.InStr(text, "donateck.aspx", CompareMethod.Binary) + 12);
					}
					if (my_URL.Contains("donateck.aspx"))
					{
						my_URL = Strings.Left(my_URL, Strings.InStr(my_URL, "donateck.aspx", CompareMethod.Binary) + 12);
					}
					Module_MyFunctions.Update_LOG(string.Concat(new string[]
					{
						"ERROR [WEB_Get] [",
						my_URL,
						"] [",
						text,
						"]"
					}), false, "", false);
					Module_Internet.verify_WEB_Error(text, my_Error404, my_ErrorTimeout);
					if (my_ShowError)
					{
						if (my_Error404)
						{
							MessageBox.Show("Error [" + my_URL + "] not found (404)", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
						else
						{
							MessageBox.Show(string.Concat(new string[]
							{
								"Error connecting to [",
								my_URL,
								"] - error details:\r\n\r\n",
								text,
								"\r\n\r\nCheck your firewall / proxy."
							}), Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						}
					}
					result = null;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}
		public static DataSet WEB_GetDS(string my_URL, bool my_ShowError = true)
		{
			DataSet result;
			try
			{
				Module_Internet.WEB_Error_404 = false;
				DataSet dataSet = new DataSet();
				bool arg_1A_2 = true;
				bool flag = false;
				bool flag2 = false;
				string text = Module_Internet.WEB_Get(my_URL, my_ShowError, arg_1A_2, flag, flag2, false);
				if (text == null)
				{
					result = null;
				}
				else
				{
					if (Operators.CompareString(text, "", false) != 0)
					{
						dataSet.ReadXml(XmlReader.Create(new StringReader(text)));
						result = dataSet;
					}
					else
					{
						result = null;
					}
				}
			}
			catch (Exception expr_53)
			{
				ProjectData.SetProjectError(expr_53);
				Exception ex = expr_53;
				string str = Strings.Replace(ex.Message, Module_Watermark.PRG_APIKEY, "<APIKEY>", 1, -1, CompareMethod.Binary);
				Module_MyFunctions.Update_LOG("ERROR [WEB_GetDS] [" + str + "]", false, "", false);
				string arg_A1_0 = ex.Message;
				bool flag2 = false;
				bool flag = false;
				Module_Internet.verify_WEB_Error(arg_A1_0, flag2, flag);
				result = null;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		private static string WEB_GetSID(string my_URL, bool my_NoCache = false)
		{
			if (my_NoCache)
			{
				Random random = new Random();
				if (my_URL.Contains("?"))
				{
					my_URL += "&";
				}
				else
				{
					my_URL += "?";
				}
				my_URL = my_URL + "SID=" + Conversions.ToString(random.Next(999999));
			}
			return my_URL;
		}
		public static bool WEB_Init_DATA(bool my_OnlyUpdater = false)
		{
			bool result;
			try
			{
				string text = "";
				string text2;
				string text3;
				if (Module_MyFunctions.DEBUG_MODE)
				{
					text2 = "http://localhost:1958/data/" + Module_Watermark.PRG_KEY_DataFile;
					text3 = "http://localhost:1958/data/updater.txt";
				}
				else
				{
					text2 = Module_Internet.WEB_Site_Updates;
					text3 = Module_Internet.WEB_Updater_URL;
				}
				if (my_OnlyUpdater)
				{
					string arg_55_0 = text3;
					bool arg_55_1 = false;
					bool arg_55_2 = false;
					bool flag = false;
					bool flag2 = false;
					text = Module_Internet.WEB_Get(arg_55_0, arg_55_1, arg_55_2, flag, flag2, true);
					if (text != null)
					{
						text = Module_MyFunctions.Parse_Page_Data(text, "#updater.start#|#updater.end#", false, false, false, true);
						if (Operators.CompareString(text, "", false) != 0)
						{
							Module_Internet.WEB_Updater_DATA = text;
						}
					}
					result = true;
				}
				else
				{
					string arg_99_0 = text2;
					bool arg_99_1 = false;
					bool arg_99_2 = true;
					bool flag2 = false;
					bool flag = false;
					string text4 = Module_Internet.WEB_Get(arg_99_0, arg_99_1, arg_99_2, flag2, flag, true);
					if (text4 == null)
					{
						result = false;
					}
					else
					{
						if (text4.Contains("#MPS_DATA"))
						{
							Module_Internet.WEB_Site_DATA = text4;
							try
							{
								string text5 = Module_MyFunctions.Parse_Page_Data(Module_Internet.WEB_Site_DATA, Module_Watermark.PRG_KEY_LIC_URL, false, false, false, true);
								if (Operators.CompareString(text5, "", false) != 0)
								{
									string[] array = Strings.Split(text5, "|", -1, CompareMethod.Binary);
									Module_Watermark.PRG_LIC_URL_Main = array[0];
									if (array.Length > 1)
									{
										Module_Watermark.PRG_LIC_URL_Mirror = array[1];
									}
								}
								Module_Watermark.PRG_LIC_BlackList = Module_MyFunctions.Parse_Page_Data(Module_Internet.WEB_Site_DATA, Module_Watermark.PRG_KEY_LIC_BlackList, false, false, false, true);
							}
							catch (Exception expr_12B)
							{
								ProjectData.SetProjectError(expr_12B);
								Exception ex = expr_12B;
								Module_MyFunctions.Update_LOG("ERROR [WEB_InitDATA.lic] [" + ex.Message + "]", false, "", false);
								ProjectData.ClearProjectError();
							}
							result = true;
						}
						else
						{
							result = false;
						}
					}
				}
			}
			catch (Exception expr_164)
			{
				ProjectData.SetProjectError(expr_164);
				Exception ex2 = expr_164;
				Module_MyFunctions.Update_LOG("ERROR [WEB_InitDATA] [" + ex2.Message + "]", false, "", false);
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static bool WEB_CheckUpdate(bool my_Silence = false, NotifyIcon my_NotifyIcon = null)
		{
			bool result;
			try
			{
				if (my_NotifyIcon != null)
				{
					my_NotifyIcon.BalloonTipTitle = "Checking for new versions...";
					my_NotifyIcon.BalloonTipIcon = ToolTipIcon.Info;
					my_NotifyIcon.BalloonTipText = " ";
					my_NotifyIcon.ShowBalloonTip(2000);
				}
				bool flag = Module_Internet.WEB_Init_DATA(false);
				if (my_NotifyIcon != null)
				{
					my_NotifyIcon.Visible = false;
					my_NotifyIcon.Visible = true;
				}
				if (!flag)
				{
					if (!my_Silence)
					{
						MessageBox.Show("Network error, cannot contact website. Please retry.", Module_Watermark.PRG_Name, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					result = false;
				}
				else
				{
					if (Operators.CompareString(Module_Internet.WEB_Site_DATA, "", false) != 0)
					{
						string text = Module_MyFunctions.Parse_Page_Data(Module_Internet.WEB_Site_DATA, Module_Watermark.PRG_KEY_UPD_URL, false, false, false, true);
						string text2 = Module_MyFunctions.Parse_Page_Data(Module_Internet.WEB_Site_DATA, Module_Watermark.PRG_KEY_UPD_MD5, false, false, false, true);
						string text3 = Module_MyFunctions.Parse_Page_Data(Module_Internet.WEB_Site_DATA, Module_Watermark.PRG_KEY_UPD_DAT, false, false, false, true);
						string text4 = Module_MyFunctions.Parse_Page_Data(Module_Internet.WEB_Site_DATA, Module_Watermark.PRG_KEY_UPD_NEW, false, false, false, true);
						if (Operators.CompareString(text, "", false) != 0)
						{
							Module_Internet.WEB_EXEUPD_URL = text;
						}
						if (Operators.CompareString(text2, "", false) != 0)
						{
							Module_Internet.WEB_EXEUPD_MD5 = text2;
						}
						if (Operators.CompareString(text3, "", false) != 0)
						{
							Module_Internet.WEB_EXEUPD_DAT = text3;
						}
						if (Operators.CompareString(text4, "", false) != 0)
						{
							Module_Internet.WEB_EXEUPD_NEW = text4;
						}
						string text5 = Module_MyFunctions.Parse_Page_Data(Module_Internet.WEB_Site_DATA, Module_Watermark.PRG_KEY_Version, false, false, false, true);
						if (Operators.CompareString(text5, Module_Watermark.PRG_Version, false) != 0)
						{
							Module_Internet.WEB_Init_DATA(true);
							MyProject.Forms.Form_VerDwl.showVerDwl(text5);
							result = true;
						}
						else
						{
							if (!my_Silence)
							{
								Module_MyFunctions.my_MsgBox("You have the latest version!", MessageBoxIcon.Asterisk, false);
							}
							result = true;
						}
					}
					else
					{
						Application.DoEvents();
						result = true;
					}
				}
			}
			catch (Exception expr_1AB)
			{
				ProjectData.SetProjectError(expr_1AB);
				Exception ex = expr_1AB;
				if (my_NotifyIcon != null)
				{
					my_NotifyIcon.Visible = false;
					my_NotifyIcon.Visible = true;
				}
				Module_MyFunctions.Update_LOG("ERROR [WEB_CheckUpdate] [" + ex.Message + "]", false, "", false);
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static bool WEB_Verify_MD5_EXE()
		{
            return true;
			bool result = true;
			try
			{
				MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
				FileStream fileStream = new FileStream(Application.ExecutablePath, FileMode.Open, FileAccess.Read);
				string text = Convert.ToBase64String(mD5CryptoServiceProvider.ComputeHash(fileStream));
				string text2 = Module_Watermark.PRG_KEY_MD5 + Module_Watermark.PRG_Version + ":";
				fileStream.Close();
				string text3 = Module_MyFunctions.Config_Read("MD5_EXE", "", true);
				if (Operators.CompareString(text3, "", false) != 0 && Operators.CompareString(text3, text, false) == 0)
				{
					result = true;
				}
				else
				{
					if (Operators.CompareString(Module_Internet.WEB_Site_DATA, "", false) == 0)
					{
						Module_Internet.WEB_Init_DATA(false);
					}
					if (Operators.CompareString(Module_Internet.WEB_Site_DATA, "", false) != 0)
					{
						if (Module_Internet.WEB_Site_DATA.Contains(text2))
						{
							text3 = Module_MyFunctions.Parse_Page_Data(Module_Internet.WEB_Site_DATA, text2 + "|#", false, false, false, true);
							if (Operators.CompareString(text, text3, false) == 0)
							{
								Module_MyFunctions.Config_Save("MD5_EXE", text3, "", false, true);
								result = true;
							}
							else
							{
								if (Module_MyFunctions.my_MsgBox("Warning: the application executable has been modified.\r\n\r\nPlease make sure you have downloaded it from the official site:\r\n\r\n" + Module_Internet.WEB_Site_URL + "\r\n\r\nDo you want to visit it now?", MessageBoxIcon.Hand, true) == DialogResult.Yes)
								{
									Process.Start(Module_Internet.WEB_Site_URL);
								}
								result = false;
							}
						}
						else
						{
							result = true;
						}
					}
					else
					{
						result = true;
					}
				}
			}
			catch (Exception expr_130)
			{
				ProjectData.SetProjectError(expr_130);
				Exception ex = expr_130;
				Module_MyFunctions.Update_LOG("ERROR [WEB_Verify_MD5_EXE] [" + ex.Message + "]", false, "", false);
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static bool NET_Send_Email(MailAddress my_EmailFROM, MailAddress my_EmailTO, MailAddress my_EmailBCC, string my_Subject, string my_Body, MailPriority my_Priority)
		{
			Module_Internet.NET_Error_Send_EMail = "";
			bool result;
			try
			{
				MailMessage mailMessage = new MailMessage();
				SmtpClient smtpClient = new SmtpClient();
				mailMessage.From = my_EmailFROM;
				mailMessage.To.Add(my_EmailTO);
				if (my_EmailBCC != null)
				{
					mailMessage.Bcc.Add(my_EmailBCC);
				}
				mailMessage.Subject = my_Subject;
				mailMessage.Priority = my_Priority;
				mailMessage.Body = my_Body;
				smtpClient.Host = Module_Internet.WEB_SMTP_Config.SMTP_Address;
				smtpClient.Port = Module_Internet.WEB_SMTP_Config.SMTP_Port;
				smtpClient.Credentials = new NetworkCredential(Module_Internet.WEB_SMTP_Config.SMTP_User, Module_Internet.WEB_SMTP_Config.SMTP_Password);
				smtpClient.EnableSsl = Module_Internet.WEB_SMTP_Config.SMTP_SSL;
				smtpClient.Send(mailMessage);
				result = true;
			}
			catch (Exception expr_A9)
			{
				ProjectData.SetProjectError(expr_A9);
				Exception ex = expr_A9;
				Module_MyFunctions.Update_LOG(string.Concat(new string[]
				{
					"ERROR [NET_Send_Email(",
					my_EmailTO.Address,
					")] [",
					ex.Message,
					"]"
				}), false, "", false);
				Module_Internet.NET_Error_Send_EMail = ex.Message;
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		private static void verify_WEB_Error(string my_ErrMsg, bool my_Error404 = false, bool my_ErrorTimeout = false)
		{
			Module_Internet.WEB_Error_MSG = my_ErrMsg;
			if (my_ErrMsg.Contains("404"))
			{
				Module_Internet.WEB_Error_404 = true;
				my_Error404 = true;
			}
			else
			{
				if (my_ErrMsg.Contains("407"))
				{
					Module_Internet.WEB_Error_Proxy = true;
				}
				else
				{
					if (my_ErrMsg.Contains("408") | my_ErrMsg.Contains("[TIMEOUT]"))
					{
						my_ErrorTimeout = true;
					}
					if (my_ErrMsg.Contains("403") | my_ErrMsg.Contains("407") | my_ErrMsg.Contains("500") | my_ErrMsg.Contains("501") | my_ErrMsg.Contains("502") | my_ErrMsg.Contains("503") | my_ErrMsg.Contains("504") | my_ErrMsg.Contains("505"))
					{
						my_ErrorTimeout = false;
					}
				}
			}
		}
	}
}
