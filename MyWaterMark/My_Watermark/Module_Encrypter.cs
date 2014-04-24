using Microsoft.VisualBasic.CompilerServices;
using System;
using System.IO;
using System.Security.Cryptography;
namespace My_Watermark
{
	[StandardModule]
	internal sealed class Module_Encrypter
	{
		public static string my_Encrypt(string my_Data, byte[] my_Password = null)
		{
			string result;
			try
			{
				if (Operators.CompareString(my_Data, "", false) == 0)
				{
					result = "";
				}
				else
				{
					if (my_Password == null)
					{
						my_Password = Module_Watermark.CFG_PwdBytes;
					}
					result = Convert.ToBase64String(Module_Encrypter.encryptString_AES(my_Data, my_Password), Base64FormattingOptions.None);
				}
			}
			catch (Exception expr_31)
			{
				ProjectData.SetProjectError(expr_31);
				Exception ex = expr_31;
				Module_MyFunctions.Update_LOG("ERROR [my_Encrypt(" + my_Data + ")] " + ex.Message, false, "", false);
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static string my_Encrypt(byte[] my_Data, byte[] my_Password = null)
		{
			string result;
			try
			{
				if (my_Data.Length == 0)
				{
					result = "";
				}
				else
				{
					if (my_Password == null)
					{
						my_Password = Module_Watermark.CFG_PwdBytes;
					}
					result = Convert.ToBase64String(Module_Encrypter.encryptBytes_AES(my_Data, my_Password), Base64FormattingOptions.None);
				}
			}
			catch (Exception expr_28)
			{
				ProjectData.SetProjectError(expr_28);
				Exception ex = expr_28;
				Module_MyFunctions.Update_LOG("ERROR [my_Encrypt(bytes[])] " + ex.Message, false, "", false);
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static string my_Decrypt(string my_EncData, byte[] my_Password = null)
		{
			string result;
			try
			{
				if (Operators.CompareString(my_EncData, "", false) == 0)
				{
					result = "";
				}
				else
				{
					if (my_Password == null)
					{
						my_Password = Module_Watermark.CFG_PwdBytes;
					}
					result = Module_Encrypter.decryptString_AES(Convert.FromBase64String(my_EncData), my_Password);
				}
			}
			catch (Exception expr_30)
			{
				ProjectData.SetProjectError(expr_30);
				Exception ex = expr_30;
				Module_MyFunctions.Update_LOG("ERROR [my_Decrypt(" + my_EncData + ")] " + ex.Message, false, "", false);
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}
		public static byte[] my_DecryptBytes(string my_EncData, byte[] my_Password = null)
		{
			byte[] result;
			try
			{
				if (my_EncData.Length == 0)
				{
					result = null;
				}
				else
				{
					if (my_Password == null)
					{
						my_Password = Module_Watermark.CFG_PwdBytes;
					}
					result = Module_Encrypter.decryptBytes_AES(Convert.FromBase64String(my_EncData), my_Password);
				}
			}
			catch (Exception expr_26)
			{
				ProjectData.SetProjectError(expr_26);
				Exception ex = expr_26;
				Module_MyFunctions.Update_LOG("ERROR [my_Decrypt(bytes[])] " + ex.Message, false, "", false);
				result = null;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		private static byte[] encrypter_PadBytes(byte[] my_Bytes, int my_Length)
		{
			checked
			{
				byte[] array = new byte[my_Length - 1 + 1];
				byte[] array2 = new byte[]
				{
					61
				};
				int arg_24_0 = 0;
				int num = my_Length - 1;
				for (int i = arg_24_0; i <= num; i++)
				{
					array[i] = array2[0];
				}
				int arg_3E_0 = 0;
				int num2 = my_Bytes.Length - 1;
				for (int i = arg_3E_0; i <= num2; i++)
				{
					if (i <= array.Length - 1)
					{
						array[i] = my_Bytes[i];
					}
				}
				return array;
			}
		}
		private static byte[] encryptString_AES(string plainText, byte[] my_Key)
		{
			byte[] result;
			try
			{
				byte[] array = Module_Encrypter.encrypter_PadBytes(my_Key, 32);
				byte[] array2 = Module_Encrypter.encrypter_PadBytes(my_Key, 16);
				if (plainText == null || plainText.Length <= 0)
				{
					throw new ArgumentNullException("plainText");
				}
				if (array == null || array.Length <= 0)
				{
					throw new ArgumentNullException("Key");
				}
				if (array2 == null || array2.Length <= 0)
				{
					throw new ArgumentNullException("IV");
				}
				RijndaelManaged rijndaelManaged = null;
				MemoryStream memoryStream = null;
				try
				{
					rijndaelManaged = new RijndaelManaged();
					rijndaelManaged.Key = array;
					rijndaelManaged.IV = array2;
					ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
					memoryStream = new MemoryStream();
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
						{
							streamWriter.Write(plainText);
						}
					}
				}
				finally
				{
					if (rijndaelManaged != null)
					{
						rijndaelManaged.Clear();
					}
				}
				result = memoryStream.ToArray();
			}
			catch (Exception expr_D4)
			{
				ProjectData.SetProjectError(expr_D4);
				Exception ex = expr_D4;
				Module_MyFunctions.Update_LOG("ERROR [encryptString_AES()] " + ex.Message, false, "", false);
				byte[] array3 = new byte[]
				{
					0
				};
				result = array3;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		private static byte[] encryptBytes_AES(byte[] plainBytes, byte[] my_Key)
		{
			byte[] result;
			try
			{
				byte[] array = Module_Encrypter.encrypter_PadBytes(my_Key, 32);
				byte[] array2 = Module_Encrypter.encrypter_PadBytes(my_Key, 16);
				if (plainBytes == null || plainBytes.Length <= 0)
				{
					throw new ArgumentNullException("plainByte");
				}
				if (array == null || array.Length <= 0)
				{
					throw new ArgumentNullException("Key");
				}
				if (array2 == null || array2.Length <= 0)
				{
					throw new ArgumentNullException("IV");
				}
				RijndaelManaged rijndaelManaged = null;
				MemoryStream memoryStream = null;
				try
				{
					rijndaelManaged = new RijndaelManaged();
					rijndaelManaged.Key = array;
					rijndaelManaged.IV = array2;
					ICryptoTransform transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
					memoryStream = new MemoryStream();
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(cryptoStream))
						{
							binaryWriter.Write(plainBytes);
						}
					}
				}
				finally
				{
					if (rijndaelManaged != null)
					{
						rijndaelManaged.Clear();
					}
				}
				result = memoryStream.ToArray();
			}
			catch (Exception expr_D1)
			{
				ProjectData.SetProjectError(expr_D1);
				Exception ex = expr_D1;
				Module_MyFunctions.Update_LOG("ERROR [encryptBytes_AES()] " + ex.Message, false, "", false);
				byte[] array3 = new byte[]
				{
					0
				};
				result = array3;
				ProjectData.ClearProjectError();
			}
			return result;
		}
		private static string decryptString_AES(byte[] cipherText, byte[] my_Key)
		{
			string result;
			try
			{
				byte[] array = Module_Encrypter.encrypter_PadBytes(my_Key, 32);
				byte[] array2 = Module_Encrypter.encrypter_PadBytes(my_Key, 16);
				if (cipherText == null || cipherText.Length <= 0)
				{
					throw new ArgumentNullException("cipherText");
				}
				if (array == null || array.Length <= 0)
				{
					throw new ArgumentNullException("Key");
				}
				if (array2 == null || array2.Length <= 0)
				{
					throw new ArgumentNullException("IV");
				}
				RijndaelManaged rijndaelManaged = null;
				string text = null;
				try
				{
					rijndaelManaged = new RijndaelManaged();
					rijndaelManaged.Key = array;
					rijndaelManaged.IV = array2;
					ICryptoTransform transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
					using (MemoryStream memoryStream = new MemoryStream(cipherText))
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
						{
							using (StreamReader streamReader = new StreamReader(cryptoStream))
							{
								text = streamReader.ReadToEnd();
							}
						}
					}
				}
				catch (Exception expr_CD)
				{
					ProjectData.SetProjectError(expr_CD);
					text = "";
					ProjectData.ClearProjectError();
				}
				finally
				{
					if (rijndaelManaged != null)
					{
						rijndaelManaged.Clear();
					}
				}
				result = text;
			}
			catch (Exception expr_F2)
			{
				ProjectData.SetProjectError(expr_F2);
				Exception ex = expr_F2;
				Module_MyFunctions.Update_LOG("ERROR [decryptString_AES()] " + ex.Message, false, "", false);
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}
		private static byte[] decryptBytes_AES(byte[] cipherBytes, byte[] my_Key)
		{
			byte[] result;
			try
			{
				byte[] array = Module_Encrypter.encrypter_PadBytes(my_Key, 32);
				byte[] array2 = Module_Encrypter.encrypter_PadBytes(my_Key, 16);
				if (cipherBytes == null || cipherBytes.Length <= 0)
				{
					throw new ArgumentNullException("cipherBytes");
				}
				if (array == null || array.Length <= 0)
				{
					throw new ArgumentNullException("Key");
				}
				if (array2 == null || array2.Length <= 0)
				{
					throw new ArgumentNullException("IV");
				}
				RijndaelManaged rijndaelManaged = null;
				byte[] array3 = null;
				try
				{
					rijndaelManaged = new RijndaelManaged();
					rijndaelManaged.Key = array;
					rijndaelManaged.IV = array2;
					ICryptoTransform transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
					using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read))
						{
							using (BinaryReader binaryReader = new BinaryReader(cryptoStream))
							{
								array3 = binaryReader.ReadBytes(9999999);
							}
						}
					}
				}
				catch (Exception expr_D2)
				{
					ProjectData.SetProjectError(expr_D2);
					array3 = null;
					ProjectData.ClearProjectError();
				}
				finally
				{
					if (rijndaelManaged != null)
					{
						rijndaelManaged.Clear();
					}
				}
				result = array3;
			}
			catch (Exception expr_F3)
			{
				ProjectData.SetProjectError(expr_F3);
				Exception ex = expr_F3;
				Module_MyFunctions.Update_LOG("ERROR [decryptBytes_AES()] " + ex.Message, false, "", false);
				result = null;
				ProjectData.ClearProjectError();
			}
			return result;
		}
	}
}
