using System;
using System.Collections.Generic;
namespace Persits.PDF
{
	internal class AuxSnippetRange
	{
		public int m_nFrom;
		public int m_nLength;
		public int m_nOriginalFrom;
		public int m_nOriginalLength;
		public int m_nBold;
		public int m_nItalic;
		public int m_nUnderlined;
		public int m_nCenter;
		public int m_nRight;
		public int m_nSub;
		public int m_nJustify;
		public string m_bstrAnchor;
		public float m_fR;
		public float m_fG;
		public float m_fB;
		public string m_bstrBaseFont;
		public string m_bstrFace2;
		public PdfFont m_ptrFont;
		public float m_fSize;
		public int m_nHtmlFontSize;
		public List<int> m_arrSkippedChars = new List<int>();
		public AuxSnippetRange(PdfFont piFont, string Face, string Face2, float fSize, float fR, float fG, float fB)
		{
			this.m_nBold = 0;
			this.m_nItalic = 0;
			this.m_nUnderlined = 0;
			this.m_nCenter = 0;
			this.m_nRight = 0;
			this.m_nSub = 0;
			this.m_nJustify = 0;
			this.m_fSize = fSize;
			this.m_ptrFont = piFont;
			this.m_fR = fR;
			this.m_fG = fG;
			this.m_fB = fB;
			this.m_nFrom = (this.m_nLength = (this.m_nOriginalFrom = (this.m_nOriginalLength = 0)));
			this.m_bstrBaseFont = Face;
			this.m_bstrFace2 = Face2;
			this.m_nHtmlFontSize = 3;
		}
		public AuxSnippetRange(AuxSnippetRange obj)
		{
			this.m_nFrom = (this.m_nLength = (this.m_nOriginalFrom = (this.m_nOriginalLength = 0)));
			this.CopyFrom(obj);
		}
		public void CopyFrom(AuxSnippetRange pObj)
		{
			this.m_nBold = pObj.m_nBold;
			this.m_nItalic = pObj.m_nItalic;
			this.m_nUnderlined = pObj.m_nUnderlined;
			this.m_nCenter = pObj.m_nCenter;
			this.m_nRight = pObj.m_nRight;
			this.m_nSub = pObj.m_nSub;
			this.m_nJustify = pObj.m_nJustify;
			this.m_fSize = pObj.m_fSize;
			this.m_ptrFont = pObj.m_ptrFont;
			this.m_fR = pObj.m_fR;
			this.m_fG = pObj.m_fG;
			this.m_fB = pObj.m_fB;
			this.m_bstrBaseFont = pObj.m_bstrBaseFont;
			this.m_nHtmlFontSize = pObj.m_nHtmlFontSize;
			this.m_bstrAnchor = pObj.m_bstrAnchor;
			this.m_bstrFace2 = pObj.m_bstrFace2;
		}
		public bool IsEmpty()
		{
			return this.m_nLength == 0;
		}
	}
}
