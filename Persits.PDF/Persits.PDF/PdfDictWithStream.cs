using System;
namespace Persits.PDF
{
	internal class PdfDictWithStream : PdfDict
	{
		public PdfStream m_objStream = new PdfStream();
		public PdfDictWithStream(string sType) : base(sType)
		{
			this.m_nType = enumType.pdfDictWithStream;
		}
		public override int WriteOut(PdfOutput pOutput)
		{
			return base.WriteOut(pOutput);
		}
		public override PdfObject Copy()
		{
			PdfDictWithStream pdfDictWithStream = new PdfDictWithStream(this.m_bstrType);
			pdfDictWithStream.CopyItems(this);
			pdfDictWithStream.m_objStream.Set(this.m_objStream);
			return pdfDictWithStream;
		}
		public PdfDict CopyDict()
		{
			PdfDict pdfDict = (PdfDict)base.Copy();
			pdfDict.m_nType = enumType.pdfDictionary;
			return pdfDict;
		}
		internal void reset()
		{
			this.m_objStream.reset();
		}
		internal int getChar()
		{
			return this.m_objStream.getChar();
		}
		internal int lookChar()
		{
			return this.m_objStream.lookChar();
		}
	}
}
