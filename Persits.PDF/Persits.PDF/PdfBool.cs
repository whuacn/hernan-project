using System;
namespace Persits.PDF
{
	internal class PdfBool : PdfObject
	{
		public bool m_bValue;
		public PdfBool()
		{
			this.m_nType = enumType.pdfBool;
			this.m_bValue = false;
		}
		public PdfBool(PdfBool obj)
		{
			base.CopyType(obj);
			this.m_bValue = obj.m_bValue;
		}
		public PdfBool(string Name, bool Value)
		{
			this.m_nType = enumType.pdfBool;
			this.m_bstrType = Name;
			this.m_bValue = Value;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int result = 0;
			base.WriteOutName(pOutput, ref result);
			pOutput.Write(this.m_bValue ? "true" : "false", ref result);
			return result;
		}
		public override PdfObject Copy()
		{
			return new PdfBool(this.m_bstrType, this.m_bValue);
		}
	}
}
