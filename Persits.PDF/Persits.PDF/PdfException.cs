using System;
namespace Persits.PDF
{
	internal class PdfException : SystemException
	{
		public float m_fWidth;
		public int m_nCode;
		public PdfException(string err) : base(err)
		{
		}
		public PdfException(string err, int nCode) : base(err)
		{
			this.m_nCode = nCode;
		}
		public PdfException(string err, float fWidth) : base(err)
		{
			this.m_fWidth = fWidth;
		}
		public PdfException(PdfErrors err, float fWidth)
		{
			this.m_nCode = (int)err;
			this.m_fWidth = fWidth;
		}
	}
}
