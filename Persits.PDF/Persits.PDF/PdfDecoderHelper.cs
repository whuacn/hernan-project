using System;
namespace Persits.PDF
{
	internal abstract class PdfDecoderHelper
	{
		internal const int EOF = -1;
		internal abstract void reset();
		internal abstract int getChar();
		internal void Decode(PdfStream pStream)
		{
			this.reset();
			PdfStream pdfStream = new PdfStream();
			this.CopyTo2(pdfStream);
			pStream.Set(pdfStream);
		}
		private void CopyTo2(PdfStream pTo)
		{
			int @char;
			while ((@char = this.getChar()) != -1)
			{
				pTo.AppendChar((byte)@char);
			}
		}
	}
}
