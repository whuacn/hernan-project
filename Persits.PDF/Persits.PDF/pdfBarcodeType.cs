using System;
namespace Persits.PDF
{
	internal enum pdfBarcodeType
	{
		barUPCA = 1,
		barUPCE,
		barEAN13,
		barEAN8,
		barUPCWithSupplemental,
		barUPCEWithSupplemental,
		barEAN13WithSupplemental,
		barEAN8WithSupplemental,
		barSupplemental2,
		barSupplemental5,
		barIndustrial25,
		barInterleave25,
		barInterleave25Special,
		barDataLogic25,
		barPlessey,
		barCodabar,
		barCode39,
		barCode11,
		barCode39Extended = 20,
		barCode93,
		barCode128,
		barCode128Raw,
		barPostal = 30,
		barUKPostal,
		barMailIntelligent
	}
}
