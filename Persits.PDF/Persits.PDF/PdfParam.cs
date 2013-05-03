using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	public class PdfParam
	{
		internal struct ColorNameMap
		{
			public string Name;
			public float Value;
			public ColorNameMap(string n, float v)
			{
				this.Name = n;
				this.Value = v;
			}
		}
		internal const int PARAM_LEFT = 0;
		internal const int PARAM_RIGHT = 1;
		internal const int PARAM_CENTER = 2;
		internal const int PARAM_TOP = 0;
		internal const int PARAM_BOTTOM = 1;
		internal const int PARAM_MIDDLE = 2;
		internal static PdfParam.ColorNameMap[] ColorNames = new PdfParam.ColorNameMap[]
		{
			new PdfParam.ColorNameMap("aliceblue", 15792383f),
			new PdfParam.ColorNameMap("antiquewhite", 16444375f),
			new PdfParam.ColorNameMap("aqua", 65535f),
			new PdfParam.ColorNameMap("aquamarine", 8388564f),
			new PdfParam.ColorNameMap("azure", 15794175f),
			new PdfParam.ColorNameMap("beige", 1.611926E+07f),
			new PdfParam.ColorNameMap("bisque", 16770244f),
			new PdfParam.ColorNameMap("black", 0f),
			new PdfParam.ColorNameMap("blanchedalmond", 16772045f),
			new PdfParam.ColorNameMap("blue", 255f),
			new PdfParam.ColorNameMap("blueviolet", 9055202f),
			new PdfParam.ColorNameMap("bottom", 1f),
			new PdfParam.ColorNameMap("brown", 10824234f),
			new PdfParam.ColorNameMap("burlywood", 14596231f),
			new PdfParam.ColorNameMap("button", 1f),
			new PdfParam.ColorNameMap("cadetblue", 6266528f),
			new PdfParam.ColorNameMap("center", 2f),
			new PdfParam.ColorNameMap("chartreuse", 8388352f),
			new PdfParam.ColorNameMap("chocolate", 1.378947E+07f),
			new PdfParam.ColorNameMap("choice", 2f),
			new PdfParam.ColorNameMap("circle", 5f),
			new PdfParam.ColorNameMap("cora", 16744272f),
			new PdfParam.ColorNameMap("cornflowerblue", 6591981f),
			new PdfParam.ColorNameMap("cornsilk", 16775388f),
			new PdfParam.ColorNameMap("crimson", 1.44231E+07f),
			new PdfParam.ColorNameMap("cyan", 65535f),
			new PdfParam.ColorNameMap("darkblue", 139f),
			new PdfParam.ColorNameMap("darkcyan", 35723f),
			new PdfParam.ColorNameMap("darkgoldenrod", 12092939f),
			new PdfParam.ColorNameMap("darkgray", 11119017f),
			new PdfParam.ColorNameMap("darkgreen", 25600f),
			new PdfParam.ColorNameMap("darkgrey", 11119017f),
			new PdfParam.ColorNameMap("darkkhaki", 12433259f),
			new PdfParam.ColorNameMap("darkmagenta", 9109643f),
			new PdfParam.ColorNameMap("darkolivegreen", 5597999f),
			new PdfParam.ColorNameMap("darkorange", 1.674752E+07f),
			new PdfParam.ColorNameMap("darkorchid", 10040012f),
			new PdfParam.ColorNameMap("darkred", 9109504f),
			new PdfParam.ColorNameMap("darksalmon", 1.530841E+07f),
			new PdfParam.ColorNameMap("darkseagreen", 9419919f),
			new PdfParam.ColorNameMap("darkslateblue", 4734347f),
			new PdfParam.ColorNameMap("darkslategray", 3100495f),
			new PdfParam.ColorNameMap("darkslategrey", 3100495f),
			new PdfParam.ColorNameMap("darkturquoise", 52945f),
			new PdfParam.ColorNameMap("darkviolet", 9699539f),
			new PdfParam.ColorNameMap("deeppink", 16716947f),
			new PdfParam.ColorNameMap("deepskyblue", 49151f),
			new PdfParam.ColorNameMap("dimgray", 6908265f),
			new PdfParam.ColorNameMap("dimgrey", 6908265f),
			new PdfParam.ColorNameMap("dodgerblue", 2003199f),
			new PdfParam.ColorNameMap("false", 0f),
			new PdfParam.ColorNameMap("fileattachment", 11f),
			new PdfParam.ColorNameMap("firebrick", 11674146f),
			new PdfParam.ColorNameMap("floralwhite", 1.677592E+07f),
			new PdfParam.ColorNameMap("forestgreen", 2263842f),
			new PdfParam.ColorNameMap("freetext", 2f),
			new PdfParam.ColorNameMap("fuchsia", 16711935f),
			new PdfParam.ColorNameMap("gainsboro", 1.447446E+07f),
			new PdfParam.ColorNameMap("ghostwhite", 16316671f),
			new PdfParam.ColorNameMap("gold", 1.676672E+07f),
			new PdfParam.ColorNameMap("goldenrod", 1.432912E+07f),
			new PdfParam.ColorNameMap("goto", 1f),
			new PdfParam.ColorNameMap("gotor", 2f),
			new PdfParam.ColorNameMap("gray", 8421504f),
			new PdfParam.ColorNameMap("green", 32768f),
			new PdfParam.ColorNameMap("greenyellow", 11403055f),
			new PdfParam.ColorNameMap("grey", 8421504f),
			new PdfParam.ColorNameMap("highlight", 6f),
			new PdfParam.ColorNameMap("honeydew", 1.579416E+07f),
			new PdfParam.ColorNameMap("hotpink", 1.673874E+07f),
			new PdfParam.ColorNameMap("indianred", 13458524f),
			new PdfParam.ColorNameMap("indigo", 4915330f),
			new PdfParam.ColorNameMap("ivory", 1.67772E+07f),
			new PdfParam.ColorNameMap("javascript", 13f),
			new PdfParam.ColorNameMap("khaki", 1.578766E+07f),
			new PdfParam.ColorNameMap("launch", 3f),
			new PdfParam.ColorNameMap("lavender", 1.513241E+07f),
			new PdfParam.ColorNameMap("lavenderblush", 16773365f),
			new PdfParam.ColorNameMap("lawngreen", 8190976f),
			new PdfParam.ColorNameMap("left", 0f),
			new PdfParam.ColorNameMap("lemonchiffon", 16775885f),
			new PdfParam.ColorNameMap("lightblue", 11393254f),
			new PdfParam.ColorNameMap("lightcora", 15761536f),
			new PdfParam.ColorNameMap("lightcyan", 14745599f),
			new PdfParam.ColorNameMap("lightgoldenrodyellow", 1.644821E+07f),
			new PdfParam.ColorNameMap("lightgray", 13882323f),
			new PdfParam.ColorNameMap("lightgreen", 9498256f),
			new PdfParam.ColorNameMap("lightgrey", 13882323f),
			new PdfParam.ColorNameMap("lightpink", 16758465f),
			new PdfParam.ColorNameMap("lightsalmon", 16752762f),
			new PdfParam.ColorNameMap("lightseagreen", 2142890f),
			new PdfParam.ColorNameMap("lightskyblue", 8900346f),
			new PdfParam.ColorNameMap("lightslategray", 7833753f),
			new PdfParam.ColorNameMap("lightslategrey", 7833753f),
			new PdfParam.ColorNameMap("lightsteelblue", 11584734f),
			new PdfParam.ColorNameMap("lightyellow", 16777184f),
			new PdfParam.ColorNameMap("lime", 65280f),
			new PdfParam.ColorNameMap("limegreen", 3329330f),
			new PdfParam.ColorNameMap("line", 3f),
			new PdfParam.ColorNameMap("linen", 1.644567E+07f),
			new PdfParam.ColorNameMap("link", 1f),
			new PdfParam.ColorNameMap("magenta", 16711935f),
			new PdfParam.ColorNameMap("maroon", 8388608f),
			new PdfParam.ColorNameMap("mediumaquamarine", 6737322f),
			new PdfParam.ColorNameMap("mediumblue", 205f),
			new PdfParam.ColorNameMap("mediumgreen", 64154f),
			new PdfParam.ColorNameMap("mediumorchid", 12211667f),
			new PdfParam.ColorNameMap("mediumpurple", 9662683f),
			new PdfParam.ColorNameMap("mediumseagreen", 3978097f),
			new PdfParam.ColorNameMap("mediumslateblue", 8087790f),
			new PdfParam.ColorNameMap("mediumspringgreen", 64154f),
			new PdfParam.ColorNameMap("mediumturquoise", 4772300f),
			new PdfParam.ColorNameMap("mediumvioletred", 13047173f),
			new PdfParam.ColorNameMap("middle", 2f),
			new PdfParam.ColorNameMap("midnightblue", 1644912f),
			new PdfParam.ColorNameMap("mintcream", 1.612185E+07f),
			new PdfParam.ColorNameMap("mistyrose", 16770273f),
			new PdfParam.ColorNameMap("moccasin", 16770229f),
			new PdfParam.ColorNameMap("movie", 13f),
			new PdfParam.ColorNameMap("named", 9f),
			new PdfParam.ColorNameMap("navajowhite", 16768685f),
			new PdfParam.ColorNameMap("navy", 128f),
			new PdfParam.ColorNameMap("navyblue", 128f),
			new PdfParam.ColorNameMap("oldlace", 16643558f),
			new PdfParam.ColorNameMap("olive", 8421376f),
			new PdfParam.ColorNameMap("olivedrab", 7048739f),
			new PdfParam.ColorNameMap("orange", 1.675392E+07f),
			new PdfParam.ColorNameMap("orangered", 16729344f),
			new PdfParam.ColorNameMap("orchid", 14315734f),
			new PdfParam.ColorNameMap("palegoldenrod", 1.565713E+07f),
			new PdfParam.ColorNameMap("palegreen", 1.002588E+07f),
			new PdfParam.ColorNameMap("paleturquoise", 11529966f),
			new PdfParam.ColorNameMap("palevioletred", 14381203f),
			new PdfParam.ColorNameMap("papayawhip", 16773077f),
			new PdfParam.ColorNameMap("peachpuff", 16767673f),
			new PdfParam.ColorNameMap("peru", 13468991f),
			new PdfParam.ColorNameMap("pink", 16761035f),
			new PdfParam.ColorNameMap("plum", 14524637f),
			new PdfParam.ColorNameMap("powderblue", 1.159191E+07f),
			new PdfParam.ColorNameMap("purple", 8388736f),
			new PdfParam.ColorNameMap("red", 1.671168E+07f),
			new PdfParam.ColorNameMap("reset", 11f),
			new PdfParam.ColorNameMap("right", 1f),
			new PdfParam.ColorNameMap("rosybrown", 12357519f),
			new PdfParam.ColorNameMap("royalblue", 4286945f),
			new PdfParam.ColorNameMap("saddlebrown", 9127187f),
			new PdfParam.ColorNameMap("salmon", 16416882f),
			new PdfParam.ColorNameMap("sandybrown", 16032864f),
			new PdfParam.ColorNameMap("seagreen", 3050327f),
			new PdfParam.ColorNameMap("seashel", 16774638f),
			new PdfParam.ColorNameMap("sienna", 10506797f),
			new PdfParam.ColorNameMap("signature", 3f),
			new PdfParam.ColorNameMap("silver", 12632256f),
			new PdfParam.ColorNameMap("skyblue", 8900331f),
			new PdfParam.ColorNameMap("slateblue", 6970061f),
			new PdfParam.ColorNameMap("slategray", 7372944f),
			new PdfParam.ColorNameMap("slategrey", 7372944f),
			new PdfParam.ColorNameMap("snow", 1.677593E+07f),
			new PdfParam.ColorNameMap("sound", 12f),
			new PdfParam.ColorNameMap("springgreen", 65407f),
			new PdfParam.ColorNameMap("square", 4f),
			new PdfParam.ColorNameMap("squiggly", 9f),
			new PdfParam.ColorNameMap("stamp", 10f),
			new PdfParam.ColorNameMap("steelblue", 4620980f),
			new PdfParam.ColorNameMap("strikeout", 8f),
			new PdfParam.ColorNameMap("submit", 10f),
			new PdfParam.ColorNameMap("tan", 1.380878E+07f),
			new PdfParam.ColorNameMap("tea", 32896f),
			new PdfParam.ColorNameMap("text", 0f),
			new PdfParam.ColorNameMap("thistle", 14204888f),
			new PdfParam.ColorNameMap("tomato", 16737095f),
			new PdfParam.ColorNameMap("top", 0f),
			new PdfParam.ColorNameMap("true", 1f),
			new PdfParam.ColorNameMap("turquoise", 4251856f),
			new PdfParam.ColorNameMap("underline", 7f),
			new PdfParam.ColorNameMap("uri", 5f),
			new PdfParam.ColorNameMap("violet", 15631086f),
			new PdfParam.ColorNameMap("wheat", 16113331f),
			new PdfParam.ColorNameMap("white", 16777215f),
			new PdfParam.ColorNameMap("whitesmoke", 16119285f),
			new PdfParam.ColorNameMap("widget", 14f),
			new PdfParam.ColorNameMap("yellow", 1.677696E+07f),
			new PdfParam.ColorNameMap("yellowgreen", 10145074f)
		};
		internal static string[] NonColorNames = new string[]
		{
			"bottom",
			"button",
			"center",
			"choice",
			"circle",
			"fileattachment",
			"freetext",
			"goto",
			"javascript",
			"launch",
			"left",
			"line",
			"link",
			"middle",
			"movie",
			"named",
			"reset",
			"right",
			"signature",
			"sound",
			"square",
			"squiggly",
			"stamp",
			"strikeout",
			"submit",
			"text",
			"top",
			"true",
			"widget"
		};
		private List<PdfParamItem> m_arrItems;
		private int m_nPtr;
		private int m_nLen;
		private char m_wcNextChar;
		private string m_bstrName;
		private ulong m_nInt;
		private ulong m_nLeadingZeroCount;
		private float m_fNumber;
		private string m_bstrParams;
		public float this[string name]
		{
			get
			{
				foreach (PdfParamItem current in this.m_arrItems)
				{
					if (string.Compare(current.Name, name, true) == 0)
					{
						return current.Value;
					}
				}
				PdfParamItem pdfParamItem = new PdfParamItem();
				pdfParamItem.Name = name;
				this.m_arrItems.Add(pdfParamItem);
				return pdfParamItem.Value;
			}
			set
			{
				foreach (PdfParamItem current in this.m_arrItems)
				{
					if (string.Compare(current.Name, name, true) == 0)
					{
						current.Value = value;
					}
				}
				PdfParamItem pdfParamItem = new PdfParamItem();
				pdfParamItem.Name = name;
				pdfParamItem.Value = value;
				this.m_arrItems.Add(pdfParamItem);
			}
		}
		public int Count
		{
			get
			{
				return this.m_arrItems.Count;
			}
		}
		internal static bool NameToValue(string Name, ref float Value)
		{
			int i = 0;
			int num = PdfParam.ColorNames.Length - 1;
			while (i <= num)
			{
				int num2 = (i + num) / 2;
				int num3 = string.Compare(Name, PdfParam.ColorNames[num2].Name, true);
				if (num3 == 0)
				{
					Value = PdfParam.ColorNames[num2].Value;
					return true;
				}
				if (num3 < 0)
				{
					num = num2 - 1;
				}
				else
				{
					i = num2 + 1;
				}
			}
			return false;
		}
		internal static bool ColorNameToValue(string Name, ref float Value)
		{
			for (int i = 0; i < 29; i++)
			{
				if (string.Compare(Name, PdfParam.NonColorNames[i], true) == 0)
				{
					return false;
				}
			}
			return PdfParam.NameToValue(Name, ref Value);
		}
		internal PdfParam(string ParamString) : this()
		{
			this.Parse(ParamString);
		}
		internal PdfParam()
		{
			this.m_arrItems = new List<PdfParamItem>();
		}
		~PdfParam()
		{
			this.m_arrItems.Clear();
		}
		private void Parse(string str)
		{
			this.m_bstrParams = str;
			this.m_bstrParams += '\0';
			this.m_nPtr = 0;
			this.m_nLen = this.m_bstrParams.Length;
			if (this.m_nLen == 0)
			{
				return;
			}
			this.ReadExpression();
		}
		private void ReadInt()
		{
			this.m_nInt = 0uL;
			this.m_nLeadingZeroCount = 0uL;
			bool flag = true;
			if (!this.IsNumber(this.m_wcNextChar))
			{
				AuxException.Throw("Parameter string parsing error: Invalid character, digit expected.", PdfErrors._ERROR_PARSE);
			}
			while (this.IsNumber(this.m_wcNextChar))
			{
				if (this.m_wcNextChar == '0' && flag)
				{
					this.m_nLeadingZeroCount += 1uL;
				}
				if (this.m_wcNextChar != '0')
				{
					flag = false;
				}
				this.m_nInt = this.m_nInt * 10uL + ((ulong)this.m_wcNextChar - 48uL);
				this.NextChar();
				if (this.m_nInt > 429496729uL)
				{
					AuxException.Throw("Parameter string parsing error: Too many digits.", PdfErrors._ERROR_PARSE);
				}
			}
		}
		private void ReadName()
		{
			this.m_bstrName = "";
			if (!this.IsLetter(this.m_wcNextChar))
			{
				AuxException.Throw("Parameter string parsing error: Invalid character, letter expected.", PdfErrors._ERROR_PARSE);
			}
			while (this.IsLetter(this.m_wcNextChar) || this.IsNumber(this.m_wcNextChar))
			{
				this.m_bstrName += this.m_wcNextChar;
				this.NextChar();
			}
		}
		private void ReadNumber()
		{
			this.m_fNumber = 0f;
			ulong num = 0uL;
			ulong num2 = 0uL;
			ulong num3 = 0uL;
			if (this.m_wcNextChar == '#')
			{
				this.NextChar();
				this.ReadHex();
				return;
			}
			if (this.m_wcNextChar == '&')
			{
				this.NextChar();
				if (this.m_wcNextChar != 'H' && this.m_wcNextChar != 'h')
				{
					AuxException.Throw("Parameter string parsing error: 'H' expected after '&'.", PdfErrors._ERROR_PARSE);
				}
				this.NextChar();
				this.ReadHex();
				return;
			}
			if (this.IsLetter(this.m_wcNextChar))
			{
				this.ReadConst();
				return;
			}
			int num4 = 1;
			if (this.m_wcNextChar == '+' || this.m_wcNextChar == '-')
			{
				if (this.m_wcNextChar == '-')
				{
					num4 = -1;
				}
				this.NextChar();
			}
			if (this.m_wcNextChar == '.')
			{
				this.NextChar();
				this.ReadInt();
				num2 = this.m_nInt;
				num3 = this.m_nLeadingZeroCount;
			}
			else
			{
				this.ReadInt();
				num = this.m_nInt;
				if (this.m_wcNextChar == '.')
				{
					this.NextChar();
					this.ReadInt();
					num2 = this.m_nInt;
					num3 = this.m_nLeadingZeroCount;
				}
			}
			this.m_fNumber = num2;
			while (this.m_fNumber >= 1f)
			{
				this.m_fNumber /= 10f;
			}
			while (num3 > 0uL)
			{
				this.m_fNumber /= 10f;
				num3 -= 1uL;
			}
			this.m_fNumber += num;
			this.m_fNumber *= (float)num4;
		}
		private void ReadExpression()
		{
			this.NextChar();
			while (this.m_wcNextChar != '\0')
			{
				this.ReadName();
				if (this.m_wcNextChar != '=')
				{
					AuxException.Throw("Parameter string parsing error: '=' expected.", PdfErrors._ERROR_PARSE);
				}
				this.NextChar();
				this.ReadNumber();
				this.SetValue();
				if (this.m_wcNextChar != ';' && this.m_wcNextChar != ',')
				{
					break;
				}
				this.NextChar();
			}
			if (this.m_wcNextChar != '\0')
			{
				AuxException.Throw("Parameter string parsing error: Invalid character.", PdfErrors._ERROR_PARSE);
			}
		}
		private void ReadHex()
		{
			if (!this.IsHex(this.m_wcNextChar))
			{
				AuxException.Throw("Parameter string parsing error: Invalid character, hex digit expected.", PdfErrors._ERROR_PARSE);
			}
			int num = 0;
			while (this.IsHex(this.m_wcNextChar))
			{
				if (this.IsNumber(this.m_wcNextChar))
				{
					num = (int)(this.m_wcNextChar - '0');
				}
				else
				{
					if (this.m_wcNextChar >= 'a' && this.m_wcNextChar <= 'f')
					{
						num = (int)('\n' + this.m_wcNextChar - 'a');
					}
					else
					{
						if (this.m_wcNextChar >= 'A' && this.m_wcNextChar <= 'F')
						{
							num = (int)('\n' + this.m_wcNextChar - 'A');
						}
					}
				}
				this.m_fNumber = this.m_fNumber * 16f + (float)num;
				this.NextChar();
				if ((double)this.m_fNumber > 429496729.0)
				{
					AuxException.Throw("Parameter string parsing error: Too many hex digits.", PdfErrors._ERROR_PARSE);
				}
			}
		}
		private void ReadConst()
		{
			string text = "";
			int num = 0;
			while (this.IsLetter(this.m_wcNextChar))
			{
				text += this.m_wcNextChar;
				this.NextChar();
				num++;
				if (num > 40)
				{
					AuxException.Throw("Parameter string parsing error: Constant too long.", PdfErrors._ERROR_PARSE);
				}
			}
			if (!PdfParam.NameToValue(text, ref this.m_fNumber))
			{
				AuxException.Throw("Parse error: Constant not found.", PdfErrors._ERROR_PARSE);
			}
		}
		private bool IsLetter(char wc)
		{
			return (wc >= 'a' && wc <= 'z') || (wc >= 'A' && wc <= 'Z');
		}
		private bool IsNumber(char wc)
		{
			return wc >= '0' && wc <= '9';
		}
		private bool IsHex(char wc)
		{
			return this.IsNumber(wc) || (wc >= 'a' && wc <= 'f') || (wc >= 'A' && wc <= 'F');
		}
		private void NextChar()
		{
			while (this.m_bstrParams[this.m_nPtr] == ' ')
			{
				this.m_nPtr++;
			}
			this.m_wcNextChar = this.m_bstrParams[this.m_nPtr++];
		}
		private void SetValue()
		{
			this.AssignValue(this.m_bstrName, this.m_fNumber);
		}
		private static bool TestOrder()
		{
			for (int i = 0; i < PdfParam.ColorNames.Length - 1; i++)
			{
				if (string.Compare(PdfParam.ColorNames[i].Name, PdfParam.ColorNames[i + 1].Name, true) >= 0)
				{
					return false;
				}
			}
			return true;
		}
		internal void Copy(PdfParam pParam)
		{
			this.Clear();
			foreach (PdfParamItem current in pParam.m_arrItems)
			{
				PdfParamItem pdfParamItem = new PdfParamItem();
				pdfParamItem.Name = current.Name;
				pdfParamItem.Value = current.Value;
				this.m_arrItems.Add(pdfParamItem);
			}
		}
		internal void AddItem(string szName, float fValue)
		{
			PdfParamItem pdfParamItem = new PdfParamItem();
			pdfParamItem.Name = szName;
			pdfParamItem.Value = fValue;
			this.m_arrItems.Add(pdfParamItem);
		}
		internal bool IsTrue(string Name)
		{
			return this.IsSet(Name) && this.Bool(Name);
		}
		internal float Number(string Name)
		{
			return this[Name];
		}
		internal bool Bool(string Name)
		{
			float num = this.Number(Name);
			return num != 0f;
		}
		internal int Long(string Name)
		{
			float num = this.Number(Name);
			return (int)num;
		}
		internal void Color(string Name, out float R, out float G, out float B)
		{
			ulong rgb = (ulong)this.Number(Name);
			B = (float)AuxRGB.GetRValue(rgb) / 255f;
			G = (float)AuxRGB.GetGValue(rgb) / 255f;
			R = (float)AuxRGB.GetBValue(rgb) / 255f;
		}
		internal void Color(string Name, ref AuxRGB rgb)
		{
			float r;
			float g;
			float b;
			this.Color(Name, out r, out g, out b);
			rgb.r = r;
			rgb.g = g;
			rgb.b = b;
			rgb.m_bIsSet = true;
		}
		internal void AssignValue(string Name, float fNumber)
		{
			this[Name] = fNumber;
		}
		internal bool GetNumberIfSet(string Name, int Index, ref float pOut)
		{
			string text = Name.Replace("%d", "{0}");
			text = string.Format(text, Index);
			if (!this.IsSet(text))
			{
				return false;
			}
			pOut = this[text];
			return true;
		}
		internal bool GetNumberIfSet(string Name, int Index, ref double pOut)
		{
			float num = 0f;
			if (!this.GetNumberIfSet(Name, Index, ref num))
			{
				return false;
			}
			pOut = (double)num;
			return true;
		}
		public void Clear()
		{
			this.m_arrItems.Clear();
		}
		public bool IsSet(string Name)
		{
			foreach (PdfParamItem current in this.m_arrItems)
			{
				if (string.Compare(current.Name, Name, true) == 0)
				{
					return true;
				}
			}
			return false;
		}
		public void Add(string ParamString)
		{
			this.Parse(ParamString);
		}
		public void Set(string ParamString)
		{
			this.Clear();
			this.Add(ParamString);
		}
		public void Remove(string Name)
		{
			if (Name.Length == 0)
			{
				return;
			}
			foreach (PdfParamItem current in this.m_arrItems)
			{
				if (string.Compare(current.Name, Name, true) == 0)
				{
					this.m_arrItems.Remove(current);
					break;
				}
			}
		}
	}
}
