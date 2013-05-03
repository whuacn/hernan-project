using System;
namespace Persits.PDF
{
	internal enum StyleFlags : uint
	{
		STYLE_NORMAL,
		STYLE_BOLD,
		STYLE_ITALIC,
		STYLE_UNDERLINED = 4u,
		STYLE_STRIKE = 8u,
		STYLE_SUB = 16u,
		STYLE_SUP = 32u,
		STYLE_ANCHOR = 64u,
		STYLE_ALIGNRIGHT = 128u,
		STYLE_ALIGNCENTER = 256u,
		STYLE_ALIGNJUSTIFY = 512u,
		STYLE_OVERLINED = 1024u,
		STYLE_ALIGNNONLEFT = 896u
	}
}
