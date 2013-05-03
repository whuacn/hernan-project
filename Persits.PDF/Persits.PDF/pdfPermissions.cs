using System;
namespace Persits.PDF
{
	public enum pdfPermissions : uint
	{
		pdfFull = 4294967292u,
		pdfPrint = 4u,
		pdfModify = 8u,
		pdfCopy = 16u,
		pdfAnnotations = 32u,
		pdfForm = 256u,
		pdfExtract = 512u,
		pdfAssemble = 1024u,
		pdfPrintHigh = 2048u,
		pdfReserved = 4294963392u
	}
}
