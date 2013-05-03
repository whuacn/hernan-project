using System;
namespace Persits.PDF
{
	internal class PdfRef : PdfObject
	{
		public int m_nObjNum;
		public int m_nGenNum;
		public PdfRef()
		{
			this.m_nType = enumType.pdfRef;
			this.m_nObjNum = 0;
			this.m_nGenNum = 0;
		}
		public PdfRef(PdfRef obj)
		{
			base.CopyType(obj);
			this.m_nObjNum = obj.m_nObjNum;
			this.m_nGenNum = obj.m_nGenNum;
		}
		public PdfRef(string Type, int ObjNum, int GenNum)
		{
			this.m_nType = enumType.pdfRef;
			this.m_bstrType = Type;
			this.m_nObjNum = ObjNum;
			this.m_nGenNum = GenNum;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			base.WriteOutName(pOutput, ref result);
			string objString = string.Format("{0} {1} R", this.m_nObjNum, this.m_nGenNum);
			pOutput.Write(objString, ref result);
			return result;
		}
		public override PdfObject Copy()
		{
			return new PdfRef(this.m_bstrType, this.m_nObjNum, this.m_nGenNum);
		}
	}
}
