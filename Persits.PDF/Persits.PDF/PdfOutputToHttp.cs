using System;
using System.Web;
namespace Persits.PDF
{
	internal class PdfOutputToHttp : PdfOutputToFile
	{
		internal string m_bstrDispHeader;
		internal string m_bstrContentType;
		private HttpResponse m_pResponse;
		public PdfOutputToHttp(int InitialSize, string DispHeader, string ContentType) : base(InitialSize, null)
		{
			this.m_bstrDispHeader = DispHeader;
			this.m_bstrContentType = ContentType;
		}
		public override void Open()
		{
			if (HttpContext.Current == null)
			{
				AuxException.Throw("SaveHttp can only be called in an ASP.NET environment.", PdfErrors._ERROR_HTTP_CONTEXT);
			}
			this.m_pResponse = HttpContext.Current.Response;
			this.m_pResponse.Clear();
			this.m_pResponse.BufferOutput = false;
			this.m_pResponse.ContentType = this.m_bstrContentType;
			this.m_pResponse.AddHeader("Content-Disposition", this.m_bstrDispHeader);
		}
		public override void Write(byte[] objString, int nStart, int nCount, ref int NewSize)
		{
			if (nCount == 0)
			{
				return;
			}
			try
			{
				if (nStart == 0 && nCount == objString.Length)
				{
					this.m_pResponse.BinaryWrite(objString);
				}
				else
				{
					byte[] array = new byte[nCount];
					Array.Copy(objString, nStart, array, 0, nCount);
					this.m_pResponse.BinaryWrite(array);
				}
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
			this.m_pResponse.Flush();
		}
		public override void Close()
		{
			HttpContext.Current.ApplicationInstance.CompleteRequest();
		}
	}
}
