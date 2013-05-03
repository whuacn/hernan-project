using System;
using System.Text;
namespace Persits.PDF
{
	internal class PdfBigEndianString : PdfString
	{
		public PdfBigEndianString(string Name, string Value)
		{
			this.m_bstrType = Name;
			this.m_nType = enumType.pdfString;
			if (Value == null)
			{
				return;
			}
			UnicodeEncoding unicodeEncoding = new UnicodeEncoding(true, true);
			base.Set(unicodeEncoding.GetPreamble());
			base.Append(unicodeEncoding.GetBytes(Value));
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			base.WriteOutName(pOutput, ref result);
			pOutput.Write("(", ref result);
			PdfStream pdfStream = new PdfStream();
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
			pdfStream.EscapeSymbols();
			pOutput.Write(pdfStream, ref result);
			pOutput.Write(")", ref result);
			return result;
		}
	}
}
