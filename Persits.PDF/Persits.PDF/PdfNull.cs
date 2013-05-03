using System;
namespace Persits.PDF
{
	internal class PdfNull : PdfObject
	{
		public PdfNull(string Name)
		{
			this.m_nType = enumType.pdfNull;
			this.m_bstrType = Name;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			base.WriteOutName(pOutput, ref result);
			pOutput.Write("null", ref result);
			return result;
		}
		public override PdfObject Copy()
		{
			return new PdfNull(this.m_bstrType);
		}
	}
}
