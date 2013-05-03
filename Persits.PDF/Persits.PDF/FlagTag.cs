using System;
namespace Persits.PDF
{
	internal enum FlagTag : uint
	{
		FLAG_TAG_UL = 1u,
		FLAG_TAG_OL,
		FLAG_TAG_LI = 4u,
		FLAG_TAG_BQ = 8u,
		FLAG_TAG_DIV = 16u,
		FLAG_TAG_TABLE = 32u,
		FLAG_TAG_CENTER = 64u,
		FLAG_TAG_ADDRESS = 128u,
		FLAG_TAG_P = 256u,
		FLAG_TAG_H1 = 512u,
		FLAG_TAG_TD = 1024u,
		FLAG_TAG_TH = 2048u,
		FLAG_TAG_TR = 4096u,
		FLAG_TAG_BODY = 8192u,
		FLAG_TAG_FORM = 16384u,
		FLAG_TAG_HR = 32768u,
		FLAG_TAG_CAPTION = 65536u,
		FLAG_TAG_IMG = 131072u,
		FLAG_TAG_BLOCK = 17407u,
		FLAG_TAG_IMPLICIT_P_CLOSE = 20223u,
		FLAG_TAG_OPEN_CLOSES_P = 991u,
		FLAG_TAG_TD_TH_TR = 7168u
	}
}