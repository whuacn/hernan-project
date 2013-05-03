using System;
using System.Text;
namespace Persits.PDF
{
	internal class PdfName : PdfObject
	{
		public string m_bstrName;
		public PdfName(PdfName obj)
		{
			base.CopyType(obj);
			this.m_bstrName = obj.m_bstrName;
		}
		public PdfName(string Type, string Name)
		{
			this.m_bstrType = Type;
			this.m_bstrName = Name;
			this.m_nType = enumType.pdfName;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			base.WriteOutName(pOutput, ref result);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("/");
			string bstrName = this.m_bstrName;
			for (int i = 0; i < bstrName.Length; i++)
			{
				char c = bstrName[i];
				if (c >= '!' && c <= '~' && c != '(' && c != ')' && c != '<' && c != '>' && c != '[' && c != ']' && c != '/' && c != '%' && c != '{' && c != '#' && c != '}')
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append(string.Format("#{0:x02}", (int)c));
				}
			}
			pOutput.Write(stringBuilder.ToString(), ref result);
			return result;
		}
		public override PdfObject Copy()
		{
			return new PdfName(this.m_bstrType, this.m_bstrName);
		}
	}
}
