using System;
using System.Text;
namespace Persits.PDF
{
	internal class PdfString : PdfStream
	{
		public bool m_bAnsi;
		public bool m_bText;
		public PdfString()
		{
			this.m_bAnsi = false;
			this.m_nType = enumType.pdfString;
			this.m_bText = false;
		}
		public PdfString(string Name, byte[] Value)
		{
			this.m_bAnsi = true;
			this.m_bText = true;
			this.m_bstrType = Name;
			this.m_nType = enumType.pdfString;
			base.Set(Value);
		}
		public PdfString(string Name, PdfStream Value)
		{
			this.m_bstrType = Name;
			this.m_nType = enumType.pdfString;
			this.m_bText = false;
			this.m_bAnsi = false;
			if (Value != null)
			{
				base.Append(Value);
			}
		}
		public PdfString(string Name, string Value)
		{
			this.m_bstrType = Name;
			this.m_bText = true;
			this.m_bAnsi = false;
			this.m_nType = enumType.pdfString;
			if (Value == null)
			{
				return;
			}
			bool flag = true;
			for (int i = 0; i < Value.Length; i++)
			{
				char c = Value[i];
				if (c > 'ÿ')
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				byte[] array = new byte[Value.Length];
				for (int j = 0; j < Value.Length; j++)
				{
					array[j] = (byte)Value[j];
				}
				base.Set(array);
			}
			else
			{
				base.Set(Encoding.Unicode.GetBytes(Value));
			}
			this.m_bAnsi = flag;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			base.WriteOutName(pOutput, ref result);
			PdfStream pdfStream = new PdfStream();
			if (this.m_bText)
			{
				pdfStream.Alloc(base.Length + 2);
				bool flag = base.Length > 1 && (int)(base[0] + base[1]) == 509;
				if (!this.m_bAnsi && !flag)
				{
					pdfStream.Append(new byte[]
					{
						255,
						254
					});
				}
				pdfStream.Append(this);
				pOutput.Write("(", ref result);
				if (pOutput.m_bEncrypt)
				{
					if (pOutput.m_pEncKey.m_nPtr == 1)
					{
						PdfAES.Encrypt(pOutput.m_pEncKey, pOutput.m_pEncKey.Length, pdfStream);
					}
					else
					{
						pdfStream.Encrypt(pOutput.m_pEncKey.m_objMemStream.ToArray());
					}
				}
				pdfStream.EscapeSymbols();
				pOutput.Write(pdfStream, ref result);
				pOutput.Write(")", ref result);
			}
			else
			{
				pOutput.Write("<", ref result);
				pdfStream.Append(this);
				if (pOutput.m_bEncrypt)
				{
					if (pOutput.m_pEncKey.m_nPtr == 1)
					{
						PdfAES.Encrypt(pOutput.m_pEncKey, pOutput.m_pEncKey.Length, pdfStream);
					}
					else
					{
						pdfStream.Encrypt(pOutput.m_pEncKey.m_objMemStream.ToArray());
					}
				}
				pdfStream.Encode(enumEncoding.PdfEncAsciiHex);
				pOutput.Write(pdfStream, ref result);
				pOutput.Write(">", ref result);
			}
			return result;
		}
		public void SetRandomID()
		{
			DateTime now = DateTime.Now;
			base.MD5FromPaddedString(now.ToString() + now.Millisecond.ToString(), 0);
		}
		public override void TestForAnsi()
		{
			if (base.Length > 1 && (int)(base[0] + base[1]) == 509)
			{
				this.m_bAnsi = false;
				return;
			}
			this.m_bAnsi = true;
		}
		public override string ToString()
		{
			if (base.Length <= 0)
			{
				return "";
			}
			if (this.m_bAnsi)
			{
				byte[] array = this.m_objMemStream.ToArray();
				char[] array2 = new char[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = (char)array[i];
				}
				return new string(array2);
			}
			if (base.Length >= 2 && (int)(base[0] + base[1]) == 509)
			{
				bool bigEndian = base[0] == 254 && base[1] == 255;
				byte[] array3 = this.m_objMemStream.ToArray();
				byte[] array4 = new byte[array3.Length - 2];
				Array.Copy(array3, 2, array4, 0, array4.Length);
				UnicodeEncoding unicodeEncoding = new UnicodeEncoding(bigEndian, false);
				return unicodeEncoding.GetString(array4);
			}
			UnicodeEncoding unicodeEncoding2 = new UnicodeEncoding();
			return unicodeEncoding2.GetString(this.m_objMemStream.ToArray());
		}
		public DateTime ToDate()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			if (base.Length < 4)
			{
				return DateTime.MinValue;
			}
			int num7 = 0;
			if (base[num7] == 68 || base[num7] == 100)
			{
				num7++;
				if (base[num7] == 58)
				{
					num7++;
				}
			}
			if (base.Length - num7 < 4)
			{
				return DateTime.MinValue;
			}
			for (int i = 0; i < 4; i++)
			{
				num = num * 10 + (int)(base[num7++] - 48);
			}
			if (num / 10 == 191)
			{
				num = num * 10 + (int)(base[num7++] - 48);
				num -= 17100;
			}
			if (base.Length - num7 >= 2)
			{
				for (int j = 0; j < 2; j++)
				{
					num2 = num2 * 10 + (int)(base[num7++] - 48);
				}
			}
			else
			{
				num2 = 1;
			}
			if (base.Length - num7 >= 2)
			{
				for (int k = 0; k < 2; k++)
				{
					num3 = num3 * 10 + (int)(base[num7++] - 48);
				}
			}
			else
			{
				num3 = 1;
			}
			if (base.Length - num7 >= 2)
			{
				for (int l = 0; l < 2; l++)
				{
					num4 = num4 * 10 + (int)(base[num7++] - 48);
				}
			}
			if (base.Length - num7 >= 2)
			{
				for (int m = 0; m < 2; m++)
				{
					num5 = num5 * 10 + (int)(base[num7++] - 48);
				}
			}
			if (base.Length - num7 >= 2)
			{
				for (int n = 0; n < 2; n++)
				{
					num6 = num6 * 10 + (int)(base[num7++] - 48);
				}
			}
			DateTime result;
			try
			{
				result = new DateTime(num, num2, num3, num4, num5, num6);
			}
			catch (Exception)
			{
				try
				{
					result = DateTime.Parse(this.ToString());
				}
				catch (Exception)
				{
					return DateTime.MinValue;
				}
			}
			int num8 = 0;
			int num9 = 0;
			if (base.Length - num7 >= 1)
			{
				byte b = base[num7++];
				if (base.Length - num7 >= 2)
				{
					for (int num10 = 0; num10 < 2; num10++)
					{
						num8 = num8 * 10 + (int)(base[num7++] - 48);
					}
				}
				if (base.Length - num7 >= 4)
				{
					num7++;
					for (int num11 = 0; num11 < 2; num11++)
					{
						num9 = num9 * 10 + (int)(base[num7++] - 48);
					}
				}
				int num12 = (b == 43) ? -1 : 1;
				result.AddDays((double)num12 * (double)(num8 * 60 + num9) / 1440.0);
				result.ToLocalTime();
			}
			return result;
		}
		public int ToLong()
		{
			int num = 0;
			int num2 = base.Length;
			if (num2 > 4)
			{
				num2 = 4;
			}
			for (int i = 0; i < num2; i++)
			{
				num = (num << 8) + (int)base[i];
			}
			return num;
		}
		public string ToUnicode(int nCodePage)
		{
			if (!this.m_bAnsi || nCodePage == 0)
			{
				return this.ToString();
			}
			Encoding encoding;
			try
			{
				encoding = Encoding.GetEncoding(nCodePage);
			}
			catch (Exception)
			{
				return this.ToString();
			}
			return encoding.GetString(base.ToBytes());
		}
		public override PdfObject Copy()
		{
			return new PdfString(this.m_bstrType, this)
			{
				m_bAnsi = this.m_bAnsi,
				m_bText = this.m_bText
			};
		}
	}
}
