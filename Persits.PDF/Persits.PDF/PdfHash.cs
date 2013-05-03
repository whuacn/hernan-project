using System;
namespace Persits.PDF
{
	internal class PdfHash : PdfStream
	{
		private MD5Context m_objContext;
		public PdfHash()
		{
			this.m_objContext = new MD5Context();
		}
		public void Init()
		{
			base.Alloc(16);
			this.m_objContext.MD5Init();
		}
		public void Update(PdfStream pStream)
		{
			this.Update(pStream.m_objMemStream.ToArray());
		}
		public void Compute(PdfStream pStream)
		{
			this.Compute(pStream.m_objMemStream.ToArray());
		}
		public void Update(byte[] pBuffer)
		{
			this.Update(pBuffer, (uint)pBuffer.Length);
		}
		public void Update(byte[] pBuffer, uint nLen)
		{
			this.m_objContext.MD5Update(pBuffer, nLen);
		}
		public void Compute(byte[] pBuffer)
		{
			this.Init();
			this.Update(pBuffer);
			this.Final();
		}
		public void HashItself()
		{
			PdfHash pdfHash = new PdfHash();
			pdfHash.Set(this);
			this.Compute(pdfHash);
		}
		public void Final()
		{
			byte[] array = new byte[16];
			this.m_objContext.MD5Final(array);
			base.Append(array);
		}
	}
}
