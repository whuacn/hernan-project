using System;
namespace Persits.PDF
{
	internal class PdfXRefEntry : PdfObject
	{
		public int m_nObjNumber;
		public int m_nGenNumber;
		public int m_nOffset;
		public byte m_cType;
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			string objString = string.Format("{0:d10} {1:d5} {2} \n", this.m_nOffset, this.m_nGenNumber, (char)this.m_cType);
			pOutput.Write(objString, ref result);
			return result;
		}
		public override PdfObject Copy()
		{
			return null;
		}
	}
}
