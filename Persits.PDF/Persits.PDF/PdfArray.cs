using System;
namespace Persits.PDF
{
	internal class PdfArray : PdfObjectList
	{
		public int m_nItemsPerLine;
		public PdfArray(string Type)
		{
			this.m_bstrType = Type;
			this.m_nType = enumType.pdfArray;
			this.m_nItemsPerLine = 0;
			this.m_pParent = null;
			this.m_bTerminate = false;
		}
		public PdfArray(string Type, PdfObject[] pPtr)
		{
			this.m_nType = enumType.pdfArray;
			this.m_bstrType = Type;
			this.m_nItemsPerLine = 0;
			this.m_pParent = null;
			this.m_bTerminate = false;
			for (int i = 0; i < pPtr.Length; i++)
			{
				PdfObject ptr = pPtr[i];
				base.Add(new PdfReference(null, ptr));
			}
		}
		private PdfArray(string Type, int[] pValues)
		{
			this.m_bstrType = Type;
			this.m_nType = enumType.pdfArray;
			this.m_nItemsPerLine = 0;
			this.m_pParent = null;
			this.m_bTerminate = false;
			for (int i = 0; i < pValues.Length; i++)
			{
				int num = pValues[i];
				base.Add(new PdfNumber(null, (double)num));
			}
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			int num = 0;
			int num2 = 0;
			bool flag = true;
			base.WriteOutName(pOutput, ref num);
			pOutput.Write("[", ref num);
			foreach (PdfObject current in this.m_arrItems)
			{
				if (!flag)
				{
					pOutput.Write(" ", ref num);
				}
				num += current.WriteOut(pOutput);
				num2++;
				if (this.m_nItemsPerLine > 0 && num2 == this.m_nItemsPerLine)
				{
					pOutput.Write("\n", ref num);
					num2 = 0;
				}
				flag = false;
			}
			pOutput.Write("]", ref num);
			if (this.m_bTerminate)
			{
				pOutput.Write("\n", ref num);
			}
			return num;
		}
		public override PdfObject Copy()
		{
			PdfArray pdfArray = new PdfArray(this.m_bstrType);
			pdfArray.CopyItems(this);
			return pdfArray;
		}
	}
}
