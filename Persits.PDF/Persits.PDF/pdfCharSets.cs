using System;
namespace Persits.PDF
{
	public enum pdfCharSets : uint
	{
		pdfAnsi,
		pdfDefault,
		pdfSymbol,
		pdfShiftJISC = 128u,
		pdfHangul,
		pdfGB2312 = 134u,
		pdfChineseBig5 = 136u,
		pdfJohab = 130u,
		pdfHebrew = 177u,
		pdfArabic,
		pdfGreek = 161u,
		pdfTurkish,
		pdfVietnamese,
		pdfThai = 222u,
		pdfEastEurope = 238u,
		pdfRussian = 204u,
		pdfMac = 77u,
		pdfBaltic = 186u
	}
}
