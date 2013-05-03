using System;
using System.IO;
namespace Persits.PDF
{
	internal class PdfOutputToFile : PdfOutput
	{
		internal FileStream m_objFileStream;
		internal string m_bstrPath;
		internal int m_nTotalSizeWritten;
		public PdfOutputToFile(int InitialSize, string Path) : base(InitialSize)
		{
			this.m_bstrPath = Path;
			this.m_nTotalSizeWritten = 0;
		}
		~PdfOutputToFile()
		{
			if (this.m_objFileStream != null)
			{
				this.m_objFileStream.Close();
				this.m_objFileStream = null;
			}
		}
		public override void Write(byte[] objString, ref int NewSize)
		{
			this.Write(objString, 0, objString.Length, ref NewSize);
		}
		public override void Write(byte[] objString, int nStart, int nCount, ref int NewSize)
		{
			try
			{
				this.m_objFileStream.Write(objString, nStart, nCount);
			}
			catch (Exception ex)
			{
				AuxException.Throw("Writing to output file failed: " + ex.Message, PdfErrors._ERROR_DUMPDOCUMENT);
			}
			this.m_nTotalSizeWritten += nCount;
			NewSize += nCount;
		}
		public override void Flush()
		{
			try
			{
				this.m_objFileStream.Flush();
			}
			catch (Exception ex)
			{
				AuxException.Throw("Flushing to output file failed: " + ex.Message, PdfErrors._ERROR_DUMPDOCUMENT);
			}
		}
		public override int CurrentLocation()
		{
			return base.Length + this.m_nTotalSizeWritten;
		}
		public override void Open()
		{
			try
			{
				this.m_objFileStream = new FileStream(this.m_bstrPath, FileMode.Create, FileAccess.Write, FileShare.Read);
			}
			catch (Exception ex)
			{
				AuxException.Throw("Opening output file failed: " + ex.Message, PdfErrors._ERROR_DUMPDOCUMENT);
			}
		}
		public override void Close()
		{
			this.m_objFileStream.Close();
			this.m_objFileStream.Dispose();
		}
	}
}
