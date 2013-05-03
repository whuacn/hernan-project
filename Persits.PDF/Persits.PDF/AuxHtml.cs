using System;
using System.Text;
namespace Persits.PDF
{
	internal class AuxHtml
	{
		private const int STOPCHAR = 254;
		private char m_ch;
		private int m_nPtr;
		private int m_nLen;
		private string m_bstrText;
		private static float[] FontSizes = new float[]
		{
			0f,
			7.5f,
			10f,
			12f,
			13.5f,
			18f,
			24f,
			36f
		};
		public AuxHtml()
		{
			this.m_nPtr = 0;
			this.m_nLen = 0;
		}
		public AuxHtml(string Text)
		{
			this.Set(Text);
		}
		public void Set(string Text)
		{
			this.m_bstrText = Text + 'þ';
			this.m_nPtr = 0;
			this.m_nLen = this.m_bstrText.Length;
		}
		public bool Parse(AuxSnippetRange pRange)
		{
			try
			{
				this.Next();
				this.ParseAll(pRange);
			}
			catch (PdfException)
			{
				return false;
			}
			return true;
		}
		private void ParseAll(AuxSnippetRange pRange)
		{
			string tag;
			this.ParseName(out tag);
			while (this.m_ch != 'þ')
			{
				string name;
				this.ParseName(out name);
				if (this.m_ch != '=')
				{
					return;
				}
				this.Next();
				string value;
				this.ParseValue(name, out value);
				this.HandleTagAttribute(tag, name, value, pRange);
			}
		}
		private void HandleTagAttribute(string Tag, string Name, string Value, AuxSnippetRange pRange)
		{
			int length = Value.Length;
			if (length <= 0)
			{
				return;
			}
			if (string.Compare(Tag, 0, "font", 0, 4, true) == 0)
			{
				if (string.Compare(Name, 0, "SIZE", 0, 4, true) == 0)
				{
					if (length > 1 && (Value[0] == '+' || Value[0] == '-'))
					{
						int num = (Value[0] == '+') ? 1 : -1;
						int num2 = HtmlTag.IntParse(Value.Substring(1));
						pRange.m_nHtmlFontSize += num * num2;
					}
					else
					{
						int num2 = HtmlTag.IntParse(Value);
						pRange.m_nHtmlFontSize = num2;
					}
					if (pRange.m_nHtmlFontSize < 1)
					{
						pRange.m_nHtmlFontSize = 1;
					}
					if (pRange.m_nHtmlFontSize > 7)
					{
						pRange.m_nHtmlFontSize = 7;
					}
					pRange.m_fSize = AuxHtml.FontSizes[pRange.m_nHtmlFontSize];
				}
				if (string.Compare(Name, 0, "FACE", 0, 4, true) == 0)
				{
					pRange.m_bstrBaseFont = Value;
				}
				if (string.Compare(Name, 0, "COLOR", 0, 5, true) == 0)
				{
					float num3 = 0f;
					AuxRGB auxRGB = new AuxRGB();
					if (!PdfParam.NameToValue(Value, ref num3))
					{
						this.ParseHexColor(Value, length, out num3);
					}
					auxRGB.Set((uint)num3);
					pRange.m_fR = auxRGB.r;
					pRange.m_fG = auxRGB.g;
					pRange.m_fB = auxRGB.b;
				}
				if (string.Compare(Name, 0, "STYLE", 0, 5, true) == 0)
				{
					this.ParseStyle(Value, length, pRange);
				}
				return;
			}
			if (string.Compare(Tag, 0, "div", 0, 3, true) == 0)
			{
				if (string.Compare(Name, 0, "ALIGN", 0, 5, true) == 0)
				{
					if (string.Compare(Value, "left", true) == 0)
					{
						pRange.m_nCenter = 0;
						pRange.m_nRight = 0;
						pRange.m_nJustify = 0;
					}
					else
					{
						if (string.Compare(Value, "right", true) == 0)
						{
							pRange.m_nCenter = 0;
							pRange.m_nRight = 1;
							pRange.m_nJustify = 0;
						}
						else
						{
							if (string.Compare(Value, "center", true) == 0)
							{
								pRange.m_nCenter = 1;
								pRange.m_nRight = 0;
								pRange.m_nJustify = 0;
							}
							else
							{
								if (string.Compare(Value, "justify", true) == 0)
								{
									pRange.m_nCenter = 0;
									pRange.m_nRight = 0;
									pRange.m_nJustify = 1;
								}
							}
						}
					}
				}
				if (string.Compare(Name, 0, "STYLE", 0, 5, true) == 0)
				{
					this.ParseStyle(Value, length, pRange);
				}
				return;
			}
			if (string.Compare(Tag, 0, "a", 0, 1, true) == 0 && string.Compare(Name, 0, "HREF", 0, 4, true) == 0)
			{
				pRange.m_bstrAnchor = Value;
			}
		}
		private void ParseStyle(string Value, int nLen, AuxSnippetRange pRange)
		{
			AuxHtml auxHtml = new AuxHtml(Value);
			auxHtml.Next();
			while (true)
			{
				string name;
				auxHtml.ParseName(out name);
				if (auxHtml.m_ch != ':')
				{
					AuxException.Throw(": expected.", PdfErrors._ERROR_PARSE);
				}
				auxHtml.Next();
				string value;
				auxHtml.ParseStyleValue(out value);
				this.HandleStyleAttribute(name, value, pRange);
				if (auxHtml.m_ch != ';')
				{
					break;
				}
				auxHtml.Next();
			}
		}
		private void HandleStyleAttribute(string Name, string Value, AuxSnippetRange pRange)
		{
			if (string.Compare(Name, 0, "font-family", 0, 11, true) == 0)
			{
				this.HandleTagAttribute("font", "face", Value, pRange);
				return;
			}
			if (string.Compare(Name, 0, "color", 0, 5, true) == 0)
			{
				this.HandleTagAttribute("font", "color", Value, pRange);
				return;
			}
			if (string.Compare(Name, 0, "font-style", 0, 10, true) == 0 && string.Compare(Value, 0, "italic", 0, 6, true) == 0)
			{
				pRange.m_nItalic++;
				return;
			}
			if (string.Compare(Name, 0, "font-style", 0, 10, true) == 0 && string.Compare(Value, 0, "normal", 0, 6, true) == 0)
			{
				pRange.m_nItalic--;
				if (pRange.m_nItalic <= 0)
				{
					pRange.m_nItalic = 0;
					return;
				}
			}
			else
			{
				if (string.Compare(Name, 0, "font-weight", 0, 11, true) == 0 && string.Compare(Value, 0, "bold", 0, 4, true) == 0)
				{
					pRange.m_nBold++;
					return;
				}
				if (string.Compare(Name, 0, "font-weight", 0, 11, true) == 0 && string.Compare(Value, 0, "normal", 0, 6, true) == 0)
				{
					pRange.m_nBold--;
					if (pRange.m_nBold <= 0)
					{
						pRange.m_nBold = 0;
						return;
					}
				}
				else
				{
					if (string.Compare(Name, 0, "text-decoration", 0, 15, true) == 0 && string.Compare(Value, 0, "underline", 0, 9, true) == 0)
					{
						pRange.m_nUnderlined++;
						return;
					}
					if (string.Compare(Name, 0, "text-decoration", 0, 15, true) == 0 && string.Compare(Value, 0, "none", 0, 4, true) == 0)
					{
						pRange.m_nUnderlined--;
						if (pRange.m_nUnderlined <= 0)
						{
							pRange.m_nUnderlined = 0;
							return;
						}
					}
					else
					{
						if (string.Compare(Name, 0, "font-size", 0, 9, true) == 0)
						{
							this.ParseFontSize(Value, out pRange.m_fSize);
						}
					}
				}
			}
		}
		private void ParseFontSize(string Size, out float pSize)
		{
			pSize = 0f;
			float num = 0f;
			AuxHtml auxHtml = new AuxHtml(Size);
			auxHtml.Next();
			auxHtml.ParseFloat(out num);
			if (num == 0f)
			{
				return;
			}
			if (auxHtml.m_ch == '%')
			{
				pSize *= num / 100f;
				return;
			}
			string text;
			auxHtml.ParseName(out text);
			if (text.Length == 0)
			{
				pSize = num;
				return;
			}
			if (string.Compare(text, 0, "pt", 0, 2, true) == 0)
			{
				pSize = num;
				return;
			}
			if (string.Compare(text, 0, "in", 0, 2, true) == 0)
			{
				pSize = num * 72f;
				return;
			}
			if (string.Compare(text, 0, "cm", 0, 2, true) == 0)
			{
				pSize = num * 28.3465f;
				return;
			}
			if (string.Compare(text, 0, "mm", 0, 2, true) == 0)
			{
				pSize = num * 2.83465f;
			}
		}
		private void ParseFloat(out float pFloat)
		{
			int num = 0;
			int num2 = 0;
			if (this.m_ch == '.')
			{
				this.Next();
				this.ParseInt(out num);
			}
			else
			{
				this.ParseInt(out num2);
				if (this.m_ch == '.')
				{
					this.Next();
					this.ParseInt(out num);
				}
			}
			float num3;
			for (num3 = (float)num; num3 >= 1f; num3 /= 10f)
			{
			}
			num3 += (float)num2;
			pFloat = num3;
		}
		private void ParseInt(out int pInt)
		{
			int num = 0;
			while (this.m_ch >= '0' && this.m_ch <= '9')
			{
				num = num * 10 + (int)(this.m_ch - '0');
				this.Next();
				if (num > 429496729)
				{
					AuxException.Throw("Too many digits.", PdfErrors._ERROR_PARSE);
				}
			}
			pInt = num;
		}
		private void ParseStyleValue(out string pValue)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.SkipSpaces();
			if (this.m_ch == '\'')
			{
				this.Next();
				while (this.m_ch != '\'')
				{
					stringBuilder.Append(this.m_ch);
					this.Next();
				}
				if (this.m_ch != '\'')
				{
					AuxException.Throw("Closing ' expected.", PdfErrors._ERROR_PARSE);
				}
				else
				{
					this.Next();
				}
			}
			else
			{
				while (this.m_ch != ';' && this.m_ch != 'þ')
				{
					stringBuilder.Append(this.m_ch);
					this.Next();
				}
			}
			this.SkipSpaces();
			this.Trim(stringBuilder.ToString(), out pValue);
		}
		private void Trim(string StrIn, out string StrOut)
		{
			int num = StrIn.Length;
			while (StrIn[num - 1] == ' ')
			{
				num--;
			}
			StrOut = StrIn.Substring(0, num);
		}
		private void ParseHexColor(string Value, int nLen, out float pValue)
		{
			float num = 0f;
			int i = 0;
			if (Value[0] == '#')
			{
				i = 1;
			}
			while (i < nLen)
			{
				if (Value[i] >= '0' && Value[i] <= '9')
				{
					num = 16f * num + (float)(Value[i] - '0');
				}
				if (Value[i] >= 'a' && Value[i] <= 'f')
				{
					num = 16f * num + (float)(Value[i] - 'a' + '\n');
				}
				if (Value[i] >= 'A' && Value[i] <= 'F')
				{
					num = 16f * num + (float)(Value[i] - 'A' + '\n');
				}
				if (Value[i] == 'o' || Value[i] == 'O')
				{
					num = 16f * num;
				}
				i++;
			}
			pValue = num;
		}
		private void ParseName(out string pName)
		{
			this.SkipSpaces();
			StringBuilder stringBuilder = new StringBuilder();
			while (this.IsAlpha())
			{
				stringBuilder.Append(this.m_ch);
				this.Next();
			}
			pName = stringBuilder.ToString();
			this.SkipSpaces();
		}
		private void Next()
		{
			if (this.m_nPtr >= this.m_nLen)
			{
				AuxException.Throw("Reached end of input.", PdfErrors._ERROR_PARSE);
			}
			this.m_ch = this.m_bstrText[this.m_nPtr];
			this.m_nPtr++;
		}
		private void ParseValue(string Name, out string pValue)
		{
			pValue = "";
			StringBuilder stringBuilder = new StringBuilder();
			this.SkipSpaces();
			if (this.m_ch == '"')
			{
				this.Next();
				while (this.m_ch != '"')
				{
					stringBuilder.Append(this.m_ch);
					this.Next();
				}
				if (this.m_ch == '"')
				{
					pValue = stringBuilder.ToString();
					this.Next();
					return;
				}
			}
			else
			{
				AuxException.Throw("Opening \" character expected.", PdfErrors._ERROR_PARSE);
			}
		}
		private void SkipSpaces()
		{
			while (this.m_ch == ' ' || this.m_ch == '\r' || this.m_ch == '\n' || this.m_ch == '\t')
			{
				this.Next();
			}
		}
		private bool IsAlpha()
		{
			return (this.m_ch >= 'a' && this.m_ch <= 'z') || (this.m_ch >= 'A' && this.m_ch <= 'Z') || this.m_ch == '-';
		}
	}
}
