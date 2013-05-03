using System;
namespace Persits.PDF
{
	internal class PdfOther : PdfObject
	{
		internal string m_bstrValue;
		internal string m_bstrOriginalValue;
		public PdfOther(string Type, string Value, string OriginalValue)
		{
			this.m_bstrType = Type;
			this.m_nType = enumType.pdfOther;
			this.m_bstrValue = Value;
			this.m_bstrOriginalValue = OriginalValue;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			return 0;
		}
		public override PdfObject Copy()
		{
			return new PdfOther(this.m_bstrType, this.m_bstrValue, this.m_bstrOriginalValue);
		}
	}
}
