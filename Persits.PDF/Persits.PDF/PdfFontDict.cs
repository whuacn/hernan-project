using System;
namespace Persits.PDF
{
	internal class PdfFontDict
	{
		private PdfPreviewFont[] fonts;
		private int numFonts;
		private PdfPreview m_pPreview;
		public PdfFontDict(PdfDict fontDict, PdfPreview pPreview)
		{
			this.m_pPreview = pPreview;
			this.numFonts = fontDict.Size;
			this.fonts = new PdfPreviewFont[this.numFonts];
			for (int i = 0; i < this.numFonts; i++)
			{
				PdfObject @object = fontDict.GetObject(i);
				if (@object != null && @object.m_nType == enumType.pdfDictionary)
				{
					this.fonts[i] = PdfPreviewFont.makeFont(@object.m_bstrType, @object.m_ref, (PdfDict)@object, pPreview);
					if (this.fonts[i] != null && !this.fonts[i].isOk)
					{
						this.fonts[i] = null;
					}
				}
				else
				{
					this.fonts[i] = null;
				}
			}
		}
		~PdfFontDict()
		{
		}
		public PdfPreviewFont lookup(string tag)
		{
			for (int i = 0; i < this.numFonts; i++)
			{
				if (this.fonts[i] != null && this.fonts[i].matches(tag))
				{
					return this.fonts[i];
				}
			}
			return null;
		}
		public int getNumFonts()
		{
			return this.numFonts;
		}
		public PdfPreviewFont getFont(int i)
		{
			return this.fonts[i];
		}
	}
}
