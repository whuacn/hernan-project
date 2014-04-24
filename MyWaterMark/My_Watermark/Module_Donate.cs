using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using My_Watermark.My;
using System;
using System.Threading;
using System.Web;
using System.Windows.Forms;
namespace My_Watermark
{
	[StandardModule]
	internal sealed class Module_Donate
	{
		public static bool PRG_Donate = false;
		public static string PRG_Donate_Last_State = "";
		public static bool PRG_Donate_Bundle = false;
		public static bool PRG_Donate_Bundle_License_Expired = false;
		public static int PRG_Donate_Bundle_License_Months = 6;
		public static string PRG_Donate_Bundle_License_Months_Desc = "SIX MONTHS";
		public static bool DAT_Bundle_Question_Wait = false;
		public static bool DAT_Bundle_Question_ResponseY = false;
		public static bool DAT_Bundle_Wait = false;
		private static string PRV_DonateCheckURL = "http://www.myportablesoftware.com/donateck.aspx";
		public static void Show_Bundle_Question()
		{
			Module_MyFunctions.Config_Save("lastInstalledVer", Module_Watermark.PRG_Version, "", false, true);
			Module_Donate.DAT_Bundle_Question_Wait = true;
			MyProject.Forms.Form_Bundle_Q.Show();
			while (Module_Donate.DAT_Bundle_Question_Wait)
			{
				if (Module_MyFunctions.PRG_Exit)
				{
					return;
				}
				Application.DoEvents();
				Thread.Sleep(10);
			}
		}
		public static bool Donate_Check(int my_Days)
		{
			bool result;
			try
			{
				if (!Module_Donate.Donate_Verify())
				{
					Module_Donate.PRG_Donate = false;
					string left = Module_MyFunctions.Config_Read("lastInstalledVer", "", true);
					if (Operators.CompareString(left, Module_Watermark.PRG_Version, false) != 0)
					{
						Module_Donate.Show_Bundle_Question();
						if (Module_MyFunctions.PRG_Exit)
						{
							result = true;
							return result;
						}
					}
					string text = Module_MyFunctions.Config_Read("lastLaunched", "", true);
					string text2 = string.Concat(new string[]
					{
						Conversions.ToString(DateAndTime.Now.Year),
						"-",
						Strings.Right("0" + Conversions.ToString(DateAndTime.Now.Month), 2),
						"-",
						Strings.Right("0" + Conversions.ToString(DateAndTime.Now.Day), 2)
					});
					if (Operators.CompareString(text, "", false) == 0)
					{
						text = text2;
						Module_MyFunctions.Config_Save("lastLaunched", text2, "", false, true);
					}
					string[] array = Strings.Split(text, "-", -1, CompareMethod.Binary);
					DateTime t = new DateTime(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]));
					if (DateTime.Compare(DateAndTime.Now, t.AddDays((double)my_Days)) > 0 | my_Days == 0 | DateTime.Compare(t, DateAndTime.Now) > 0)
					{
						Module_MyFunctions.Config_Save("lastLaunched", text2, "", false, true);
						MyProject.Forms.Form_Donate.Show();
					}
					result = false;
				}
				else
				{
					Module_Donate.PRG_Donate = true;
					result = true;
				}
			}
			catch (Exception expr_196)
			{
				ProjectData.SetProjectError(expr_196);
				try
				{
					Module_MyFunctions.Config_Save("lastLaunched", string.Concat(new string[]
					{
						Conversions.ToString(DateAndTime.Now.Year),
						"-",
						Strings.Right("0" + Conversions.ToString(DateAndTime.Now.Month), 2),
						"-",
						Strings.Right("0" + Conversions.ToString(DateAndTime.Now.Day), 2)
					}), "", false, true);
					if (!Module_Donate.Donate_Verify())
					{
						Module_Donate.PRG_Donate = false;
						MyProject.Forms.Form_Donate.Show();
						result = false;
						ProjectData.ClearProjectError();
					}
					else
					{
						Module_Donate.PRG_Donate = true;
						result = true;
						ProjectData.ClearProjectError();
					}
				}
				catch (Exception expr_26A)
				{
					ProjectData.SetProjectError(expr_26A);
					Module_Donate.PRG_Donate = false;
					result = false;
					ProjectData.ClearProjectError();
				}
			}
			return result;
		}
		private static bool Donate_Verify()
		{
            return true;
			string text = Module_MyFunctions.Config_Read("Donate_Email", "", true);
			string text2 = Module_MyFunctions.Config_Read("Donate_ID", "", true);
			string left = Module_MyFunctions.Config_Read("Donate_Code", "", true);
			Module_Donate.PRG_Donate_Bundle = false;
			if (Operators.CompareString(text, "", false) == 0 | Operators.CompareString(left, "", false) == 0 | Operators.CompareString(text2, "", false) == 0)
			{
				if (Module_Donate.Donate_Bundle_Verify())
				{
					return true;
				}
				Module_Donate.PRG_Donate_Last_State = "N";
				return false;
			}
			else
			{
				if (Operators.CompareString(left, Module_MyFunctions.Calculate_MD5(text), false) != 0)
				{
					return Operators.CompareString(Module_Donate.Donate_Check_Email(text, text2), "D", false) == 0 || Module_Donate.Donate_Bundle_Verify();
				}
				if (Module_MyFunctions.DAT_Command)
				{
					Module_Donate.PRG_Donate_Last_State = "D";
					return true;
				}
				string left2 = Module_Donate.Donate_Check_Email(text, text2);
				return (Operators.CompareString(left2, "E", false) == 0 | Operators.CompareString(left2, "D", false) == 0) || Module_Donate.Donate_Bundle_Verify();
			}
		}
		private static bool Donate_Bundle_Verify()
		{
            return true;
			bool result;
			try
			{
				string text = Module_MyFunctions.Config_Read("DWLlicense", "", true);
				if (Operators.CompareString(text, "", false) == 0)
				{
					result = false;
				}
				else
				{
					string text2 = Module_Encrypter.my_Decrypt(text, null);
					string expression = Module_MyFunctions.Parse_Page_Data(text2, "#mpslic.start#|#mpslic.end#", false, false, false, true);
					string str = Module_MyFunctions.Parse_Page_Data(text2, "#mpslicoriginal.start#|#mpslicoriginal.end#", false, false, false, true);
					if (Module_Watermark.PRG_LIC_BlackList.Contains("|" + str + "|"))
					{
						Module_Donate.PRG_Donate_Bundle_License_Expired = true;
						result = false;
					}
					else
					{
						string[] array = Strings.Split(expression, "-", -1, CompareMethod.Binary);
						DateTime t = new DateTime(Conversions.ToInteger(array[0]), Conversions.ToInteger(array[1]), Conversions.ToInteger(array[2]));
						if (DateTime.Compare(t, DateAndTime.Now) < 0)
						{
							Module_Donate.PRG_Donate_Bundle_License_Expired = true;
							result = false;
						}
						else
						{
							Module_Donate.PRG_Donate_Bundle_License_Expired = false;
							Module_Donate.PRG_Donate_Last_State = "D";
							Module_Donate.PRG_Donate_Bundle = true;
							result = true;
						}
					}
				}
			}
			catch (Exception expr_EC)
			{
				ProjectData.SetProjectError(expr_EC);
				Exception ex = expr_EC;
				Module_MyFunctions.Update_LOG("ERROR [Donate_Bundle_Verify] [" + ex.Message + "]", false, "", false);
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static string Donate_Check_Email(string my_Email, string my_ID)
		{
			string text = "";
			string text2 = Module_Donate.PRV_DonateCheckURL;
			string text3 = Module_MyFunctions.Calculate_MD5(my_Email);
			string str = Module_MyFunctions.Calculate_MD5(my_ID);
			if (Module_MyFunctions.DEBUG_MODE)
			{
				text2 = "http://localhost:1958/donateck.aspx";
			}
			string text4 = string.Concat(new string[]
			{
				text2,
				"?RID=",
				HttpUtility.UrlEncode(text3),
				"&PID=",
				HttpUtility.UrlEncode(str),
				"&PRG=",
				HttpUtility.UrlEncode(Module_Watermark.PRG_Name.Replace(" ", "") + Module_Watermark.PRG_Version)
			});
			string text5 = string.Concat(new string[]
			{
				text4,
				"&REPORT=",
				HttpUtility.UrlEncode(my_Email),
				"|",
				HttpUtility.UrlEncode(my_ID),
				"|",
				HttpUtility.UrlEncode(Module_Watermark.PRG_Name.Replace(" ", "") + Module_Watermark.PRG_Version)
			});
			string arg_120_0 = text4;
			bool arg_120_1 = false;
			bool arg_120_2 = true;
			bool flag = false;
			bool flag2 = false;
			text = Module_Internet.WEB_Get(arg_120_0, arg_120_1, arg_120_2, flag, flag2, false);
			if (text == null)
			{
				Module_Donate.PRG_Donate_Last_State = "E";
				return "E";
			}
			if (Operators.CompareString(text, "", false) == 0 | text.Contains("#MPS.ERR#") | !text.Contains("#MPS.DCK"))
			{
				Module_Donate.PRG_Donate_Last_State = "E";
				return "E";
			}
			if (text.Contains("#MPS.DCK.BL#"))
			{
				Module_MyFunctions.Config_Save("Donate_Code", "", "", false, true);
				Module_Donate.PRG_Donate_Last_State = "B";
				return "B";
			}
			if (text.Contains("#MPS.DCK.PKO#"))
			{
				string str2 = Module_MyFunctions.Parse_Page_Data(text, "#MPS.DCK.HINT#|#", false, false, false, true);
				Module_MyFunctions.Config_Save("Donate_Code", "", "", false, true);
				string arg_1F4_0 = text5;
				bool arg_1F4_1 = false;
				bool arg_1F4_2 = true;
				flag2 = false;
				flag = false;
				Module_Internet.WEB_Get(arg_1F4_0, arg_1F4_1, arg_1F4_2, flag2, flag, false);
				Module_Donate.PRG_Donate_Last_State = "I";
				return "I" + str2;
			}
			if (text.Contains("#MPS.DCK.OK#"))
			{
				Module_MyFunctions.Config_Save("Donate_Email", my_Email, "", false, true);
				Module_MyFunctions.Config_Save("Donate_ID", my_ID, "", false, true);
				Module_MyFunctions.Config_Save("Donate_Code", text3, "", false, true);
				Module_Donate.PRG_Donate_Last_State = "D";
				return "D";
			}
			string arg_277_0 = text5;
			bool arg_277_1 = false;
			bool arg_277_2 = true;
			flag2 = false;
			flag = false;
			Module_Internet.WEB_Get(arg_277_0, arg_277_1, arg_277_2, flag2, flag, false);
			Module_Donate.PRG_Donate_Last_State = "N";
			return "";
		}
	}
}
