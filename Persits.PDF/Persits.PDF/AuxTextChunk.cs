using System;
namespace Persits.PDF
{
	internal class AuxTextChunk
	{
		public PdfFont m_pFont;
		public PdfParam m_pParam;
		public string m_bstrText;
		public int m_nVAlignment;
		public float m_fVerticalExtent;
		public AuxTextChunk()
		{
			this.m_nVAlignment = 0;
			this.m_fVerticalExtent = 0f;
		}
	}
}
