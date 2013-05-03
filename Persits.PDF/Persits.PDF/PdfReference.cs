using System;
namespace Persits.PDF
{
	internal class PdfReference : PdfObject
	{
		public PdfObject m_pPtr;
		public PdfReference()
		{
			this.m_nType = enumType.pdfReference;
			this.m_pPtr = null;
		}
		public PdfReference(PdfReference obj)
		{
			base.CopyType(obj);
			this.m_pPtr = obj.m_pPtr;
		}
		public PdfReference(string Type, PdfObject Ptr)
		{
			this.m_nType = enumType.pdfReference;
			this.m_bstrType = Type;
			this.m_pPtr = Ptr;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			if (this.m_pPtr == null)
			{
				return 0;
			}
			PdfIndirectObj pdfIndirectObj = (PdfIndirectObj)this.m_pPtr;
			base.WriteOutName(pOutput, ref result);
			string objString = string.Format("{0} {1} R", pdfIndirectObj.m_nObjNumber, pdfIndirectObj.m_nGenNumber);
			pOutput.Write(objString, ref result);
			return result;
		}
		public override PdfObject Copy()
		{
			return new PdfReference(this.m_bstrType, this.m_pPtr);
		}
	}
}
