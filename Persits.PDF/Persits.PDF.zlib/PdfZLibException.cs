using System;
using System.IO;
namespace Persits.PDF.zlib
{
	internal class PdfZLibException : IOException
	{
		public PdfZLibException()
		{
		}
		public PdfZLibException(string s) : base(s)
		{
		}
	}
}
