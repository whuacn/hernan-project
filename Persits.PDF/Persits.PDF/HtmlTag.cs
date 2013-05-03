using System;
using System.Collections.Generic;
using System.Text;
namespace Persits.PDF
{
	internal class HtmlTag : HtmlObject
	{
		internal static TagStruct[] m_arrTags = new TagStruct[]
		{
			new TagStruct(TagType.TAG_A, "A"),
			new TagStruct(TagType.TAG_ADDRESS, "ADDRESS"),
			new TagStruct(TagType.TAG_B, "B"),
			new TagStruct(TagType.TAG_BASE, "BASE"),
			new TagStruct(TagType.TAG_BASEFONT, "BASEFONT"),
			new TagStruct(TagType.TAG_BIG, "BIG"),
			new TagStruct(TagType.TAG_BQ, "BLOCKQUOTE"),
			new TagStruct(TagType.TAG_BODY, "BODY"),
			new TagStruct(TagType.TAG_BR, "BR"),
			new TagStruct(TagType.TAG_CAPTION, "CAPTION"),
			new TagStruct(TagType.TAG_CENTER, "CENTER"),
			new TagStruct(TagType.TAG_CITE, "CITE"),
			new TagStruct(TagType.TAG_CODE, "CODE"),
			new TagStruct(TagType.TAG_DD, "DD"),
			new TagStruct(TagType.TAG_DEL, "DEL"),
			new TagStruct(TagType.TAG_DFN, "DFN"),
			new TagStruct(TagType.TAG_UL, "DIR"),
			new TagStruct(TagType.TAG_DIV, "DIV"),
			new TagStruct(TagType.TAG_DL, "DL"),
			new TagStruct(TagType.TAG_DT, "DT"),
			new TagStruct(TagType.TAG_EM, "EM"),
			new TagStruct(TagType.TAG_FONT, "FONT"),
			new TagStruct(TagType.TAG_FORM, "FORM"),
			new TagStruct(TagType.TAG_H1, "H1"),
			new TagStruct(TagType.TAG_H2, "H2"),
			new TagStruct(TagType.TAG_H3, "H3"),
			new TagStruct(TagType.TAG_H4, "H4"),
			new TagStruct(TagType.TAG_H5, "H5"),
			new TagStruct(TagType.TAG_H6, "H6"),
			new TagStruct(TagType.TAG_HR, "HR"),
			new TagStruct(TagType.TAG_HTML, "HTML"),
			new TagStruct(TagType.TAG_I, "I"),
			new TagStruct(TagType.TAG_IMG, "IMG"),
			new TagStruct(TagType.TAG_INPUT, "INPUT"),
			new TagStruct(TagType.TAG_INS, "INS"),
			new TagStruct(TagType.TAG_KBD, "KBD"),
			new TagStruct(TagType.TAG_LI, "LI"),
			new TagStruct(TagType.TAG_LINK, "LINK"),
			new TagStruct(TagType.TAG_UL, "MENU"),
			new TagStruct(TagType.TAG_META, "META"),
			new TagStruct(TagType.TAG_OL, "OL"),
			new TagStruct(TagType.TAG_OPTION, "OPTION"),
			new TagStruct(TagType.TAG_P, "P"),
			new TagStruct(TagType.TAG_S, "S"),
			new TagStruct(TagType.TAG_SAMP, "SAMP"),
			new TagStruct(TagType.TAG_SCRIPT, "SCRIPT"),
			new TagStruct(TagType.TAG_SELECT, "SELECT"),
			new TagStruct(TagType.TAG_SMALL, "SMALL"),
			new TagStruct(TagType.TAG_SPAN, "SPAN"),
			new TagStruct(TagType.TAG_S, "STRIKE"),
			new TagStruct(TagType.TAG_STRONG, "STRONG"),
			new TagStruct(TagType.TAG_STYLE, "STYLE"),
			new TagStruct(TagType.TAG_SUB, "SUB"),
			new TagStruct(TagType.TAG_SUP, "SUP"),
			new TagStruct(TagType.TAG_TABLE, "TABLE"),
			new TagStruct(TagType.TAG_TBODY, "TBODY"),
			new TagStruct(TagType.TAG_TD, "TD"),
			new TagStruct(TagType.TAG_TEXTAREA, "TEXTAREA"),
			new TagStruct(TagType.TAG_TFOOT, "TFOOT"),
			new TagStruct(TagType.TAG_TH, "TH"),
			new TagStruct(TagType.TAG_THEAD, "THEAD"),
			new TagStruct(TagType.TAG_TITLE, "TITLE"),
			new TagStruct(TagType.TAG_TR, "TR"),
			new TagStruct(TagType.TAG_TT, "TT"),
			new TagStruct(TagType.TAG_U, "U"),
			new TagStruct(TagType.TAG_UL, "UL"),
			new TagStruct(TagType.TAG_VAR, "VAR")
		};
		private static TagStruct[] m_arrAttributes = new TagStruct[]
		{
			new TagStruct(TagType.TAG_NOTATAG, "left"),
			new TagStruct(TagType.TAG_UNKNOWN, "right"),
			new TagStruct(TagType.TAG_B, "top"),
			new TagStruct(TagType.TAG_I, "bottom"),
			new TagStruct(TagType.TAG_CODE, "middle"),
			new TagStruct(TagType.TAG_KBD, "both"),
			new TagStruct(TagType.TAG_SAMP, "none"),
			new TagStruct(TagType.TAG_CITE, "disc"),
			new TagStruct(TagType.TAG_DFN, "circle"),
			new TagStruct(TagType.TAG_EM, "square"),
			new TagStruct(TagType.TAG_VAR, "true"),
			new TagStruct(TagType.TAG_STRONG, "false"),
			new TagStruct(TagType.TAG_SMALL, "center"),
			new TagStruct(TagType.TAG_BIG, "justify")
		};
		private static TagFlagStruct[] m_arrTagFlags = new TagFlagStruct[]
		{
			new TagFlagStruct(TagType.TAG_UL, 1u),
			new TagFlagStruct(TagType.TAG_OL, 2u),
			new TagFlagStruct(TagType.TAG_LI, 4u),
			new TagFlagStruct(TagType.TAG_BQ, 8u),
			new TagFlagStruct(TagType.TAG_DIV, 16u),
			new TagFlagStruct(TagType.TAG_TABLE, 32u),
			new TagFlagStruct(TagType.TAG_CENTER, 64u),
			new TagFlagStruct(TagType.TAG_ADDRESS, 128u),
			new TagFlagStruct(TagType.TAG_P, 256u),
			new TagFlagStruct(TagType.TAG_H1, 512u),
			new TagFlagStruct(TagType.TAG_H2, 512u),
			new TagFlagStruct(TagType.TAG_H3, 512u),
			new TagFlagStruct(TagType.TAG_H4, 512u),
			new TagFlagStruct(TagType.TAG_H5, 512u),
			new TagFlagStruct(TagType.TAG_H6, 512u),
			new TagFlagStruct(TagType.TAG_TD, 1024u),
			new TagFlagStruct(TagType.TAG_TH, 2048u),
			new TagFlagStruct(TagType.TAG_TR, 4096u),
			new TagFlagStruct(TagType.TAG_BODY, 8192u),
			new TagFlagStruct(TagType.TAG_FORM, 16384u),
			new TagFlagStruct(TagType.TAG_HR, 32768u),
			new TagFlagStruct(TagType.TAG_CAPTION, 65536u),
			new TagFlagStruct(TagType.TAG_IMG, 131072u),
			new TagFlagStruct(TagType.TAG_DD, 16u),
			new TagFlagStruct(TagType.TAG_DT, 16u),
			new TagFlagStruct(TagType.TAG_DL, 16u)
		};
		public bool m_bClosing;
		public uint m_dwTagFlag;
		public TagType m_nType;
		public List<string> m_arrNames = new List<string>();
		public List<string> m_arrValues = new List<string>();
		public string m_bstrAttributes;
		public int m_nAttributeLen;
		public int m_nAttrPtr;
		public string m_bstrName;
		private string m_bstrAttrName;
		private string m_bstrAttrValue;
		public override ObjectType Type
		{
			get
			{
				return ObjectType.htmlTag;
			}
		}
		public HtmlTag(HtmlManager pMgr) : base(pMgr)
		{
			this.m_bClosing = false;
			this.m_dwTagFlag = 0u;
		}
		public override void CalculateSize()
		{
		}
		public override void AssignCoordinates()
		{
		}
		private void Clear()
		{
			this.m_nType = TagType.TAG_UNKNOWN;
			this.m_bClosing = false;
			this.m_dwTagFlag = 0u;
			this.m_arrNames.Clear();
			this.m_arrValues.Clear();
		}
		public override void Parse()
		{
			this.m_nType = TagType.TAG_UNKNOWN;
			this.m_dwTagFlag = 0u;
			int nPtr = base.m_nPtr;
			base.Next();
			if (base.m_c == '!' || base.m_c == '?')
			{
				do
				{
					base.Next();
				}
				while (base.m_c != '>');
				base.Next();
				this.m_nType = TagType.TAG_COMMENT;
				return;
			}
			this.m_bClosing = false;
			if (base.m_c == '/')
			{
				base.Next();
				this.m_bClosing = true;
			}
			if (!HtmlObject.IsLetter(base.m_c))
			{
				base.m_nPtr = nPtr;
				base.m_c = '<';
				this.m_nType = TagType.TAG_NOTATAG;
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			char c = base.m_c;
			while ((base.m_c != '>' && base.m_c != '<') || flag)
			{
				if (!flag && base.m_c == '"' && c == '=')
				{
					flag = true;
				}
				else
				{
					if (flag && base.m_c == '"')
					{
						flag = false;
					}
				}
				stringBuilder.Append(base.m_c);
				if (!HtmlObject.IsSpace(base.m_c))
				{
					c = base.m_c;
				}
				base.Next();
			}
			this.ParseAttributes(stringBuilder.ToString());
			if (base.m_c == '>')
			{
				if (this.m_nType == TagType.TAG_STYLE)
				{
					base.m_c = this.m_pHtmlManager.m_pBuffer[this.m_pHtmlManager.m_nPtr++];
				}
				else
				{
					base.Next();
				}
			}
			if (this.m_nType == TagType.TAG_NOTATAG)
			{
				base.m_nPtr = nPtr;
				base.m_c = '<';
			}
		}
		private void ParseAttributes(string bstrTag)
		{
			int length = bstrTag.Length;
			bool flag = false;
			int i;
			for (i = 0; i < length; i++)
			{
				if (HtmlObject.IsSpace(bstrTag[i]) || bstrTag[i] == '/')
				{
					flag = true;
					break;
				}
			}
			string text;
			if (flag)
			{
				text = bstrTag.Substring(0, i);
				this.m_nAttrPtr = i + 1;
			}
			else
			{
				text = bstrTag;
			}
			int j = 0;
			int num = HtmlTag.m_arrTags.Length - 1;
			while (j <= num)
			{
				int num2 = (j + num) / 2;
				int num3 = string.Compare(text, HtmlTag.m_arrTags[num2].m_szName, true);
				if (num3 == 0)
				{
					this.m_nType = HtmlTag.m_arrTags[num2].m_nType;
					this.m_bstrName = text;
					break;
				}
				if (num3 > 0)
				{
					j = num2 + 1;
				}
				else
				{
					num = num2 - 1;
				}
			}
			this.LookupFlag();
			if (this.m_nType == TagType.TAG_UNKNOWN || !flag)
			{
				return;
			}
			this.m_bstrAttributes = bstrTag;
			this.m_nAttributeLen = length;
			while (this.SkipSpaces())
			{
				this.ReadName();
				this.m_arrNames.Add(this.m_bstrAttrName);
				if (!this.SkipSpaces())
				{
					this.m_arrValues.Add("");
					return;
				}
				if (this.m_bstrAttributes[this.m_nAttrPtr] != '=')
				{
					this.m_arrValues.Add("");
				}
				else
				{
					this.m_nAttrPtr++;
					if (!this.SkipSpaces())
					{
						this.m_arrValues.Add("");
						return;
					}
					this.ReadValue();
					this.m_arrValues.Add(this.m_bstrAttrValue);
				}
			}
		}
		public void LookupFlag()
		{
			for (int i = 0; i < HtmlTag.m_arrTagFlags.Length; i++)
			{
				if (this.m_nType == HtmlTag.m_arrTagFlags[i].m_nType)
				{
					this.m_dwTagFlag = HtmlTag.m_arrTagFlags[i].m_dwFlag;
					return;
				}
			}
		}
		private bool SkipSpaces()
		{
			while (this.m_nAttrPtr < this.m_nAttributeLen && HtmlObject.IsSpace(this.m_bstrAttributes[this.m_nAttrPtr]))
			{
				this.m_nAttrPtr++;
			}
			return this.m_nAttrPtr < this.m_nAttributeLen;
		}
		private bool ReadName()
		{
			this.m_bstrAttrName = "";
			while (HtmlObject.IsLetter(this.m_bstrAttributes[this.m_nAttrPtr]) || this.m_bstrAttributes[this.m_nAttrPtr] == '!')
			{
				this.m_bstrAttrName += this.m_bstrAttributes[this.m_nAttrPtr];
				this.m_nAttrPtr++;
				if (this.m_nAttrPtr >= this.m_nAttributeLen)
				{
					return false;
				}
			}
			if (this.m_bstrAttrName.Length == 0)
			{
				while (!HtmlObject.IsLetter(this.m_bstrAttributes[this.m_nAttrPtr]))
				{
					this.m_nAttrPtr++;
					if (this.m_nAttrPtr >= this.m_nAttributeLen)
					{
						return false;
					}
				}
				this.ReadName();
			}
			return true;
		}
		private bool ReadValue()
		{
			this.m_bstrAttrValue = "";
			char c = '\0';
			if (this.m_bstrAttributes[this.m_nAttrPtr] == '"' || this.m_bstrAttributes[this.m_nAttrPtr] == '\'')
			{
				c = this.m_bstrAttributes[this.m_nAttrPtr];
				this.m_nAttrPtr++;
				if (this.m_nAttrPtr >= this.m_nAttributeLen)
				{
					this.m_nType = TagType.TAG_NOTATAG;
					return false;
				}
			}
			if (c != '\0')
			{
				while (this.m_nAttrPtr < this.m_nAttributeLen && this.m_bstrAttributes[this.m_nAttrPtr] != c)
				{
					this.m_bstrAttrValue += this.m_bstrAttributes[this.m_nAttrPtr];
					this.m_nAttrPtr++;
				}
				if (this.m_nAttrPtr < this.m_bstrAttributes.Length && this.m_bstrAttributes[this.m_nAttrPtr] == c)
				{
					this.m_nAttrPtr++;
				}
				else
				{
					this.m_nType = TagType.TAG_NOTATAG;
				}
				HtmlObject.Trim(ref this.m_bstrAttrValue);
				return this.m_nAttrPtr < this.m_nAttributeLen;
			}
			while (this.m_nAttrPtr < this.m_nAttributeLen && !HtmlObject.IsSpace(this.m_bstrAttributes[this.m_nAttrPtr]))
			{
				this.m_bstrAttrValue += this.m_bstrAttributes[this.m_nAttrPtr];
				this.m_nAttrPtr++;
			}
			HtmlObject.Trim(ref this.m_bstrAttrValue);
			return this.m_nAttrPtr < this.m_nAttributeLen;
		}
		public bool GetStandardAttribute(string Name, ref HtmlAttributes pRes)
		{
			if (Name == null || Name.Length == 0)
			{
				return false;
			}
			string strA = null;
			if (!this.GetStringAttribute(Name, ref strA))
			{
				return false;
			}
			for (int i = 0; i < HtmlTag.m_arrAttributes.Length; i++)
			{
				if (string.Compare(strA, HtmlTag.m_arrAttributes[i].m_szName, true) == 0)
				{
					pRes = (HtmlAttributes)HtmlTag.m_arrAttributes[i].m_nType;
					return true;
				}
			}
			return false;
		}
		public bool GetColorAttribute(string Name, ref AuxRGB pRes)
		{
			if (Name == null || Name.Length == 0)
			{
				return false;
			}
			string text = null;
			if (!this.GetStringAttribute(Name, ref text))
			{
				return false;
			}
			float num = 0f;
			if (!PdfParam.ColorNameToValue(text, ref num))
			{
				this.ParseHexColor(text, ref num);
			}
			pRes.Set((uint)num);
			return true;
		}
		private void ParseHexColor(string Value, ref float pValue)
		{
			float num = 0f;
			int i = 0;
			if (Value[0] == '#')
			{
				i = 1;
			}
			while (i < Value.Length)
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
		public bool GetStringAttribute(string Name)
		{
			string text = "";
			return this.GetStringAttribute(Name, ref text);
		}
		public bool GetStringAttribute(string Name, ref string pVal)
		{
			if (Name == null || Name.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.m_arrNames.Count; i++)
			{
				if (this.m_arrNames[i] != null && this.m_arrNames[i].Length != 0 && string.Compare(this.m_arrNames[i], Name, true) == 0)
				{
					pVal = this.m_arrValues[i];
					return true;
				}
			}
			return false;
		}
		public bool GetIntAttribute(string Name)
		{
			int num = 0;
			return this.GetIntAttribute(Name, ref num);
		}
		public bool GetIntAttribute(string Name, ref int pVal)
		{
			char c = ' ';
			bool flag = true;
			return this.GetIntAttribute(Name, ref pVal, ref c, ref flag);
		}
		public bool GetIntAttribute(string Name, ref int pVal, ref char pSign, ref bool pPercent)
		{
			string text = null;
			if (!this.GetStringAttribute(Name, ref text))
			{
				return false;
			}
			int length = text.Length;
			if (length == 0)
			{
				return true;
			}
			if (length > 1 && (text[0] == '+' || text[0] == '-'))
			{
				pSign = text[0];
				pVal = HtmlTag.IntParse(text.Substring(1));
			}
			else
			{
				pSign = '\0';
				pVal = HtmlTag.IntParse(text);
			}
			bool flag = false;
			if (text[text.Length - 1] == '%')
			{
				flag = true;
				pVal = HtmlTag.IntParse(text.Substring(0, text.Length - 1));
			}
			pPercent = flag;
			return true;
		}
		public static int IntParse(string s)
		{
			int num = 0;
			while (num < s.Length && s[num] >= '0' && s[num] <= '9')
			{
				num++;
			}
			if (num == 0)
			{
				return 0;
			}
			int result;
			try
			{
				result = int.Parse(s.Substring(0, num));
			}
			catch (Exception)
			{
				result = 0;
			}
			return result;
		}
		public void CopyTo(ref HtmlTag pDestTag)
		{
			pDestTag.Clear();
			pDestTag.m_bstrName = this.m_bstrName;
			pDestTag.m_nType = this.m_nType;
			pDestTag.m_bClosing = this.m_bClosing;
			pDestTag.m_dwTagFlag = this.m_dwTagFlag;
			for (int i = 0; i < this.m_arrNames.Count; i++)
			{
				pDestTag.m_arrNames.Add(this.m_arrNames[i]);
				pDestTag.m_arrValues.Add(this.m_arrValues[i]);
			}
		}
		public bool IsOneOf(uint dwFlags)
		{
			return (dwFlags & this.m_dwTagFlag) > 0u;
		}
		public static string LookupTag(TagType Tag)
		{
			for (int i = 0; i < HtmlTag.m_arrTags.Length; i++)
			{
				if (Tag == HtmlTag.m_arrTags[i].m_nType)
				{
					return HtmlTag.m_arrTags[i].m_szName;
				}
			}
			return "?";
		}
	}
}
