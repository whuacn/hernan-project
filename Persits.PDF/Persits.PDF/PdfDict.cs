using System;
namespace Persits.PDF
{
	internal class PdfDict : PdfObjectList
	{
		public PdfDict() : this(false)
		{
		}
		public PdfDict(bool bTerminate)
		{
			this.m_nType = enumType.pdfDictionary;
			this.m_bTerminate = bTerminate;
			this.m_pParent = null;
		}
		public PdfDict(string Type)
		{
			this.m_bstrType = Type;
			this.m_nType = enumType.pdfDictionary;
			this.m_bTerminate = false;
			this.m_pParent = null;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			if (this.m_arrItems.Count <= 0 && this.m_bstrType == null)
			{
				return 0;
			}
			int result = 0;
			base.WriteOutName(pOutput, ref result);
			pOutput.Write("<<", ref result);
			foreach (PdfObject current in this.m_arrItems)
			{
				current.WriteOut(pOutput);
				pOutput.Write("\n", ref result);
			}
			pOutput.Write(">>", ref result);
			if (this.m_bTerminate)
			{
				pOutput.Write("\n", ref result);
			}
			return result;
		}
		public override PdfObject Copy()
		{
			PdfDict pdfDict = new PdfDict(this.m_bstrType);
			pdfDict.CopyItems(this);
			return pdfDict;
		}
		public void AddRGBArray(string Name, AuxRGB color)
		{
			PdfArray pdfArray = new PdfArray(Name);
			pdfArray.Add(new PdfNumber(null, (double)color.r));
			pdfArray.Add(new PdfNumber(null, (double)color.g));
			pdfArray.Add(new PdfNumber(null, (double)color.b));
			base.Add(pdfArray);
		}
	}
}
