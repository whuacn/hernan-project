using System;
namespace Persits.PDF
{
	internal enum arabic_level
	{
		ar_nothing,
		ar_novowel,
		ar_standard,
		ar_composedtashkeel = 4,
		ar_lig = 8,
		ar_mulefont = 16,
		ar_lboxfont = 32,
		ar_unifont = 64,
		ar_naqshfont = 128
	}
}
