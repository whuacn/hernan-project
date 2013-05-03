using System;
namespace Persits.PDF
{
	internal abstract class PdfObject
	{
		internal enumType m_nType;
		public string m_bstrType;
		public _Point m_ref = new _Point(0, 0);
		public abstract int WriteOut(PdfOutput pOutput);
		public void CopyType(PdfObject obj)
		{
			this.m_nType = obj.m_nType;
			this.m_bstrType = obj.m_bstrType;
			this.m_ref = obj.m_ref;
		}
		public int WriteOutName(PdfOutput pOutput, ref int pSize)
		{
			if (this.m_bstrType != null)
			{
				PdfName pdfName = new PdfName(null, this.m_bstrType);
				pdfName.WriteOut(pOutput);
				pOutput.Write(" ", ref pSize);
			}
			return 0;
		}
		public abstract PdfObject Copy();
		~PdfObject()
		{
		}
	}
}
